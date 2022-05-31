using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected Slider fuelGauge;
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected TextMeshProUGUI deadScoreText;
    [SerializeField] protected TextMeshProUGUI highScoreText;
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] protected GameObject tutorialText;
    protected bool playerDied;

    private void OnEnable()
    {
        GameEvents.AddFuel += AddFuel;
        GameEvents.SubtractFuel += SubtractFuel;
        GameEvents.SetFuel += SetFuel;
        GameEvents.ScoreUpdate += ChangeScore;
        GameEvents.PauseMenu += PauseMenu;
        GameEvents.PlayerKilled += ShowFinalScore;
    }

    private void OnDisable()
    {
        GameEvents.AddFuel -= AddFuel;
        GameEvents.SubtractFuel -= SubtractFuel;
        GameEvents.SetFuel += SetFuel;
        GameEvents.ScoreUpdate -= ChangeScore;
        GameEvents.PauseMenu -= PauseMenu;
        GameEvents.PlayerKilled -= ShowFinalScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        deadScoreText.gameObject.SetActive(false);
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (fuelGauge.value <= 0)
        {
            fuelGauge.fillRect.gameObject.SetActive(false);
        }
        else
        {
            fuelGauge.fillRect.gameObject.SetActive(true);
        }
    }

    private void SetFuel(int fuel)
    {
        fuelGauge.maxValue = fuel;
    }

    private void AddFuel(int fuel)
    {
        fuelGauge.value += fuel;
    }

    private void SubtractFuel(int fuel)
    {
        fuelGauge.value -= fuel;
    }

    private void ChangeScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
        deadScoreText.text = currentScore.ToString();

        scoreText.gameObject.GetComponent<Animator>().SetTrigger("scored");
    }

    private void ShowFinalScore()
    {
        if (!playerDied)
        {
            playerDied = true;
            StartCoroutine(DeadScore());
        }
        
    }

    private IEnumerator DeadScore()
    {
        yield return new WaitForSeconds(3.0f);
        
        scoreText.gameObject.SetActive(false);
        fuelGauge.gameObject.SetActive(false);
        deadScoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        
        deadScoreText.gameObject.SetActive(false);
        
        PauseMenu();
    }

    private void PauseMenu()
    {
        highScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        
        if (pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    private IEnumerator Tutorial()
    {
        if (PlayerPrefs.GetInt("Tutorial Text", 0) == 0)
        {
            PlayerPrefs.SetInt("Tutorial Text", 1);
            GameEvents.SavePlayerPrefs();
            
            tutorialText.SetActive(true);

            yield return new WaitForSeconds(5.0f);
            
            tutorialText.SetActive(false);
        }
        else
        {
            tutorialText.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        GameEvents.Restart();
    }

    public void Quit()
    {
        GameEvents.Quit();
    }
}
