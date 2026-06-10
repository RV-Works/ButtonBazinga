using Unity.VisualScripting;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private GameObject grapplePoint;
    private GameObject grappleString;
    [SerializeField] private int Range;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Range))
        {
            if (hit.collider.CompareTag("GrapplePoint"))
            {
                grapplePoint = hit.collider.gameObject;
                grappleString = new GameObject("GrappleString");
                LineRenderer lineRenderer = grappleString.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grapplePoint.transform.position);
            }
        }
        
    }
    
}
