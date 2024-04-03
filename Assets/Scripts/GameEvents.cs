using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;
    private void Awake()
    {
        // Singleton Logic
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public event Action<BoardData> OnBoardInitalize;
    public event Action<Marker> OnStartTurn;
    public event Action<BoardData, int, int> OnPlayTurn;
    public event Action<bool> OnAllowPlayerAction;
    public event Action<Marker, List<Vector2Int>> OnGameEnd;

    public void TriggerOnBoardInitialize(BoardData boardData) => OnBoardInitalize?.Invoke(boardData);
    public void TriggerOnStartTurn(Marker marker) => OnStartTurn?.Invoke(marker);
    public void TriggerOnPlayTurn(BoardData boardData, int row, int col) => OnPlayTurn?.Invoke(boardData, row, col);
    public void TriggerOnAllowPlayerAction(bool allow) => OnAllowPlayerAction?.Invoke(allow);
    public void TriggerOnGameEnd(Marker winner, List<Vector2Int> winningPositions) => OnGameEnd?.Invoke(winner, winningPositions);
}
