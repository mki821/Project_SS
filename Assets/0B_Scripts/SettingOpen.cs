using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingOpen : MonoBehaviour
{
    public bool _isOpening = false;

    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private Options options;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InventoryUI.instance.activeInventory && !options._isOpening)
        {
            settings.SetActive(false);
            Open();
        }
    }

    public void Open()
    {
        _isOpening = !_isOpening;

        ui.SetActive(_isOpening);

        if (_isOpening)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
