using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearMessage : MonoBehaviour
{
    private int _time;
    private int _score;

    private TextMeshProUGUI tmpro;

    private void Awake()
    {
        _time = UIManager.instance.Time;
        _score = UIManager.instance.Score;

        tmpro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        int t1 = _time / 60;
        int t2 = _time % 60;
        tmpro.text = $"Time: {t1:D2}:{t2:D2} ¸¸Å­ ¹öÆ¼°í\n{_score:D6} Á¡À» È¹µæÇÏ¼Ì½À´Ï´Ù!";
    }

    public void Main()
    {
        SceneManager.LoadScene(0);
    }
}
