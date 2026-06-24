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

    [Header("PlayerStats")]

    [SerializeField] private GameObject Player;

    private float startPlayerHeight;

    private float currentPlayerHeight;

    [SerializeField] private float endPlayerHeight;

    private float currentPlayerHeightPercentage;

    [SerializeField] private Image PlayerUI;

    private float startPlayerUIHeight;

    private float currentPlayerUIHeight;

    [SerializeField] private float endPlayerUIHeight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = riseSpeed;

        startWaterHeight = transform.position.y;

        waterUiStartHeight = waterUI.GetComponent<RectTransform>().localScale.y;

        startPlayerHeight = transform.position.y;

        startPlayerUIHeight = -endPlayerUIHeight;

        Debug.Log(startPlayerUIHeight);
        Debug.Log(startPlayerHeight);
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

            currentPlayerHeight = Player.transform.position.y;

            //currentHeightPercentage = currentHeight / endHeight * 1;
            //target = currentHeightPercentage * ((startHeight * endHeight) - startHeight);
            //Debug.Log(currentHeightPercentage);
            //target = target + startHeight;


            currentPlayerHeightPercentage = Mathf.InverseLerp(startPlayerHeight, endPlayerHeight, currentPlayerHeight);

            currentPlayerUIHeight = Mathf.Lerp(startPlayerUIHeight, endPlayerUIHeight, currentPlayerHeightPercentage);


            Debug.Log(currentPlayerHeightPercentage);

            currentWaterHeightPercentage = Mathf.InverseLerp(startWaterHeight, endWaterHeight, currentWaterHeight);

            waterUiCurrentHeight = Mathf.Lerp(waterUiStartHeight, waterUiEndHeight, currentWaterHeightPercentage);



            PlayerUiIncrease();
            WaterUiIncrease();
            //Debug.Log(target);
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

    private void PlayerUiIncrease()
    {
        PlayerUI.GetComponent<RectTransform>().localPosition = new Vector3(PlayerUI.GetComponent<RectTransform>().localPosition.x, currentPlayerUIHeight, PlayerUI.GetComponent<RectTransform>().localPosition.z);
    }
}
