using System.Runtime.CompilerServices;
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
    [SerializeField] private int coinCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        GetComponent<Rain>();
        
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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
        SceneManager.LoadScene("Thnx for demp");
        }

    }
}