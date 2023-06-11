using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource[] audios;
    public AudioSource[] bgms;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);

        audios = transform.Find("SFX").GetComponentsInChildren<AudioSource>();
        bgms = transform.Find("BGM").GetComponentsInChildren<AudioSource>();

        PlayBGM("BGM"); 
    }

    public void PlaySound(string name)
    {
        foreach (var item in audios)
        {
            if (item.name != name) continue;

            item.Play();
            break;
        }
    }

    public void PlayBGM(string name)
    {
        foreach(var item in bgms)
        {
            if (item.name != name) continue;

            item.Play();
            break;
        }
    }

    public void StopBGM(string name)
    {
        foreach (var item in bgms)
        {
            if (item.name != name) continue;

            item.Stop();
            break;
        }
    }
}
