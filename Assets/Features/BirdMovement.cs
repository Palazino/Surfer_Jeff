using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float destroyX = -15f;
    [SerializeField] private AudioClip BirdFly;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(BirdFly);
    }

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
