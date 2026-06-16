using UnityEngine;

public class Zipline : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool invertMapRotation = false;
    private bool isRiding = false;
    private PlayerMovement player;
    private Rigidbody playerRb;
    private float progress;
    private float travelTime;
    private float startY;
    private float endY;
    private float totalRotationDiff;
    private Vector3 initialPlayerFixedPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!isRiding && other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                playerRb = other.GetComponent<Rigidbody>();
                initialPlayerFixedPos = playerRb.position;

                player.onJumpEvent += OnPlayerJumped;

                // Determine direction based on proximity
                bool goingToEnd = true;
                if (Vector3.Distance(playerRb.position, endPoint.position) < Vector3.Distance(playerRb.position, startPoint.position))
                {
                    goingToEnd = false;
                }

                Transform fromP = goingToEnd ? startPoint : endPoint;
                Transform toP = goingToEnd ? endPoint : startPoint;

                Vector2 fromDir = new Vector2(fromP.position.x, fromP.position.z).normalized;
                Vector2 toDir = new Vector2(toP.position.x, toP.position.z).normalized;

                float angleFrom = Mathf.Atan2(fromDir.y, fromDir.x) * Mathf.Rad2Deg;
                float angleTo = Mathf.Atan2(toDir.y, toDir.x) * Mathf.Rad2Deg;

                totalRotationDiff = Mathf.DeltaAngle(angleFrom, angleTo);

                startY = fromP.position.y;
                endY = toP.position.y;

                float dist = Vector3.Distance(fromP.position, toP.position);
                travelTime = dist / speed;
                if (travelTime <= 0.01f) travelTime = 0.01f;

                isRiding = true;
                progress = 0f;

                Vector3 v = playerRb.linearVelocity;
                v.y = 0f;
                playerRb.linearVelocity = v;
                playerRb.useGravity = false;

                player.isGrappling = true; 
            }
        }
    }

    private void OnPlayerJumped()
    {
        if (isRiding)
        {
            EndRide();
        }
    }

    private void EndRide()
    {
        isRiding = false;
        playerRb.useGravity = true;
        player.isGrappling = false;
        if (player != null)
        {
            player.onJumpEvent -= OnPlayerJumped;
        }
    }

    private void Update()
    {
        if (isRiding && player != null)
        {
            float lastP = progress;
            progress += Time.deltaTime / travelTime;

            bool finished = false;
            if (progress >= 1f)
            {
                progress = 1f;
                finished = true;
            }

            float deltaProgress = progress - lastP;

            // Map and player thingy so it doesnt go whacky ;-;"
            float angleStep = totalRotationDiff * deltaProgress;
            if (invertMapRotation) angleStep = -angleStep;
            player.RotateMap(-angleStep);
            float currentY = Mathf.Lerp(startY, endY, progress);
            Vector3 pos = playerRb.position;
            pos.x = initialPlayerFixedPos.x;
            pos.z = initialPlayerFixedPos.z;
            pos.y = currentY;
            playerRb.MovePosition(pos);

            if (finished)
            {

                isRiding = false;
                playerRb.useGravity = true;
                player.isGrappling = false;
            }
        }
    }
}
