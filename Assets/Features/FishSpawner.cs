using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float difficultyRampSpeed = 0.05f;

    private float difficultyTimer;
    private float currentSpawnInterval;
    private float timer;
    private bool isStopped = false;


    void Start()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (isStopped) return;
        difficultyTimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnFish();
            timer = 0f;
            float difficultyFactor = 1f - (difficultyTimer * difficultyRampSpeed);

            difficultyFactor = Mathf.Clamp(difficultyFactor, 0.3f, 1f);

            float min = minSpawnInterval * difficultyFactor;
            float max = maxSpawnInterval * difficultyFactor;

            currentSpawnInterval = Random.Range(min, max);

        }

    }
    void SpawnFish()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawn = spawnPoints[randomIndex];

        Instantiate(fishPrefab, chosenSpawn.position, Quaternion.identity);
    }
    public void StopSpawning()
    {
        isStopped = true;
    }

}
