using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinGame : MonoBehaviour
{
    public GameObject winImg;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            winImg.SetActive(true);
            Invoke("Replay", 3f);
        }

    }

    public void Replay()
    {
        SceneManager.LoadScene("Ãö¥d 1");
    }
    
}
