using UnityEngine;

public class MoveCommandQuebra : ICommand
{
    private PuzzlePiece a, b;

    public MoveCommandQuebra(PuzzlePiece p1, PuzzlePiece p2)
    {
        a = p1; b = p2;
    }

    public void Execute()
    {
        Swap();
    }

    public void Undo()
    {
        Swap(); // como a troca é simétrica
    }

    public void Do()
    {
        throw new System.NotImplementedException();
    }

    private void Swap()
    {
        Vector3 tempPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = tempPos;

        // Atualiza info de estado interno, se tiver
    }
}
