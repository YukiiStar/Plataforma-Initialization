using UnityEngine;
public class SwapCommand : ICommand2
{
    private Transform pieceA;
    private Transform pieceB;
    private int indexA;
    private int indexB;
    
    public SwapCommand(Transform a, Transform b, int aIndex, int bIndex)
    {
        pieceA = a;
        pieceB = b;
        indexA = aIndex;
        indexB = bIndex;
    }
    public void Execute()
    {
        pieceA.SetSiblingIndex(indexB);
        pieceB.SetSiblingIndex(indexA);
        Debug.Log("executou");
    }
    
    public void Undo()
    {
        pieceA.SetSiblingIndex(indexA);
        pieceB.SetSiblingIndex(indexB);
        Debug.Log("desfez");
    }
}