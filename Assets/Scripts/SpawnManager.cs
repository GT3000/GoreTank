using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected Wave[] waves;
    protected int currentWaveNumber;
    [SerializeField] protected float firstEnemyWaveDelay;
    [SerializeField] protected float delayBetweenEnemySpawns;
    [SerializeField] protected float delayBetweenGibbletSpawns;
    [SerializeField] protected float timeBetweenWaves;
    [SerializeField] protected GameObject spawnFX;
    [SerializeField] protected int spawnAreaX;
    [SerializeField] protected int spawnAreaY;
    
    protected GameObject gibbletHolder;
    protected GameObject enemyHolder;
    protected bool gameStarted;
    protected bool waveEnded;


    private void OnEnable()
    {
        GameEvents.EnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.EnemyKilled -= EnemyKilled;
    }

    private void Start()
    {
        StartCoroutine(FirstWave());
        
        gibbletHolder = new GameObject("Gibblets");
        enemyHolder = new GameObject("Enemies");
    }

    private void Update()
    {
        if (waves[currentWaveNumber].enemiesLeft <= 0 && gameStarted && !waveEnded)
        {
            waveEnded = true;
            currentWaveNumber++;
            StartCoroutine(StartSpawning());
        }
    }

    private IEnumerator FirstWave()
    {
        yield return new WaitForSeconds(3.0f);
        
        waves[currentWaveNumber].enemiesLeft = waves[currentWaveNumber].numberOfEnemiesInWave;

        gameStarted = true;
        
        StartCoroutine(SpawnGibblets());

        yield return new WaitForSeconds(firstEnemyWaveDelay);
        
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        
        waves[currentWaveNumber].enemiesLeft = waves[currentWaveNumber].numberOfEnemiesInWave;
        waveEnded = false;
        
        StartCoroutine(SpawnGibblets());
        
        yield return new WaitForSeconds(timeBetweenWaves);
        
        StartCoroutine(SpawnEnemies());
    }
    
    

    private IEnumerator SpawnGibblets()
    {
        if (waves[currentWaveNumber].currentGibblets < waves[currentWaveNumber].numberOfGibbletsInWave)
        {
            int randomIndex = Random.Range(0, waves[currentWaveNumber].gibbletsToSpawn.Count);
            Vector3 randomSpot = GetRandomSpot();

            GameObject tempFX = Instantiate(this.spawnFX, randomSpot, Quaternion.identity);

            yield return new WaitForSeconds(tempFX.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.25f);
            
            Destroy(tempFX);
                
            GameObject tempGibblet = Instantiate(waves[currentWaveNumber].gibbletsToSpawn[randomIndex], randomSpot, Quaternion.identity);
            tempGibblet.transform.parent = gibbletHolder.transform;

            waves[currentWaveNumber].currentGibblets++;

            yield return new WaitForSeconds(delayBetweenGibbletSpawns);

            StartCoroutine(SpawnGibblets());
        }

        yield return null;
    }
    
    private IEnumerator SpawnEnemies()
    {
        if (waves[currentWaveNumber].currentEnemies < waves[currentWaveNumber].numberOfEnemiesInWave)
        {
            int randomIndex = Random.Range(0, waves[currentWaveNumber].enemiesToSpawn.Count);
            Vector3 randomSpot = GetRandomSpot();

            GameObject tempFX = Instantiate(this.spawnFX, randomSpot, Quaternion.identity);

            yield return new WaitForSeconds(tempFX.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.25f);
            
            Destroy(tempFX);
                
            GameObject tempEnemy = Instantiate(waves[currentWaveNumber].enemiesToSpawn[randomIndex], randomSpot, Quaternion.identity);
            tempEnemy.transform.parent = enemyHolder.transform;

            waves[currentWaveNumber].currentEnemies++;

            yield return new WaitForSeconds(delayBetweenEnemySpawns);

            StartCoroutine(SpawnEnemies());
        }

        yield return null;
    }

    private void EnemyKilled()
    {
        waves[currentWaveNumber].enemiesLeft--;
    }

    private Vector2 GetRandomSpot()
    {
        Vector2 randomSpot;
        
        int randomX = Random.Range(-spawnAreaX, spawnAreaX);
        int randomY = Random.Range(-spawnAreaY, spawnAreaY);

        randomSpot = new Vector2(randomX, randomY);

        return randomSpot;
    }
}

[System.Serializable]
public struct Wave
{
    [SerializeField] public int numberOfEnemiesInWave;
    [SerializeField] public int numberOfGibbletsInWave;
    [SerializeField] public int currentEnemies;
    [SerializeField] public int currentGibblets;
    [SerializeField] public int enemiesLeft;
    [SerializeField] public List<GameObject> gibbletsToSpawn;
    [SerializeField] public List<GameObject> enemiesToSpawn;
}
