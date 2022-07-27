using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinGame : MonoBehaviour
{
    public GameObject winImg;
    // public GameObject soundObject;
    private AudioSource audioSource;


    public void Start()
    {
        // Find找階層面板的物件
        GameObject soundObject = GameObject.Find("Player");
        audioSource = soundObject.GetComponent<AudioSource>();
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            winImg.SetActive(true);
            audioSource.Pause();
            Invoke("Replay", 2f);
        }

    }

    public void Replay()
    {
        SceneManager.LoadScene("關卡 1");
    }
    
}
