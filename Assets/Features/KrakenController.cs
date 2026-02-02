using UnityEngine;
using System.Collections;

public class KrakenController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tentacleLeft;
    [SerializeField] private SpriteRenderer tentacleRight;
    [SerializeField] private Sprite leftHigh;
    [SerializeField] private Sprite leftDown;
    [SerializeField] private Sprite rightHigh;
    [SerializeField] private Sprite rightDown;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float strikeDuration = 0.5f;
    [SerializeField] private GameObject hitboxLeft;
    [SerializeField] private GameObject hitboxRight;


    private bool attackLeftNext = true;

    void Start()
    {
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);

            if (attackLeftNext)
                yield return StartCoroutine(StrikeLeft());
            else
                yield return StartCoroutine(StrikeRight());

            attackLeftNext = !attackLeftNext;
        }
    }

    IEnumerator StrikeLeft()
    {
        tentacleLeft.sortingOrder = 1;
        tentacleLeft.sprite = leftDown;

        hitboxLeft.SetActive(true);

        yield return new WaitForSeconds(strikeDuration);

        hitboxLeft.SetActive(false);

        tentacleLeft.sprite = leftHigh;
        tentacleLeft.sortingOrder = -1;
    }



    IEnumerator StrikeRight()
    {
        tentacleRight.sortingOrder = 1;
        tentacleRight.sprite = rightDown;

        hitboxRight.SetActive(true);

        yield return new WaitForSeconds(strikeDuration);

        hitboxRight.SetActive(false);

        tentacleRight.sprite = rightHigh;
        tentacleRight.sortingOrder = -1;
    }


}
