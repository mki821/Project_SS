using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        SceneManager.sceneLoaded += LoadedSceneEvent;
    }

    private void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            vCam = FindObjectOfType<CinemachineVirtualCamera>();
            cameraPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _gun = GameObject.Find("Player/PlayerSprite/Gun").GetComponent<Gun>();
        }
    }

    public void CameraShake(float amplitude, int type)
    {
        StartCoroutine(ShakeCam(amplitude, type));
    }

    IEnumerator ShakeCam(float amplitude, int type)
    {
        if (cameraPerlin.m_AmplitudeGain <= 0 || cameraPerlin.m_FrequencyGain <= 0)
        {
            cameraPerlin.m_AmplitudeGain = amplitude;

            switch (type)
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
