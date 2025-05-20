using UnityEngine;

public class MoveCommandQuebra : MonoBehaviour, ICommand
{
    private PuzzlePiece a, b;

    public MoveCommandQuebra(PuzzlePiece p1, PuzzlePiece p2)
    {
        a = p1;
        b = p2;
    }

    public void Execute()
    {
        Swap();
    }

    public void Undo()
    {
        Swap();
    }

    private void Swap()
    {
        Vector3 temPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = temPos;
    }
}
