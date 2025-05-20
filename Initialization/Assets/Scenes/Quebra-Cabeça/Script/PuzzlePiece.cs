using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2 correctPosition;
    public Vector2 currentPosition;

    public bool IsInCorrectPosition()
    {
        return currentPosition == correctPosition;
    }

    public void OnMouseDown()
    {
        FindObjectOfType<InputManager>().OnPieceClicked(this);
    }
}
