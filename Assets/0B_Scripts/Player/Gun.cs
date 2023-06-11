using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    public bool isAttack = false;

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
    private int _levelAR = 1;
    private int _levelShotgun = 1;
    private int _levelAxe = 1;
    private bool _reloading = false;

    private Transform _bulletSpawnPosition;
    private TextMeshProUGUI _ammoText;
    private TextMeshProUGUI _magazineText;
    private TextMeshProUGUI _gunTypeText;
    private GunMove _gunMove;

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
                case 3:
                    return 1;
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
                case 3:
                    return 1;
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
                case 3:
                    return 1;
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
    private int GunType { get => gunType; set => gunType = Mathf.Clamp(value, 1, 3); }
    public int LevelAR
    {
        get => _levelAR;
        set
        {
            _levelAR = Mathf.Clamp(value, 1, 5);
            _maxAmmoAR = 30 + (LevelAR * 15);
            TextSet();
        }
    }
    public int LevelShotgun { get => _levelShotgun; set => _levelShotgun = Mathf.Clamp(value, 1, 5); }
    public int LevelAxe { get => _levelAxe; set => _levelAxe = Mathf.Clamp(value, 1, 5); }

    private void Awake()
    {
        _bulletSpawnPosition = transform.Find("Square");
        _ammoText = GameObject.Find("Canvas/GunStatus/AmmoText").GetComponent<TextMeshProUGUI>();
        _magazineText = GameObject.Find("Canvas/GunStatus/MagazineText").GetComponent<TextMeshProUGUI>();
        _gunTypeText = GameObject.Find("Canvas/GunStatus/GunTypeText").GetComponent<TextMeshProUGUI>();
        _gunMove = GetComponent<GunMove>();

        _ammoAR = 30;
        _maxAmmoAR = 30;
        _ammoShotgun = 6;
        _maxAmmoShotgun = 6;
        _magazineAR = 10;
        _magazineShotgun = 5;
}

    private void Start()
    {
        StartCoroutine(Shoot());
        StartCoroutine(Reload());
    }

    private void Update()
    {
        GunChange();
    }

    private void GunChange()
    {
        if (!_reloading && !InventoryUI.instance.activeInventory)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0)
            {
                GunType -= 1;
                TextSet();
            }
            else if (scroll < 0)
            {
                GunType += 1;
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
                case 3:
                    _gunTypeText.text = "Axe";
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
                isAttack = false;
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
                PoolManager.instance.Pop("ARBullet", _bulletSpawnPosition.position, -(Mathf.Atan2(_gunMove._v2.x, _gunMove._v2.y) * Mathf.Rad2Deg));
                Instantiate(gunEffect, transform.Find("Square").position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_gunMove._v2.x, _gunMove._v2.y) * Mathf.Rad2Deg) - 90), transform);
                TextSet();
                ret = 0.12f - (LevelAR * 0.02f);
                break;
            case 2:
                CameraManager.instance.CameraShake(rebound * 2, 2);
                ShotgunLevel();
                AudioManager.instance.PlaySound("ShotgunShot");
                Instantiate(gunEffect, transform.Find("Square").position, Quaternion.Euler(0, 0, -(Mathf.Atan2(_gunMove._v2.x, _gunMove._v2.y) * Mathf.Rad2Deg) - 90), transform);
                TextSet();
                ret = 0.7f;
                break;
            case 3:
                isAttack = true;
                _gunMove.Swing();
                AudioManager.instance.PlaySound("Swing");
                ret = 0.8f;
                break;
        }

        return ret;
    }

    private void ShotgunLevel()
    {
        int a = 72 * LevelShotgun;
        Debug.Log(a);

        int angle = -a / 2;

        for (int i = 0; i < LevelShotgun * 7; i++)
        {
            PoolManager.instance.Pop("ShotgunBullet", _bulletSpawnPosition.position, -(Mathf.Atan2(_gunMove._v2.x, _gunMove._v2.y) * Mathf.Rad2Deg + angle));
            angle += 12;
        }
    }

    public void TextSet()
    {
        if (gunType != 3)
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
        else
        {
            _ammoText.text = "None";
            _magazineText.text = "None";
        }
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
            AudioManager.instance.PlaySound("Reload");

            yield return new WaitForSeconds(2f);

            _reloading = false;
            Ammo = MaxAmmo;
            TextSet();
        }
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
