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
        tentacleLeft.sprite = leftDown;

        yield return new WaitForSeconds(strikeDuration);

        tentacleLeft.sprite = leftHigh;
    }

    IEnumerator StrikeRight()
    {
        tentacleRight.sprite = rightDown;

        yield return new WaitForSeconds(strikeDuration);

        tentacleRight.sprite = rightHigh;
    }
}
