using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    //public CinemachineBasicMultiChannelPerlin vCamPerlin;

    public CinemachineImpulseSource impulse;

    public float shakeIntensity = 1f;
    public float shakeTime = 0.2f;

    public float timer;
    public float rayTimer;
    public bool hasImpactCalculated = false;
    public float previousImpactVelocity;

    public float currentCameraFOV;
    public float minCameraFOV = 90f;
    public float maxCameraFOV = 150f;
    public float cameraSmoothTime = 0.2f;
    public float cameraVelocityFOV;



    private void Start()
    {
        StopShake();
    }

    private void Update()
    {
        //if (timer > 0)
        //{
        //    timer -= Time.deltaTime;

        //    if (timer <= 0)
        //    {
        //        StopShake();
        //    }
        //}
    }


    public void ShakeCamera(float intensity, float playerMagnitude)
    {
        //vCamPerlin = playerVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //vCamPerlin.m_AmplitudeGain = shakeIntensity;

        //timer = shakeTime;

        impulse.GenerateImpulse(intensity * playerMagnitude);
    }

    public void StopShake()
    {
        //vCamPerlin = playerVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //vCamPerlin.m_AmplitudeGain = 0f;
        //timer = 0f;
    }

}
