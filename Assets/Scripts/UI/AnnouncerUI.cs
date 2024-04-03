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
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnStartTurn -= UpdateUI;
        GameEvents.instance.OnGameEnd -= UpdateUI;
    }

    private void UpdateUI(Marker marker)
    {
        announcerLabel.text = $"{marker}'s Turn";
    }

    private void UpdateUI(Marker marker, List<Vector2Int> winningPositions)
    {
        announcerLabel.text = winningPositions.Count == 0 ? "Draw!" : $"{marker} Wins!";
    }
}
