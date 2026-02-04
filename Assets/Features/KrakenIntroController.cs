using UnityEngine;
using System.Collections;

public class KrakenIntroController : MonoBehaviour
{
    [SerializeField] private float introDelay = 10f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float phase1Scale = 0.3f;
    [SerializeField] private float phase2Scale = 0.6f;
    [SerializeField] private float phase3Scale = 1f;
    [SerializeField] private KrakenController krakenController;
    [SerializeField] private AudioClip introClip;

    [SerializeField] private float phase1Volume = 0.3f;
    [SerializeField] private float phase2Volume = 0.6f;
    [SerializeField] private float phase3Volume = 1f;

    private AudioSource audioSource;


    private Vector3 finalPosition;
    private Vector3 finalScale;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        finalPosition = transform.position;
        finalScale = transform.localScale;

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Attendre avant apparition
        yield return new WaitForSeconds(introDelay);

        // Phase 1 : petit et loin
        transform.localScale = finalScale * phase1Scale;
        transform.position = new Vector3(-12f, finalPosition.y, finalPosition.z);
        PlayIntroSound(phase1Volume);


        // Traverse gauche → droite
        while (transform.position.x < 12f)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Phase 2 : plus grand
        transform.localScale = finalScale * phase2Scale;
        transform.position = new Vector3(15f, finalPosition.y, finalPosition.z);
        PlayIntroSound(phase2Volume);


        // Traverse droite → gauche
        while (transform.position.x > -15f)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Phase 3 : presque taille finale
        transform.localScale = finalScale * phase3Scale;
        transform.position = new Vector3(-15f, finalPosition.y, finalPosition.z);
        PlayIntroSound(phase3Volume);


        // Traverse gauche → droite
        while (transform.position.x < finalPosition.x)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            yield return null;
        }
        // Position finale exacte
        transform.position = finalPosition;
        transform.localScale = finalScale;

        // Activation des attaques
        krakenController.enabled = true;

    }
    void PlayIntroSound(float volume)
    {
        if (introClip == null) return;

        audioSource.volume = volume;
        audioSource.PlayOneShot(introClip);
    }

}
