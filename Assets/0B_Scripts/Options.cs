using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public bool _isOpening = false;

    [SerializeField]
    private GameObject options;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isOpening = false;
            options.SetActive(_isOpening);
        }
    }

    public void Open()
    {
        _isOpening = !_isOpening;

        options.SetActive(_isOpening);
    }
}
