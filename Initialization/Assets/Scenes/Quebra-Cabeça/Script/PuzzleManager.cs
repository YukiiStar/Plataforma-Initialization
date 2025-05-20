using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public List<PuzzlePiece> allPieces;

    public bool CheckVictory()
    {
        foreach (var piece in allPieces)
        {
            if(!piece.IsInCorrectPosition()) return false;
        }
        
        return true;
    }

    public void Shuffle()
    {
        
    }
}