using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject enemyPrefab;
    private float spawnRange = 2f;
    public int waveNumber = 1;
    public GameObject powerupPrefab;
    public GameObject[] spawnLocations;
    private int enemiesToSpawn;
    private int enemiesAlive;
    private int maxEnemiesAtOnce = 20;
    [SerializeField] private AudioSource newRoundSound;
    
    void Start()
    {
        gameManager = GameObject.Find("/--Managers--/Game Manager").GetComponent<GameManager>();
        enemiesToSpawn = waveNumber*3;
        StartCoroutine("SpawnWave");
    }
 
    void Update()
    {
        
        // int remaining = enemiesAlive + enemiesToSpawn;
        // Debug.Log("Remaining: "+remaining);
        if(isEndOfRound() && !gameManager.gameOver)
        {
            waveNumber++;
            enemiesToSpawn = waveNumber*3;
            // Debug.Log("Wave: "+waveNumber+"\nAlive(Expected zero): "+enemiesAlive+"\nToSpawn: "+enemiesToSpawn);

            StartCoroutine("SpawnWave");
        }
    }

    Vector3 GenerateRandomSpawnLocation()
    {
        float spawnOffsetX = Random.Range(-spawnRange, spawnRange);
        float spawnOffsetZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position;
        return spawnLocation + new Vector3(spawnOffsetX, 0, spawnOffsetZ);
    }

    private IEnumerator SpawnWave()
    {
        newRoundSound.Play();
        yield return new WaitForSeconds(5);
        while(enemiesToSpawn > 0)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.25f));
            enemiesAlive = FindObjectsOfType<Enemy>().Length;
            if(enemiesAlive < maxEnemiesAtOnce)
            {
                SpawnEnemy();
                enemiesToSpawn -= 1;
            }
        }
    }

    private bool isEndOfRound()
    {
        enemiesAlive = FindObjectsOfType<Enemy>().Length;
        return enemiesAlive == 0 && enemiesToSpawn == 0;
    }

    void SpawnPowerup()
    {
        Instantiate(powerupPrefab, GenerateRandomSpawnLocation(), powerupPrefab.transform.rotation);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, GenerateRandomSpawnLocation(), enemyPrefab.transform.rotation);
    }
}
