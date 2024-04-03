using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<MarkerUI> markerUIs;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        GameEvents.instance.OnBoardInitalize += UpdateUI;
        GameEvents.instance.OnAllowPlayerAction += UpdateUI;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnBoardInitalize -= UpdateUI;
        GameEvents.instance.OnAllowPlayerAction -= UpdateUI;
    }

    private void UpdateUI(BoardData boardData)
    {
        for (int i = 0; i < boardData.Width; i++)
        {
            for (int j = 0; j < boardData.Height; j++)
            {
                int index = i * boardData.Width + j;
                markerUIs[index].Initialize(i, j, boardData[i, j]);
            }
        }
    }

    private void UpdateUI(bool allow)
    {
        // Set interaction state
        canvasGroup.blocksRaycasts = allow;
    }
}
