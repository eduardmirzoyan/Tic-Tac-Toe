using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnnouncerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI announcerLabel;

    private void Start()
    {
        GameEvents.instance.OnStartTurn += UpdateUI;
        GameEvents.instance.OnGameEnd += UpdateUI;
        GameEvents.instance.OnGameReset += UpdateUI;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnStartTurn -= UpdateUI;
        GameEvents.instance.OnGameEnd -= UpdateUI;
        GameEvents.instance.OnGameReset -= UpdateUI;
    }

    private void UpdateUI(Marker marker)
    {
        announcerLabel.text = $"{marker}'s Turn";

        // Play audio
        AudioManager.instance.PlaySFX("Place Marker");
    }

    private void UpdateUI(Marker marker, List<Vector2Int> winningPositions)
    {
        announcerLabel.text = winningPositions.Count == 0 ? "Draw!" : $"{marker} Wins!";

        // Play audio
        AudioManager.instance.PlaySFX("Game Over");
    }

    private void UpdateUI()
    {
        announcerLabel.text = "Tic-Tac-Toe";
    }
}
