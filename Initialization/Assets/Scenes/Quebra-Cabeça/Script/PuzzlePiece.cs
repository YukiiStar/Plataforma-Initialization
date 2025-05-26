using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2 correctPosition;
    public Vector2 currentPosition;
    public int CurrentIndex { get; set; }
    public int CorrectIndex { get; set; }

    public bool IsInCorrectPosition()
    {
        return currentPosition == correctPosition;
    }

    public void OnMouseDown()
    {
        FindObjectOfType<InputManager>().OnPieceClicked(this);
    }

    public bool IsInRightPlace()
    {
        throw new System.NotImplementedException();
    }
}
