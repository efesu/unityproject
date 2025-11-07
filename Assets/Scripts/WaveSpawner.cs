using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 15f;
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 3;
    public float spawnDelay = 0.5f;

    private int currentWave = 0;
    private bool waveInProgress = false;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(2f); // oyun başında küçük bir bekleme

        while (true)
        {
            currentWave++;
            Debug.Log($"Wave {currentWave} başladı!");

            waveInProgress = true;

            int enemiesToSpawn = enemiesPerWave + (currentWave - 1) * 2; // her wave'de +2 düşman
            yield return StartCoroutine(SpawnEnemies(enemiesToSpawn));

            waveInProgress = false;
            Debug.Log($"Wave {currentWave} bitti. Yeni wave {timeBetweenWaves} saniye sonra başlıyor.");

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
{
    if (player == null || enemyPrefab == null) return;

    Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
    Vector3 spawnPos = new Vector3(player.position.x + randomCircle.x, player.position.y, player.position.z + randomCircle.y);

    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

    float difficultyMultiplier = 1f + (currentWave - 1) * 0.2f;
    enemy.GetComponent<Enemy>()?.SetDifficulty(difficultyMultiplier);
}

}
