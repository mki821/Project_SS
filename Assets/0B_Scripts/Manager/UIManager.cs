using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    private int _score = 0;
    private int _time = 0;
    private bool timeFlow = true;

    private IEnumerator TU;

    public int Score 
    {
        get => _score;
        set
        {
            _score = value;
            UISet();
        }
    }
    public int Time { get => _time; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += LoadedSceneEvent;
    }

    public void LoadedSceneEvent(Scene scene, LoadSceneMode loadSceneMode)
    {
        TU = TimeUpdate();

        if (scene.name == "SampleScene" && UIManager.instance != null)
        {
            timeFlow = true;
            _time = 0;
            _score = 0;

            StartCoroutine(TU);
        }
    }

    private IEnumerator TimeUpdate()
    {
        while (timeFlow)
        {
            yield return new WaitForSeconds(1f);

            _time+= 1;
            UISet();

            if (_time >= 3599)
            {
                GameEnd();
            }
        }
    }

    public void GameEnd()
    {
        GameManager.instance.gameEnd = true;
        StopAllCoroutines();

        foreach (var item in GameManager.instance.transform.GetComponentsInChildren<PoolMono>())
        {
            PoolManager.instance.Push(item);
        }

        timeFlow = false;
        CancelInvoke();
        SceneM.instance.SceneMove("GameClear");
    }

    private void UISet()
    {
        int t1 = _time / 60;
        int t2 = _time % 60;

        timeText.text = $"Time: {t1:D2}:{t2:D2}";
        scoreText.text = $"Score: {_score:D6}";
    }
}
