using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin cameraPerlin;
    private Gun _gun;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        vCam = FindObjectOfType<CinemachineVirtualCamera>();
        cameraPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _gun = GameObject.Find("Player/PlayerSprite/Gun").GetComponent<Gun>();
    }

    public void CameraShake(float amplitude, int attackType)
    {
        StartCoroutine(ShakeCam(amplitude, attackType));
    }

    IEnumerator ShakeCam(float amplitude, int attackType)
    {
        if (cameraPerlin.m_AmplitudeGain <= 0 || cameraPerlin.m_FrequencyGain <= 0)
        {
            cameraPerlin.m_AmplitudeGain = amplitude;

            switch (attackType)
            {
                case 1:
                    yield return new WaitUntil(() => Input.GetMouseButtonUp(0) || _gun.Ammo < 1);
                    break;
                case 2:
                    yield return new WaitForSeconds(0.3f);//
                    break;
            }

            while (true)
            {
                if (cameraPerlin.m_AmplitudeGain <= 0)
                {
                    cameraPerlin.m_AmplitudeGain = 0;

                    break;
                }

                cameraPerlin.m_AmplitudeGain -= 2.6f * Time.deltaTime * amplitude;

                yield return null;
            }
        }
    }
}
