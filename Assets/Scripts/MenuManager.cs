using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] protected Animator titlePulseAnimator;
    [SerializeField] protected Animator topTextAnimator;
    [SerializeField] protected Animator bottomText;
    [SerializeField] protected Animator sceneTransition;
    [SerializeField] protected GameObject startButton;
    [SerializeField] protected GameObject optionsButton;

    [SerializeField] protected Slider musicVolume;
    [SerializeField] protected Slider sfxVolume;

    private void OnEnable()
    {
        GameEvents.SetOptionsSliders += SetSliders;
    }
    
    private void OnDisable()
    {
        GameEvents.SetOptionsSliders -= SetSliders;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextFinished());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TextFinished()
    {
        sceneTransition.gameObject.SetActive(false);
        startButton.SetActive(false);
        optionsButton.SetActive(false);
        
        yield return new WaitForSeconds(bottomText.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.01f);
        
        StartPulse();
    }

    private void StartPulse()
    {
        titlePulseAnimator.GetComponent<Animator>().SetBool("pulse", true);

        startButton.SetActive(true);
        optionsButton.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        sceneTransition.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(sceneTransition.GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.01f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SetSliders(float music, float sfx)
    {
        musicVolume.value = music;
        sfxVolume.value = sfx;
    }

    public void ChangeMusic()
    {
        GameEvents.UpdateMusicVolume(musicVolume.value);
    }
    
    public void ChangeSfx()
    {
        GameEvents.UpdateSfxVolume(sfxVolume.value);
    }
    
    public void ClearScore()
    {
        GameEvents.ClearScores();
    }
}
