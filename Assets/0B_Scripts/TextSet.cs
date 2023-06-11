using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSet : MonoBehaviour
{
    private void Start()
    {
        UIManager.instance.timeText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        UIManager.instance.scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
}
