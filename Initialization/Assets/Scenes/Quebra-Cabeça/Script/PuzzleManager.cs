using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public List<PuzzlePiece> pieces = new List<PuzzlePiece>();
    public GameObject piecePrefab;
    public Transform gridParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Shuffle();
    }

    public void SwapPieces(PuzzlePiece a, PuzzlePiece b)
    {
        // Troca posições visuais
        Vector3 tempPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = tempPos;

        // Troca índices
        int tempIndex = a.CurrentIndex;
        a.CurrentIndex = b.CurrentIndex;
        b.CurrentIndex = tempIndex;

        // Verifica vitória
        CheckVictory();
    }

    public void Shuffle()
    {
        // Algoritmo simples de embaralhamento
        for (int i = 0; i < pieces.Count; i++)
        {
            int rand = Random.Range(i, pieces.Count);
            SwapVisual(pieces[i], pieces[rand]);
        }
    }

    private void SwapVisual(PuzzlePiece a, PuzzlePiece b)
    {
        // Troca visual apenas para embaralhar no início (não registra comando)
        Vector3 tempPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = tempPos;

        int tempIndex = a.CurrentIndex;
        a.CurrentIndex = b.CurrentIndex;
        b.CurrentIndex = tempIndex;
    }

    private void CheckVictory()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsInRightPlace())
                return;
        }

        // Se todas estão no lugar certo
        UIManager.Instance.ShowVictory();
    }
}