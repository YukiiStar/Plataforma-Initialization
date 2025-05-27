using UnityEngine;

public class Piece : MonoBehaviour
{
    public int correctIndex; // Posição correta da peça no puzzle
    public PuzzleManager manager;
    public void SetIndex(int index)
    {
        correctIndex = index;
    }
    public void OnClick()
    {
        manager.OnPieceClicked(this);
    }
}
