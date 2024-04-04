using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Sprite[] audioSprites;

    private bool isOSTMuted;

    public void ToggleOST()
    {
        isOSTMuted = !isOSTMuted;
        if (isOSTMuted)
            iconImage.sprite = audioSprites[3];
        else
            iconImage.sprite = audioSprites[2];

        float volume = isOSTMuted ? -80f : 0f;
        audioMixer.SetFloat("OSTVolume", volume);
    }
}
