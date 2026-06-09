using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 1f;
    private bool isRising;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRising == true) { 
        transform.Translate(Vector3.up * (riseSpeed * Time.deltaTime), Space.World);
    }
}

    public void StartRain() 
    {
        isRising = true;
    }
}
