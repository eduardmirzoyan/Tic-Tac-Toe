using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarkerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Image spriteImage;
    [SerializeField] private Image hoverImage;

    [Header("Data")]
    [SerializeField] private Sprite[] sprites;

    [Header("Debug")]
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private bool allowInteraction;

    private void Start()
    {
        // Sub
        GameEvents.instance.OnStartTurn += UpdateUI;
        GameEvents.instance.OnPlayTurn += UpdateUI;
        GameEvents.instance.OnGameEnd += UpdateUI;
        GameEvents.instance.OnGameReset += UpdateUI;
    }

    private void OnDestroy()
    {
        // Unsub
        GameEvents.instance.OnStartTurn -= UpdateUI;
        GameEvents.instance.OnPlayTurn -= UpdateUI;
        GameEvents.instance.OnGameEnd -= UpdateUI;
        GameEvents.instance.OnGameReset -= UpdateUI;
    }

    public void Initialize(int row, int col, Marker marker)
    {
        this.row = row;
        this.col = col;
        SetSprite(marker);
        hoverImage.enabled = false;

        gameObject.name = $"Marker [{row}, {col}] ({marker})";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Only allow Left-Click
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Play
        GameManager.instance.PlayTurn(row, col);
    }

    private void SetSprite(Marker marker)
    {
        if (marker == Marker.None || marker == Marker.Draw)
        {
            spriteImage.color = Color.clear;
            spriteImage.raycastTarget = true;
            return;
        }

        // Set sprite based on enum
        spriteImage.sprite = sprites[(int)marker - 1];
        spriteImage.color = Color.white;

        // Prevent interaction
        spriteImage.raycastTarget = false;
    }

    private void UpdateUI(Marker marker)
    {
        // Set hover based on marker
        hoverImage.sprite = sprites[(int)marker - 1];
    }

    private void UpdateUI(BoardData boardData, int row, int col)
    {
        // Ignore other indices
        if (this.row != row || this.col != col)
            return;

        // Update sprite
        Marker marker = boardData[row, col];
        SetSprite(marker);
    }

    private void UpdateUI(Marker _, List<Vector2Int> winningPositions)
    {
        if (winningPositions.Count == 0)
        {
            spriteImage.color = Color.gray;
        }
        else
        {
            // If this is any of the winning positions
            if (spriteImage.color != Color.clear)
                spriteImage.color = winningPositions.Any(position => position.x == row && position.y == col) ? Color.red : Color.gray;
        }
    }

    private void UpdateUI()
    {
        // Reset
        SetSprite(Marker.None);
        hoverImage.enabled = false;
    }
}
