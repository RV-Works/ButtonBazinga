using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.08f;

    [SerializeField] private float fallDelay = 0.1f;
    [SerializeField] private float fallDistance = 10f;
    [SerializeField] private float fallSpeed = 3f;

    private bool isFalling;

    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;

        Vector3 startPos = transform.position;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 offset = Random.insideUnitSphere * shakeMagnitude;
            offset.y = 0f;
            transform.position = startPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;

        yield return new WaitForSeconds(fallDelay);

        Vector3 endPos = startPos + Vector3.down * fallDistance;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fallSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        yield return new WaitForSeconds(5f);
        transform.position = startPos;
    }

}
