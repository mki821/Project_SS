using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject shotgunBulletPrefab;
    [SerializeField]
    private GameObject gunEffect;
    [SerializeField]
    private float rebound = 4;
    [SerializeField]
    private int gunType = 1;
    [SerializeField]
    private InventoryUI _inventoryUI;

    private int _ammoAR;
    private int _maxAmmoAR;
    private int _ammoShotgun;
    private int _maxAmmoShotgun;
    private int _magazineAR;
    private int _magazineShotgun;
    private bool _reloading = false;
    private Vector2 _mousePos;
    private Vector2 _v2;

    private Transform _bulletSpawnPosition;
    private TextMeshProUGUI _ammoText;
    private TextMeshProUGUI _magazineText;
    private TextMeshProUGUI _gunTypeText;

    public int Ammo
    {
        get
        {
            switch (GunType)
            {
                case 1:
                    return _ammoAR;
                case 2:
                    return _ammoShotgun;
            }

            return 0;
        }
        set
        {
            switch (GunType)
            {
                case 1:
                    _ammoAR = Mathf.Max(0, value);
                    break;
                case 2:
                    _ammoShotgun = Mathf.Max(0, value);
                    break;
            } 
        }
    }
    public int MaxAmmo
    {
        get
        {
            switch (GunType)
            {
                case 1:
                    return _maxAmmoAR;
                case 2:
                    return _maxAmmoShotgun;
            }

            return 0;
        }
    }
    public int Magazine
    {
        get
        {
            switch (GunType)
            {
                case 1:
                    return _magazineAR;
                case 2:
                    return _magazineShotgun;
            }

            return 0;
        }
        set
        {
            switch (GunType)
            {
                case 1:
                    _magazineAR = Mathf.Max(0, value);
                    break;
                case 2:
                    _magazineShotgun = Mathf.Max(0, value);
                    break;
            }
        }
    }
    private int GunType { get => gunType; set => gunType = Mathf.Clamp(value, 1, 2); }

    private void Awake()
    {
        _bulletSpawnPosition = transform.Find("Square");
        _ammoText = GameObject.Find("Canvas/GunStatus/AmmoText").GetComponent<TextMeshProUGUI>();
        _magazineText = GameObject.Find("Canvas/GunStatus/MagazineText").GetComponent<TextMeshProUGUI>();
        _gunTypeText = GameObject.Find("Canvas/GunStatus/GunTypeText").GetComponent<TextMeshProUGUI>();

        _ammoAR = 30;
        _maxAmmoAR = 30;
        _ammoShotgun = 6;
        _maxAmmoShotgun = 6;
        _magazineAR = 5;
        _magazineShotgun = 5;
}

    private void Start()
    {
        StartCoroutine(Shoot());
        StartCoroutine(Reload());
    }

    private void Update()
    {
        MouseMove();
        MousePositionSet();
        GunChange();
    }

    private void GunChange()
    {
        if (!_reloading)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0)
            {
                GunType += 1;
                TextSet();
            }
            else if (scroll < 0)
            {
                GunType -= 1;
                TextSet();
            }

            switch (GunType)
            {
                case 1:
                    _gunTypeText.text = "AR";
                    break;
                case 2:
                    _gunTypeText.text = "Shotgun";
                    break;
            }
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetMouseButton(0) && !_reloading && !_inventoryUI.activeInventory);

            if (Ammo < 1 || Input.GetMouseButtonDown(1) && !_reloading)
            {
                StartCoroutine(Reloading());
                continue;
            }
            else
            {
                yield return new WaitForSeconds(AttackType(GunType));
            }

            yield return null;
        }
    }

    private float AttackType(int attackType)
    {
        float ret = 0;
        Ammo--;

        switch (attackType)
        {
            case 1:
                CameraManager.instance.CameraShake(rebound, 1);
                Instantiate(bulletPrefab, _bulletSpawnPosition.position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg)));
                Instantiate(gunEffect, transform.Find("Square").position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) - 90), transform);
                TextSet();
                ret = 0.1f;
                break;
            case 2:
                CameraManager.instance.CameraShake(rebound * 2, 2);
                int angle = -10;
                for(int i = 0; i < 5; i++)
                {
                    //Instantiate(shotgunBulletPrefab, _bulletSpawnPosition.position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg + angle)));
                    PoolManager.instance.Pop("Shotgun", _bulletSpawnPosition.position , - (Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg + angle));
                    angle += 5;
                }
                Instantiate(gunEffect, transform.Find("Square").position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) - 90), transform);
                TextSet();
                ret = 1;
                break;
        }


        return ret;
    }

    public void TextSet()
    {
        if (!_reloading)
        {
            _ammoText.text = "Ammo " + Ammo + " / " + MaxAmmo;
            _magazineText.text = "Magazine: " + Magazine;

            if (Ammo == 0)
            {
                StartCoroutine(Reloading());
                return;
            }
        }
        _magazineText.text = "Magazine: " + Magazine;
    }

    private IEnumerator Reload()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetMouseButton(1) && !_reloading);
            yield return new WaitForSeconds(Time.deltaTime);

            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        if (Magazine > 0)
        {
            _reloading = true;
            Magazine--;
            _magazineText.text = "Magazine: " + Magazine;
            _ammoText.text = "Ammo Reloading...";

            yield return new WaitForSeconds(2f);

            _reloading = false;
            Ammo = MaxAmmo;
            TextSet();
        }
    }

    private void MouseMove()
    {
        if(Time.timeScale != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -(Mathf.Atan2(_v2.x, _v2.y) * Mathf.Rad2Deg) + 90);

            if (transform.parent.position.x < _mousePos.x)
            {
                transform.parent.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.parent.position.x > _mousePos.x)
            {
                transform.parent.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void MousePositionSet()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _v2 = _mousePos - (Vector2)transform.position;
    }

    public void MagazinePlus(int magazineType)
    {
        switch (magazineType)
        {
            case 1:
                _magazineAR++;
                break;
            case 2:
                _magazineShotgun++;
                break;
        }
        TextSet();
    }
}
