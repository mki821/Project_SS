using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;

    [SerializeField]
    private string mixerName;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(mixerName, 0.75f);
    }

    public void SetLevel()
    {
        audioMixer.SetFloat(mixerName, Mathf.Log10(slider.value) * 20);
        PlayerPrefs.SetFloat(mixerName, slider.value);
    }
}
