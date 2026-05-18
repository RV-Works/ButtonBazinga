using System.Collections;
using UnityEngine;

public class SquishyPlatform : MonoBehaviour
{
    [SerializeField] private float Bounced = 0.5f;
    [SerializeField] private float resetDelay = 0.05f;

    private Vector3 startPosition;
    private bool isPressed;
    private bool playerOnPlatform;
    private Coroutine resetCoroutine;

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
        transform.position = startPosition + new Vector3(0f, -Bounced, 0f);
    }

    private IEnumerator ResetWhenPlayerLeaves()
    {
        yield return new WaitForSeconds(resetDelay);

        if (playerOnPlatform)
        {
            yield break;
        }

        isPressed = false;
        transform.position = startPosition;
        resetCoroutine = null;
    }
}