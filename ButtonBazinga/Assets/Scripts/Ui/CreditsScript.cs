using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    [SerializeField] private float scrollspeed = 40f;

    private RectTransform rectTransform;
    

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(ReturnToMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollspeed * Time.deltaTime);
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(29f);
        SceneManager.LoadScene("StartingScreen");
    }
}
