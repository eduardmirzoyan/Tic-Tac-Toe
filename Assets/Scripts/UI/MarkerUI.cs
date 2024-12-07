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
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Color hoverColor;

    [Header("Debug")]
    [SerializeField] private int row;
    [SerializeField] private int col;

    private void Start()
    {
        // Sub
        GameEvents.instance.OnStartTurn += InitMaker;
        GameEvents.instance.OnPlayTurn += PlayMarker;
        GameEvents.instance.OnGameEnd += EndMarker;
        GameEvents.instance.OnGameReset += ClearMarker;
    }

    private void OnDestroy()
    {
        // Unsub
        GameEvents.instance.OnStartTurn -= InitMaker;
        GameEvents.instance.OnPlayTurn -= PlayMarker;
        GameEvents.instance.OnGameEnd -= EndMarker;
        GameEvents.instance.OnGameReset -= ClearMarker;
    }

    public void Initialize(int row, int col, Marker marker)
    {
        this.row = row;
        this.col = col;
        SetSprite(marker);
        SetHover(false);

        gameObject.name = $"Marker [{row}, {col}] ({marker})";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHover(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Only allow Left-Click
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Play
        GameManager.instance.PlayTurn(row, col);
    }

    private void InitMaker(Marker marker)
    {
        // Set hover based on marker
        hoverImage.sprite = sprites[(int)marker - 1];
    }

    private void PlayMarker(BoardData boardData, int row, int col)
    {
        // Ignore other indices
        if (this.row != row || this.col != col)
            return;

        // Update sprite
        Marker marker = boardData[row, col];
        SetSprite(marker);
    }

    private void EndMarker(Marker _, List<Vector2Int> winningPositions)
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

    private void ClearMarker()
    {
        // Reset
        SetSprite(Marker.None);
        SetHover(false);
    }

    #region Helpers

    private void SetSprite(Marker marker)
    {
        if (marker == Marker.None || marker == Marker.Draw)
        {
            hoverImage.raycastTarget = true;
            LeanTween.scale(spriteImage.gameObject, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInQuad);
            return;
        }
        else
        {
            // Set sprite based on enum
            spriteImage.sprite = sprites[(int)marker - 1];
            spriteImage.color = Color.white;

            // Prevent interaction
            hoverImage.raycastTarget = false;

            // Fade in
            spriteImage.transform.localScale = Vector3.zero;
            LeanTween.scale(spriteImage.gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeOutQuad);
        }
    }

    private void SetHover(bool state)
    {
        hoverImage.color = state ? hoverColor : Color.clear;
    }

    #endregion
}
