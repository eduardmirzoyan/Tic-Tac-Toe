using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class MenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI startLabel;
    [SerializeField] private Outline startOutline;

    [Header("Settings")]
    [SerializeField] private float transitionDuration = 0.25f;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Debug")]
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private bool isStarted;

    private void Start()
    {
        difficulty = Difficulty.None;
        UpdateLabel(difficulty);
        isStarted = false;

        GameEvents.instance.OnGameStart += UpdateStart;
        GameEvents.instance.OnGameEnd += UpdateEnd;
        GameEvents.instance.OnGameReset += UpdateReset;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= UpdateStart;
        GameEvents.instance.OnGameEnd -= UpdateEnd;
        GameEvents.instance.OnGameReset -= UpdateReset;
    }

    #region Event

    private void UpdateStart()
    {
        SetNavigationButtons(false);
        startLabel.text = "Reset";

        startOutline.enabled = false;
        startOutline.effectColor = Color.clear;

        isStarted = true;
    }

    private void UpdateReset()
    {
        SetNavigationButtons(true);
        UpdateLabel(difficulty);

        startOutline.enabled = true;
        startOutline.effectColor = startColor;

        isStarted = false;
    }

    private void UpdateEnd(Marker _, List<Vector2Int> __)
    {
        startOutline.enabled = true;
        startOutline.effectColor = endColor;
    }

    #endregion

    #region Button

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

    #endregion

    #region Helpers

    private void SetNavigationButtons(bool show)
    {
        if (show)
        {
            LeanTween.scale(nextButton.gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(previousButton.gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeOutQuad);
        }
        else
        {
            LeanTween.scale(nextButton.gameObject, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInQuad);
            LeanTween.scale(previousButton.gameObject, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInQuad);
        }
    }

    private void UpdateLabel(Difficulty difficulty)
    {
        if (difficulty == Difficulty.None)
            startLabel.text = $"Start [2 Player]";
        else
            startLabel.text = $"Start [{difficulty} CPU]";
    }

    #endregion
}
