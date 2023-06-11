using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour
{
    public static SceneM instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SceneMove(string sceneName)
    {
        if(sceneName == "SampleScene")
            GameManager.instance.gameEnd = false;

        SceneManager.LoadScene(sceneName);
    }
}
