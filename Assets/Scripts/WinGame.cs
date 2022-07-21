using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    [SerializeField] GameObject winImg;


    void Update()
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
