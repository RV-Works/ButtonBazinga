using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Rain : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 0.5f;
    [SerializeField] private float maxRiseSpeed = 2f;
    [SerializeField] private float acceleration = 0.05f;

    private float currentSpeed;
    [SerializeField] private bool isRising;

    [Header("WaterStats")]

    private float startWaterHeight;

    private float currentWaterHeight;

    private float currentWaterHeightPercentage;

    [SerializeField] private float endWaterHeight;

    [SerializeField] private Image waterUI; 

    private float waterUiStartHeight;

    private float waterUiCurrentHeight;

    [SerializeField] private float waterUiEndHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.getWaterObject(gameObject);

        currentSpeed = riseSpeed;

        startWaterHeight = transform.position.y;

        waterUiStartHeight = waterUI.GetComponent<RectTransform>().localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (isRising == true && currentWaterHeight <= endWaterHeight) { 
            if (currentSpeed < maxRiseSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed > maxRiseSpeed)
                {
                    currentSpeed = maxRiseSpeed;
                }
            }

            transform.Translate(Vector3.up * (currentSpeed * Time.deltaTime), Space.World);

            currentWaterHeight = transform.position.y;

            currentWaterHeightPercentage = Mathf.InverseLerp(startWaterHeight, endWaterHeight, currentWaterHeight);

            waterUiCurrentHeight = Mathf.Lerp(waterUiStartHeight, waterUiEndHeight, currentWaterHeightPercentage);

            WaterUiIncrease();
        }
    }

    public void StartRain() 
    {
        Debug.Log("OHNOTHERAIN!!");
        isRising = true;
    }

    private void WaterUiIncrease()
    {
        waterUI.GetComponent<RectTransform>().localScale = new Vector3(waterUI.GetComponent<RectTransform>().localScale.x, waterUiCurrentHeight, waterUI.GetComponent<RectTransform>().localScale.z);
    }

}
