using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public Transform puzzleGrid; // Painel que contém as peças
    public Button undoButton;    // Botão desfazer
    public Button replayButton;  // Botão de replay
    public Button skipButton;    // Botão de pular replay
    public GameObject Vitoria;   // Painel de vitória
    public Button playAgainButton;

    private List<ICommand2> commandHistory = new List<ICommand2>(); // Histórico de comandos
    private Stack<ICommand2> undoStack = new Stack<ICommand2>();    // Stack para desfazer
    private List<Piece> pieces = new List<Piece>(); // Lista das peças
    private Piece firstSelected = null;             // Primeira peça selecionada

    private bool isReplaying = false; // Se está em replay

    void Start()
    {
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);

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
        Debug.Log(puzzleGrid.childCount);
        for (int i = 0; i < pieces.Count; i++)
        {
            Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
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

        skipButton.gameObject.SetActive(true); // Mostra o botão skip
        Vitoria.SetActive(false);
        StartCoroutine(ReplaySequence());
    }

    IEnumerator ReplaySequence()
    {
        isReplaying = true;
        skipButton.gameObject.SetActive(true); // Mantém o botão skip ativo
        Vitoria.SetActive(false);

        ShufflePieces();
        yield return new WaitForSeconds(1f);

        foreach (ICommand2 cmd in commandHistory)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
        skipButton.gameObject.SetActive(true); // Mantém o botão skip ativo após o replay
        Debug.Log("✅ Replay finalizado!");
        CheckWin();
    }

    public void SkipReplay()
    {
        if (!isReplaying) return;

        StopAllCoroutines();
        isReplaying = false;

        foreach (ICommand2 cmd in commandHistory)
        {
            cmd.Execute();
        }

        skipButton.gameObject.SetActive(true); // Mantém o botão skip ativo após o skip
        Debug.Log("⏩ Replay pulado!");
        CheckWin();
    }
    
    public void PlayAgain()
    {
        Vitoria.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        commandHistory.Clear();
        undoStack.Clear();

        SetupPieces();
        ShufflePieces();

        firstSelected = null;
        isReplaying = false;

        Debug.Log("🔁 Novo jogo iniciado!");
    }
} 
  