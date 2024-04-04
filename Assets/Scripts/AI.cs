using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { None, Easy, Medium, Hard }

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

            case Difficulty.Medium:

                return MediumAI(boardData, marker);

            case Difficulty.Hard:

                return HardAI(boardData);

            default:
                throw new System.Exception($"Diffculty={difficulty} not implemented.");
        }
    }

    private Vector2Int MediumAI(BoardData boardData, Marker marker)
    {
        // Logic: If any possible wins exist, go for it, else choose randomly.

        // Make list of open positions
        List<Vector2Int> validPositions = new();
        for (int i = 0; i < boardData.Width; i++)
            for (int j = 0; j < boardData.Height; j++)
                if (boardData[i, j] == Marker.None)
                    validPositions.Add(new(i, j));

        // Error check
        if (validPositions.Count == 0)
            throw new System.Exception("No valid positions found!");

        // Check if any positions lead to a win
        foreach (var position in validPositions)
        {
            // Make copy
            var copy = boardData.Copy();

            // Make move and check win
            copy[position.x, position.y] = marker;

            bool win = CheckWin(copy);
            if (win)
                return position;
        }

        // Choose one at random
        return validPositions[Random.Range(0, validPositions.Count)];
    }

    private Vector2Int HardAI(BoardData boardData)
    {
        // Logic full blown logic

        var bestMove = FindBestMove(boardData);


        return new(bestMove.Item1, bestMove.Item2);
    }

    private bool CheckWin(BoardData boardData)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
            if (boardData[row, 0] != Marker.None && boardData[row, 0] == boardData[row, 1] && boardData[row, 1] == boardData[row, 2])
                return true;

        // Check columns
        for (int col = 0; col < 3; col++)
            if (boardData[0, col] != Marker.None && boardData[0, col] == boardData[1, col] && boardData[1, col] == boardData[2, col])
                return true;

        // Check diagonals
        if (boardData[0, 0] != Marker.None && boardData[0, 0] == boardData[1, 1] && boardData[1, 1] == boardData[2, 2])
            return true;
        if (boardData[0, 2] != Marker.None && boardData[0, 2] == boardData[1, 1] && boardData[1, 1] == boardData[2, 0])
            return true;

        // No win
        return false;
    }



    // ================================

    // Minimax function to determine the best move
    private int Minimax(BoardData boardData, int depth, bool isMaximizing)
    {
        if (CheckWin(boardData))
        {
            // If the game is over, return the score
            if (isMaximizing)
                return -1; // minimizing player wins
            else
                return 1; // maximizing player wins
        }

        // If it's maximizing player's turn
        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < boardData.Width; i++)
            {
                for (int j = 0; j < boardData.Height; j++)
                {
                    if (boardData[i, j] == Marker.None)
                    {
                        boardData[i, j] = Marker.X;
                        int score = Minimax(boardData, depth + 1, false);
                        boardData[i, j] = Marker.None;
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        // If it's minimizing player's turn
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < boardData.Width; i++)
            {
                for (int j = 0; j < boardData.Height; j++)
                {
                    if (boardData[i, j] == Marker.None)
                    {
                        boardData[i, j] = Marker.O;
                        int score = Minimax(boardData, depth + 1, true);
                        boardData[i, j] = Marker.None;
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    // Function to find the best move using minimax
    public (int, int) FindBestMove(BoardData boardData)
    {
        int bestScore = int.MinValue;
        (int, int) bestMove = (-1, -1);

        for (int i = 0; i < boardData.Width; i++)
        {
            for (int j = 0; j < boardData.Height; j++)
            {
                var copy = boardData.Copy();
                if (copy[i, j] == Marker.None)
                {
                    copy[i, j] = Marker.X;
                    int score = Minimax(copy, 0, false);
                    copy[i, j] = Marker.O;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (i, j);
                    }
                }
            }
        }
        return bestMove;
    }
}
