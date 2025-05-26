using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PuzzlePiece selectedPiece;
    public CommandInvokerr invoker;
    public static object Instance { get; set; }

    public void OnPieceClicked(PuzzlePiece piece)
    {
        if (selectedPiece == null)
        {
            selectedPiece = piece;
        }
        else
        {
            ICommand move = new MoveCommandQuebra(selectedPiece, piece);
            invoker.ExecuteCommand(move);
            selectedPiece = null;

            // Verifica vitória aqui
        }
    }
}