using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI startLabel;

    [Header("Debug")]
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private bool isStarted;

    private void Start()
    {
        difficulty = Difficulty.None;
        UpdateLabel(difficulty);
        isStarted = false;

        GameEvents.instance.OnGameStart += UpdateStart;
        GameEvents.instance.OnGameReset += UpdateReset;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= UpdateStart;
        GameEvents.instance.OnGameReset -= UpdateReset;
    }

    private void UpdateStart()
    {
        previousButton.enabled = false;
        nextButton.enabled = false;
        startLabel.text = "Reset";

        isStarted = true;
    }

    private void UpdateReset()
    {
        previousButton.enabled = true;
        nextButton.enabled = true;
        UpdateLabel(difficulty);

        isStarted = false;
    }

    public void Next()
    {
        int count = Enum.GetNames(typeof(Difficulty)).Length;
        int index = (int)difficulty;

        index++;
        if (index >= count)
            index = 0;

        difficulty = (Difficulty)index;

        UpdateLabel(difficulty);
    }

    public void StartGame()
    {
        if (isStarted)
            GameManager.instance.Reset();
        else
            GameManager.instance.StartGame(difficulty);
    }

    public void Previous()
    {
        int count = Enum.GetNames(typeof(Difficulty)).Length;
        int index = (int)difficulty;

        index--;
        if (index < 0)
            index = count - 1;

        difficulty = (Difficulty)index;

        UpdateLabel(difficulty);
    }

    private void UpdateLabel(Difficulty difficulty)
    {
        if (difficulty == Difficulty.None)
            startLabel.text = $"Start [vs Player]";
        else
            startLabel.text = $"Start [vs {difficulty} CPU]";
    }
}
