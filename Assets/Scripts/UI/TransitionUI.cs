using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas playerScreen;
    [SerializeField] private Image blackScreen;

    [Header("Settings")]
    [SerializeField] private float transitionDuration;

    private Vector2 _targetCanvasPos;

    private Coroutine coroutine;
    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
    private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

    private void Awake()
    {
        // _canvas = GetComponent<Canvas>();
        // _blackScreen = GetComponentInChildren<Image>();
    }

    public void Initialize()
    {
        DrawBlackScreen();
    }

    public void OpenBlackScreen()
    {
        DrawBlackScreen();

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(TransitionOverTime(transitionDuration, 0, 1));
    }

    public void CloseBlackScreen()
    {
        DrawBlackScreen();

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(TransitionOverTime(transitionDuration, 1, 0));
    }

    private void DrawBlackScreen()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        // Need a target
        var positon = Vector3.zero;
        var playerScreenPos = Camera.main.WorldToScreenPoint(positon);

        // To Draw to Image to Full Screen, we get the Canvas Rect size
        var canvasRect = playerScreen.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        // But because the Black Screen is now square (different to Screen). So we much added the different of width/height to it
        // Now we convert Screen Pos to Canvas Pos
        _targetCanvasPos = new Vector2
        {
            x = playerScreenPos.x / screenWidth * canvasWidth,
            y = playerScreenPos.y / screenHeight * canvasHeight,
        };

        var squareValue = 0f;
        if (canvasWidth > canvasHeight)
        {
            // Landscape
            squareValue = canvasWidth;
            _targetCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
        }
        else
        {
            // Portrait            
            squareValue = canvasHeight;
            _targetCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
        }

        _targetCanvasPos /= squareValue;

        var mat = blackScreen.material;
        mat.SetFloat(CENTER_X, _targetCanvasPos.x);
        mat.SetFloat(CENTER_Y, _targetCanvasPos.y);

        blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);

        // Now we want the circle to follow the player position
        // So First, we must get the player world position, convert it to screen position, and normalize it (0 -> 1)
        // And input into the shader
    }

    private IEnumerator TransitionOverTime(float duration, float beginRadius, float endRadius)
    {
        var mat = blackScreen.material;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            var t = elapsed / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);
            mat.SetFloat(RADIUS, radius);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat(RADIUS, endRadius);
    }
}