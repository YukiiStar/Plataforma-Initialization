using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public Transform puzzleGrid;      // Painel que contém as peças
    public Button undoButton;         // Botão de desfazer jogada
    public Button replayButton;       // Botão de assistir replay
    public Button skipButton;         // Botão de pular o replay
    public GameObject Vitoria;        // Painel de vitória (onde está também o botão de jogar novamente)
    public Button restartButton;

    public Button playAgainButton;

    private List<ICommand2> commandHistory = new List<ICommand2>(); // Histórico de comandos
    private Stack<ICommand2> undoStack = new Stack<ICommand2>();    // Pilha de desfazer
    
    private List<Piece> pieces = new List<Piece>();                 // Lista de peças
    private Piece firstSelected = null;                             // Primeira peça clicada
    
    private bool isReplaying = false;                               // Flag para saber se está em replay

    private List<Transform> initialPieceOrder = new List<Transform>(); // Ordem inicial salv

    void Start()
    {
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        Vitoria.SetActive(false);
        restartButton.gameObject.SetActive(false);

        SetupPieces();
        ShufflePieces();
    }

    void SetupPieces()
    {
        pieces.Clear();
        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            Transform t = puzzleGrid.GetChild(i);
            Piece p = t.GetComponent<Piece>();
            p.manager = this;
            p.SetIndex(i);
            pieces.Add(p);
        }
    }
    void RestoreInitialOrder()
    {
        for (int i = 0; i < initialPieceOrder.Count; i++)
        {
            initialPieceOrder[i].SetSiblingIndex(i);
        }
    }
    void ShufflePieces()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int randomIndex = Random.Range(0, pieces.Count);
            Transform a = pieces[i].transform;
            Transform b = pieces[randomIndex].transform;

            int indexA = a.GetSiblingIndex();
            int indexB = b.GetSiblingIndex();
            a.SetSiblingIndex(indexB);
            b.SetSiblingIndex(indexA);
        }
    }

    public void OnPieceClicked(Piece clicked)
    {
        if (isReplaying) return;
        
        if (commandHistory.Count == 0)
        {
            SaveInitialSiblingOrder();
        }
        if (firstSelected == null)
        {
            firstSelected = clicked;
        }
        else
        {
            Transform a = firstSelected.transform;
            Transform b = clicked.transform;

            int indexA = a.GetSiblingIndex();
            int indexB = b.GetSiblingIndex();

            SwapCommand cmd = new SwapCommand(a, b, indexA, indexB);
            cmd.Execute();

            commandHistory.Add(cmd);
            undoStack.Push(cmd);

            firstSelected = null;
            CheckWin();
        }
    }
    void SaveInitialSiblingOrder()
    {
        initialPieceOrder.Clear();
        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            initialPieceOrder.Add(puzzleGrid.GetChild(i));
        }
    }
    
    void RestoreInitialSiblingOrder()
    {
        for (int i = 0; i < initialPieceOrder.Count; i++)
        {
            initialPieceOrder[i].SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleGrid.GetComponent<RectTransform>());
    }

    void CheckWin()
    {
        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return;
        }

        Debug.Log(" Quebra-cabeça completo!");
        ShowVictoryScreen();
    }
    void ShowVictoryScreen() // Exibe painel de vitória
    {
        Vitoria.SetActive(true);
        restartButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(true);
        BringButtonsToFront();
    }
    public void Undo()
    {
        if (isReplaying || undoStack.Count == 0 || firstSelected != null) return;
        ICommand2 lastCommand = undoStack.Pop();
        lastCommand.Undo();
        
        if (commandHistory.Count > 0)
        {
            commandHistory.RemoveAt(commandHistory.Count - 1);
        }

        if (!IsPuzzleComplete())
        {
            Vitoria.SetActive(false);
        }
    }
    
    bool IsPuzzleComplete()
    {
        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return false;
        }
        return true;
    }
    public void StartReplay()
    {
        if (isReplaying) return;
        StartCoroutine(ReplaySequence());
        Vitoria.SetActive(false);
    }

    IEnumerator ReplaySequence()
    {
        isReplaying = true;
        
        Vitoria.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);

        RestoreInitialOrder();
        yield return new WaitForSeconds(1f);

        foreach (ICommand2 cmd in commandHistory)
        {
            if (!isReplaying) yield break;
            
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
        ShowVictoryScreen();
        Debug.Log("✅ Replay finalizado!");
    }

    public void SkipReplay()
    {
        StopAllCoroutines();
        isReplaying = true;

        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            for (int j = 0; j < puzzleGrid.childCount; j++)
            {
                Piece piece = puzzleGrid.GetChild(j).GetComponent<Piece>();
                if (piece.correctIndex == i)
                {
                    piece.transform.SetSiblingIndex(i);
                    break;
                }
            }
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleGrid.GetComponent<RectTransform>());
        isReplaying = false;
        
        ShowVictoryScreen();
        Debug.Log(" Replay pulado: peças montadas corretamente.");
    }
    
    public void RestartGame()
    {
        ShufflePieces();
        commandHistory.Clear();
        undoStack.Clear();
        initialPieceOrder.Clear();
        
        Vitoria.SetActive(false);
        restartButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(true);
        
        Debug.Log("🔁 Jogo reiniciado.");
    }
    void BringButtonsToFront()
    {
        restartButton.transform.SetAsLastSibling();
        replayButton.transform.SetAsLastSibling();
        skipButton.transform.SetAsLastSibling();
        undoButton.transform.SetAsLastSibling();
    }
  
}
