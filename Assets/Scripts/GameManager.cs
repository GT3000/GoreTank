using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected int currentScore;
    [SerializeField] protected int bestScore;

    private void OnEnable()
    {
        GameEvents.ChangeScore += ChangeScore;
        GameEvents.Restart += RestartLevel;
        GameEvents.Quit += Quit;
        GameEvents.SavePlayerPrefs += SaveValues;
        GameEvents.ClearScores += ClearHighScore;
    }
    
    private void OnDisable()
    {
        GameEvents.ChangeScore -= ChangeScore;
        GameEvents.Restart += RestartLevel;
        GameEvents.Quit -= Quit;
        GameEvents.SavePlayerPrefs -= SaveValues;
        GameEvents.ClearScores -= ClearHighScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEvents.PauseMenu();
        }
    }

    private void ChangeScore(int score)
    {
        currentScore += score;
        GameEvents.ScoreUpdate(currentScore);
    }
    
    private void SaveScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            SaveValues();
        }
    }

    private void ClearHighScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        SaveValues();
    }

    private void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SaveScore(currentScore);
        //TODO scene transition
        SceneManager.LoadScene(1);
    }

    private void Quit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    private void SaveValues()
    {
        PlayerPrefs.Save();
    }
}
