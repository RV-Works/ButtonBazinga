using System;
using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] private Flash flashImage = null;
    [SerializeField] private GameObject RainDrops;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            FindAnyObjectByType<Rain>().StartRain();
            RainDrops.SetActive(true);
            flashImage.startFlash(0.5f, 1f, Color.red);
        }
        
    }
}
