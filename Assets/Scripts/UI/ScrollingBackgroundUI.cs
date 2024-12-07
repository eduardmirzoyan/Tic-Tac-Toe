using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackgroundUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RawImage backgroundRawImage;

    [Header("Settings")]
    [SerializeField] private Vector2 scrollSpeed;

    private void Update()
    {
        Vector2 curPosition = backgroundRawImage.uvRect.position;
        Vector2 newPositon = curPosition + scrollSpeed * Time.deltaTime;
        Vector2 size = backgroundRawImage.uvRect.size;

        backgroundRawImage.uvRect = new Rect(newPositon, size);
    }
}
