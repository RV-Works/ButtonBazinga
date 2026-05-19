using System.Collections;
using UnityEngine;

public class SquishyPlatform : MonoBehaviour
{
    [SerializeField] private float Bounced = 0.5f;
    [SerializeField] private float pressDuration = 0.15f;
    [SerializeField] private float resetDelay = 0.05f;
    [SerializeField] private float resetDuration = 0.15f;

    private Vector3 startPosition;
    private bool isPressed;
    private bool playerOnPlatform;
    private Coroutine resetCoroutine;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        playerOnPlatform = true;

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        if (isPressed)
        {
            return;
        }

        Press();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        playerOnPlatform = false;

        resetCoroutine = StartCoroutine(ResetWhenPlayerLeaves());
    }

    private void Press()
    {
        isPressed = true;
        StartMove(startPosition + new Vector3(0f, -Bounced, 0f), pressDuration);
    }

    private IEnumerator ResetWhenPlayerLeaves()
    {
        yield return new WaitForSeconds(resetDelay);

        if (playerOnPlatform)
        {
            yield break;
        }

        isPressed = false;
        StartMove(startPosition, resetDuration);
        resetCoroutine = null;
    }

    private void StartMove(Vector3 targetPosition, float duration)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        moveCoroutine = StartCoroutine(MoveTo(targetPosition, duration));
    }

    private IEnumerator MoveTo(Vector3 targetPosition, float duration)
    {
        if (duration <= 0f)
        {
            transform.position = targetPosition;
            moveCoroutine = null;
            yield break;
        }

        Vector3 from = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(from, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        moveCoroutine = null;
    }
}