using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 0.5f;
    [SerializeField] private float maxRiseSpeed = 2f;
    [SerializeField] private float acceleration = 0.05f;

    private float currentSpeed;
    [SerializeField] private bool isRising;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = riseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRising == true) { 
            if (currentSpeed < maxRiseSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed > maxRiseSpeed)
                {
                    currentSpeed = maxRiseSpeed;
                }
            }

            transform.Translate(Vector3.up * (currentSpeed * Time.deltaTime), Space.World);
        }
    }

    public void StartRain() 
    {
        Debug.Log("OHNOTHERAIN!!");
        isRising = true;
    }
}
