using UnityEngine;

public class GroundWaveEffect : MonoBehaviour
{
    [SerializeField] private float waveAmplitude = 0.3f;
    [SerializeField] private float waveFrequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + offsetY,
            startPosition.z
        );
    }
}
