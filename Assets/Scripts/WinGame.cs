using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinGame : MonoBehaviour
{
    public GameObject winImg;
    // public GameObject soundObject;
    // private AudioSource audioSource;


    public void Start()
    {
        // GameObject soundObject = GameObject.Find("BackgroundSoundObjectName");
        // AudioSource audioSource = soundObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            winImg.SetActive(true);
            // audioSource.Pause();
            
            Invoke("Replay", 3f);
        }

    }

    public void Replay()
    {
        SceneManager.LoadScene("Ãö¥d 1");
    }
    
}
