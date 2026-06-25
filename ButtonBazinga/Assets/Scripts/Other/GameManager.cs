using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject StartGame;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private Image coinsImage;
    private Vector3 startcoinsImagePos;
    [SerializeField] private float sizeIncrease;

    [SerializeField] private int coinCounter;

    public static GameManager instance;

    private GameObject water;

    [Header("PlayerStats")]

    private GameObject Player;

    private float startPlayerHeight;

    private float currentPlayerHeight;

    [SerializeField] private float endPlayerHeight;

    private float currentPlayerHeightPercentage;

    [SerializeField] private Image PlayerUI;

    private float startPlayerUIHeight;

    private float currentPlayerUIHeight;

    [SerializeField] private float endPlayerUIHeight;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(instance);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPlayerUIHeight = -endPlayerUIHeight;
        startcoinsImagePos = coinsImage.GetComponent<RectTransform>().transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            currentPlayerHeight = Player.transform.position.y;
        }

        currentPlayerHeightPercentage = Mathf.InverseLerp(startPlayerHeight, endPlayerHeight, currentPlayerHeight);

        currentPlayerUIHeight = Mathf.Lerp(startPlayerUIHeight, endPlayerUIHeight, currentPlayerHeightPercentage);
        
        PlayerUiIncrease();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<Rain>().StartRain();
        }
    }

    public void CoinCounter() 
    {
        Debug.Log("Wow, you got a coin. GREAT JOB.");
        coinCounter++;
        coinText.text = coinCounter.ToString();
        StartCoroutine(GetCoinAnimation());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
        SceneManager.LoadScene("Thnx for demp");
        }
    }

    public void getWaterObject(GameObject gameObject)
    {
        water = gameObject;
        startPlayerHeight = water.transform.position.y;
    }

    public void getPlayerObject(GameObject gameObject)
    {
        Player = gameObject;
    }

    private void PlayerUiIncrease()
    {
        PlayerUI.GetComponent<RectTransform>().localPosition = new Vector3(PlayerUI.GetComponent<RectTransform>().localPosition.x, currentPlayerUIHeight, PlayerUI.GetComponent<RectTransform>().localPosition.z);
    }

    private IEnumerator GetCoinAnimation()
    {
        coinsImage.GetComponent<RectTransform>().transform.DOScale(new Vector3(startcoinsImagePos.x + sizeIncrease, startcoinsImagePos.y + sizeIncrease, startcoinsImagePos.z + sizeIncrease), 0.25f).OnComplete(() =>
        {
            coinsImage.GetComponent<RectTransform>().transform.DOScale(new Vector3(startcoinsImagePos.x, startcoinsImagePos.y, startcoinsImagePos.z), 0.25f);
        });
        yield return null;
    }
}