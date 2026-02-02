using UnityEngine;

public class WaveScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private Transform otherWave;
    [SerializeField] private float waveAmplitude = 0.5f;
    [SerializeField] private float waveFrequency = 1f;

    private float startY;


    public float spriteWidth;
    private Camera cam;

    void Start()
    {
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x - 1;
        cam = Camera.main;
        startY = transform.position.y;

    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );

        // Déplacement vers la gauche
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // Bord gauche de la caméra
        float leftBound = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

        // Bord droit de la vague
        float waveRightEdge = transform.position.x + (spriteWidth * 0.5f);

        // Si la vague sort complètement de l'écran à gauche
        if (waveRightEdge < leftBound)
        {
            transform.position = new Vector3(
                otherWave.position.x + spriteWidth,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
