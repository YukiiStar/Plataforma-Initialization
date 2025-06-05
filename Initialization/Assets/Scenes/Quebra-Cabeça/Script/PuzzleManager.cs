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
    public Button playAgainButton;
    
    private List<ICommand2> commandHistory = new List<ICommand2>(); // Histórico de comandos
    private Stack<ICommand2> undoStack = new Stack<ICommand2>();    // Pilha de desfazer
    private List<Piece> pieces = new List<Piece>();                 // Lista de peças
    private Piece firstSelected = null;                             // Primeira peça clicada
    private bool isReplaying = false;                               // Flag para saber se está em replay

    void Start()
    {
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
        Vitoria.SetActive(false);

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

    void CheckWin()
    {
        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return;
        }

        Debug.Log("🎉 Quebra-cabeça completo!");
        Vitoria.SetActive(true);
        replayButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
    }

    public void Undo()
    {
        if (isReplaying || undoStack.Count == 0 || firstSelected != null) return;
        ICommand2 lastCommand = undoStack.Pop();
        lastCommand.Undo();
    }

    public void StartReplay()
    {
        if (isReplaying) return;
        Vitoria.SetActive(false);
        skipButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(false);
        StartCoroutine(ReplaySequence());
    }

    IEnumerator ReplaySequence()
    {
        isReplaying = true;
        skipButton.gameObject.SetActive(true);

        ShufflePieces();
        yield return new WaitForSeconds(1f);

        foreach (ICommand2 cmd in commandHistory)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
        Debug.Log("✅ Replay finalizado!");
        // Chama CheckWin para manter o estado correto
        CheckWin();
    }

    public void SkipReplay()
    {
        if (!isReplaying) return;

        StopAllCoroutines();
        isReplaying = false;

        // Executa todas as jogadas imediatamente
        foreach (ICommand2 cmd in commandHistory)
        {
            cmd.Execute();
        }

        Debug.Log("⏩ Replay pulado!");

        // NÃO reativa a tela de vitória
        Vitoria.SetActive(false); // Oculta se estivesse ativa
        skipButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(true); // Agora o puzzle está montado e visível ao jogador, sem a tela de vitória em cima
    }


    public void PlayAgain()
    {
        Vitoria.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);

        ShufflePieces();
        commandHistory.Clear();
        undoStack.Clear();
        firstSelected = null;
    }
}



  