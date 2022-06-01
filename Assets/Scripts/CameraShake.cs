using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    protected float currentAmp;
    protected float currentduration;
    protected float currentFrequency;
    protected float elapsedTime;
    [SerializeField] protected CinemachineVirtualCamera virtualCam;
    protected CinemachineBasicMultiChannelPerlin virualCamNoise;

    private void OnEnable()
    {
        GameEvents.CameraShake += SetShake;
    }

    private void OnDisable()
    {
        GameEvents.CameraShake -= SetShake;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (virtualCam != null)
        {
            virualCamNoise = virtualCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shake();
    }

    private void SetShake(float duration, float shakeAmp, float shakeFreq)
    {
        elapsedTime = duration;
        currentAmp = shakeAmp;
        currentFrequency = shakeFreq;

        if (virtualCam != null && virualCamNoise != null)
        {
            if (elapsedTime > 0)
            {
                virualCamNoise.m_AmplitudeGain = currentAmp;
                virualCamNoise.m_FrequencyGain = currentFrequency;

                elapsedTime -= Time.deltaTime;
            }
            else
            {
                currentFrequency = 0f;
                currentAmp = 0f;
                
                virualCamNoise.m_AmplitudeGain = 0f;
                elapsedTime = 0f;
            }
        }
    }

    private void Shake()
    {
        SetShake(elapsedTime, currentAmp , currentFrequency);
    }
}
