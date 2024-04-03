using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Easy, Medium, Hard }

public class AI : ScriptableObject
{
    public Difficulty difficulty;

    public void Initialize(Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    public Vector2Int DetermineBestMove(BoardData boardData, Marker marker)
    {
        switch (difficulty)
        {
            case Difficulty.Easy: // Make random valid move

                // Make list of open positions
                List<Vector2Int> validPositions = new();
                for (int i = 0; i < boardData.Width; i++)
                    for (int j = 0; j < boardData.Height; j++)
                        if (boardData[i, j] == Marker.None)
                            validPositions.Add(new(i, j));

                // Error check
                if (validPositions.Count == 0)
                    throw new System.Exception("No valid positions found!");

                // Choose one at random
                return validPositions[Random.Range(0, validPositions.Count)];

            default:
                throw new System.Exception($"Diffculty={difficulty} not implemented.");
        }
    }
}
