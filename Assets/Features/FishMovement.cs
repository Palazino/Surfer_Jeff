using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minJumpHeight = 2f;
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float minJumpFrequency = 1.5f;
    [SerializeField] private float maxJumpFrequency = 3f;
    [SerializeField] private float jumpDuration = 1f;

    private float jumpTimer;
    private float jumpHeight;
    private float jumpFrequency;

    private float startY;

    void Start()
    {
        jumpHeight = Random.Range(minJumpHeight, maxJumpHeight);
        jumpFrequency = Random.Range(minJumpFrequency, maxJumpFrequency);

        startY = transform.position.y; // ← IMPORTANT
    }


    void Update()
    {
        // Déplacement horizontal
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Avancement du timer
        jumpTimer += Time.deltaTime;

        // Normalisation du temps (0 → 1)
        float t = jumpTimer / jumpDuration;

        // Parabole (départ sol → sommet → retour sol)
        float height = 4f * jumpHeight * t * (1 - t);

        float newY = startY + height;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );

        // Si le saut est terminé → détruire
        if (t >= 1f)
        {
            Destroy(gameObject);
        }

    }
}
