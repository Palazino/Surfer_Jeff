using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float minSpawnInterval = 3f;
    [SerializeField] private float maxSpawnInterval = 6f;

    private float timer;
    private float currentSpawnInterval;

    void Start()
    {
        SetNextInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnBird();
            timer = 0f;
            SetNextInterval();
        }
    }

    void SpawnBird()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(birdPrefab, spawnPoints[index].position, Quaternion.identity);
    }

    void SetNextInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
