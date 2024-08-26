using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
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


    public void ShakeCamera(float intensity, float playerMagnitude)
    {
        impulse.GenerateImpulse(intensity * playerMagnitude);
    }

}
