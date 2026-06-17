using UnityEngine;
using UnityEngine.SceneManagement;

public class Startgame : MonoBehaviour
{
    [SerializeField] public string sceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartGame();
        }
    }
}