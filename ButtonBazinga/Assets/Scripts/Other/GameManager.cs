using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject StartGame;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private int coinCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartGame.IsDestroyed();
        }
    }

    private void CoinCounter() 
    {
        coinCounter++;
        coinText.text = coinCounter.ToString();
    }

}
