using UnityEngine;
using System.Collections;

public class GroundWaveEffect : MonoBehaviour
{
    [SerializeField] private float waveHeight = 0.5f;
    [SerializeField] private float waveDuration = 0.4f;

    private Vector3 startPosition;
    private bool isWaving = false;

    void Start()
    {
        startPosition = transform.position;
    }

    public void TriggerWave()
    {
        if (!isWaving)
            StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        isWaving = true;

        float timer = 0f;

        while (timer < waveDuration)
        {
            timer += Time.deltaTime;

            float t = timer / waveDuration;

            float height = Mathf.Sin(t * Mathf.PI) * waveHeight;

            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + height,
                startPosition.z
            );

            yield return null;
        }

        transform.position = startPosition;
        isWaving = false;
    }
}
