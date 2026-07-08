using Unity.VisualScripting;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private float Range = 15f;
    [SerializeField] private Transform grappleGunTip;
    [SerializeField] private string grappleTag = "GrapplePoint";
    [SerializeField] private float pullSpeed = 10f;
    [SerializeField] private float autoRotateMapSpeed = 30f;

    private GameObject ropeVisual;

    private SpringJoint joint;
    private PlayerMovement playerMovement;
    private Transform grapplePointRef;
    private Vector3 localHitPoint;
    private bool stopAutoRotate = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (joint != null && grapplePointRef != null)
        {
            Vector3 currentGlobalPoint = grapplePointRef.TransformPoint(localHitPoint);
            joint.connectedAnchor = currentGlobalPoint;

            // player gets put under that thingy
            joint.maxDistance = Mathf.MoveTowards(joint.maxDistance, 0f, pullSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentGlobalPoint) > Range)
            {
                StopGrapple();
            }

            // map turns
            if (!stopAutoRotate)
            {
                float xDiff = currentGlobalPoint.x - transform.position.x;
                if (Mathf.Abs(xDiff) > 0.1f)
                {
                    float rotAmount = Mathf.Sign(xDiff) * autoRotateMapSpeed * Time.deltaTime;
                    if (playerMovement != null)
                    {
                        playerMovement.RotateMap(rotAmount);
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartGrapple()
    {
        stopAutoRotate = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Range))
        {
            if (hit.collider.CompareTag(grappleTag) || hit.collider.name.Contains("Grapple"))
            {
                grapplePointRef = hit.transform;
                localHitPoint = grapplePointRef.InverseTransformPoint(hit.point);

                joint = gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = hit.point;

                float distanceFromPoint = Vector3.Distance(transform.position, hit.point);
                
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                if (ropeVisual == null)
                {
                    ropeVisual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    Destroy(ropeVisual.GetComponent<Collider>()); 
                }
                ropeVisual.SetActive(true);

                if (playerMovement != null) 
                    playerMovement.isGrappling = true;
            }
        }
    }

    private void StopGrapple()
    {
        if (ropeVisual != null) 
            ropeVisual.SetActive(false);

        if (joint != null) 
            Destroy(joint);

        grapplePointRef = null;

        if (playerMovement != null) 
            playerMovement.isGrappling = false;
    }

    private void DrawRope()
    {
        if (!joint || ropeVisual == null) return;

        Vector3 startPos = grappleGunTip != null ? grappleGunTip.position : transform.position;
        Vector3 endPos = joint.connectedAnchor;

        Vector3 dir = endPos - startPos;
        float distance = dir.magnitude;

        ropeVisual.transform.position = startPos + dir / 2f;
        ropeVisual.transform.up = dir;
        ropeVisual.transform.localScale = new Vector3(0.05f, distance / 2f, 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            stopAutoRotate = true;
        }
    }
}
