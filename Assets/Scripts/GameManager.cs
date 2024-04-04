using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum Marker { None, X, O, Draw }

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private BoardData boardData;
    [SerializeField] private Marker currentTurn;
    [SerializeField] private AI ai;

    public static GameManager instance;
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

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        // Allow objects to initialize
        yield return new WaitForEndOfFrame();

        Reset();

        // Open scene
        TransitionManager.instance.Initialize();
        TransitionManager.instance.OpenScene();
    }

    #region Public Functions

    public void StartGame(Difficulty difficulty)
    {
        // Setup board
        Setup(difficulty);

        // Update UI
        GameEvents.instance.TriggerOnGameStart();
    }

    public void Reset()
    {
        // Update UI
        GameEvents.instance.TriggerOnGameReset();
    }

    public void PlayTurn(int row, int col)
    {
        if (boardData[row, col] != Marker.None)
        {
            print($"Attempted to change set position: {row}, {col}");
            return;
        }

        // Set location to marker
        boardData[row, col] = currentTurn;

        // Update UI
        GameEvents.instance.TriggerOnPlayTurn(boardData, row, col);

        // Check for win
        var result = CheckGameoverState(boardData);
        if (result == null) // Null means no winner yet
        {
            ChangeTurn();
        }
        else if (result.Count == 0) // count of 0 means draw
        {
            // Prevent player
            GameEvents.instance.TriggerOnAllowPlayerAction(false);
            GameEvents.instance.TriggerOnGameEnd(currentTurn, result);
        }
        else if (result.Count == 3) // count of 3 means winner 
        {
            // Prevent player
            GameEvents.instance.TriggerOnAllowPlayerAction(false);
            GameEvents.instance.TriggerOnGameEnd(currentTurn, result);
        }
        else throw new System.Exception("Undefined number of winners? " + result.Count);
    }

    #endregion

    #region Private Functions

    private void Setup(Difficulty difficulty)
    {
        // Initialize board
        boardData = ScriptableObject.CreateInstance<BoardData>();
        InitializeBoard(boardData);
        GameEvents.instance.TriggerOnBoardInitialize(boardData);

        // Create easy AI
        ai = ScriptableObject.CreateInstance<AI>();
        ai.Initialize(difficulty);

        // X starts
        currentTurn = Marker.X;
        GameEvents.instance.TriggerOnStartTurn(currentTurn);
        GameEvents.instance.TriggerOnAllowPlayerAction(true);
    }

    private void InitializeBoard(BoardData boardData)
    {
        int width = 3, height = 3;
        boardData.grid = new Marker[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                boardData[i, j] = Marker.None;
            }
        }
    }

    private void ChangeTurn()
    {
        // Update turn
        if (currentTurn == Marker.X)
        {
            currentTurn = Marker.O;
            GameEvents.instance.TriggerOnStartTurn(currentTurn);

            // If no ai, then let player go
            if (ai.difficulty == Difficulty.None)
            {
                // Allow player to play
                GameEvents.instance.TriggerOnAllowPlayerAction(true);
            }
            else
            {
                // Prevent player interference
                GameEvents.instance.TriggerOnAllowPlayerAction(false);

                // Allow AI to play
                Vector2Int move = ai.DetermineBestMove(boardData, currentTurn);
                PlayTurn(move.x, move.y);
            }

        }
        else if (currentTurn == Marker.O)
        {
            currentTurn = Marker.X;
            GameEvents.instance.TriggerOnStartTurn(currentTurn);

            // Allow player to play
            GameEvents.instance.TriggerOnAllowPlayerAction(true);
        }
    }

    private List<Vector2Int> CheckGameoverState(BoardData boardData)
    {
        // Check rows
        for (int row = 0; row < 3; row++)
            if (boardData[row, 0] != Marker.None && boardData[row, 0] == boardData[row, 1] && boardData[row, 1] == boardData[row, 2])
                return new List<Vector2Int>() { new(row, 0), new(row, 1), new(row, 2) };

        // Check columns
        for (int col = 0; col < 3; col++)
            if (boardData[0, col] != Marker.None && boardData[0, col] == boardData[1, col] && boardData[1, col] == boardData[2, col])
                return new List<Vector2Int>() { new(0, col), new(1, col), new(2, col) };

        // Check diagonals
        if (boardData[0, 0] != Marker.None && boardData[0, 0] == boardData[1, 1] && boardData[1, 1] == boardData[2, 2])
            return new List<Vector2Int>() { new(0, 0), new(1, 1), new(2, 2) };
        if (boardData[0, 2] != Marker.None && boardData[0, 2] == boardData[1, 1] && boardData[1, 1] == boardData[2, 0])
            return new List<Vector2Int>() { new(0, 2), new(1, 1), new(2, 0) };

        // Check for draw
        bool hasEmptyCell = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (boardData[i, j] == Marker.None)
                {
                    hasEmptyCell = true;
                    goto Done;
                }
            }
        }

    Done:
        if (!hasEmptyCell)
            return new List<Vector2Int>();

        // No win
        return null;
    }

    #endregion
}