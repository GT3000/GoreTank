using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] protected AudioClip mainMenuBGM;
    [SerializeField] protected List<AudioClip> gameBGM;
    [SerializeField] protected float currentMusicVolume;
    [SerializeField] protected float defaultMusicVolume;
    [SerializeField] protected AudioSource audioSource;
    
    [Header("SFX")]
    [SerializeField] protected float currentSfxVolume;
    [SerializeField] protected float defaultSfxVolume;
    [SerializeField] protected AudioSource sfxAudioSource;
    
    private void OnEnable()
    {
        GameEvents.PlaySfx += PlaySfx;
        GameEvents.UpdateMusicVolume += ChangeMusicVolume;
        GameEvents.UpdateSfxVolume += ChangeSfxVolume;
    }

    private void OnDisable()
    {
        GameEvents.PlaySfx -= PlaySfx;
        GameEvents.UpdateMusicVolume -= ChangeMusicVolume;
        GameEvents.UpdateSfxVolume -= ChangeSfxVolume;
    }

    void Awake()
    {
        PickMusic();

        audioSource.Play();
    }

    private void Start()
    {
        SetVolumes();
    }

    private void SetVolumes()
    {
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVol", defaultMusicVolume);
        audioSource.volume = currentMusicVolume;
        
        currentSfxVolume = PlayerPrefs.GetFloat("SfxVol", defaultSfxVolume);
        sfxAudioSource.volume = currentSfxVolume;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameEvents.SetOptionsSliders(currentMusicVolume, currentSfxVolume);
        }
    }

    private void PickMusic()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.clip = mainMenuBGM;
        }
        else
        {
            int randomIndex = Random.Range(0, gameBGM.Count);

            audioSource.clip = gameBGM[randomIndex];
        }
    }

    private void PlaySfx(AudioClip sfx)
    {
        if (sfxAudioSource != null)
        {
            float defaultPitch = sfxAudioSource.pitch;
            float randomPitch = Random.Range(0.95f, 1.05f);

            sfxAudioSource.pitch = randomPitch;
            sfxAudioSource.PlayOneShot(sfx);
            sfxAudioSource.pitch = defaultPitch;
        }
    }

    private void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVol", value);
        audioSource.volume = PlayerPrefs.GetFloat("MusicVol");
        GameEvents.SavePlayerPrefs();
    }
    
    private void ChangeSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SfxVol", value);
        sfxAudioSource.volume = PlayerPrefs.GetFloat("SfxVol");

        GameEvents.SavePlayerPrefs();
    }
}
