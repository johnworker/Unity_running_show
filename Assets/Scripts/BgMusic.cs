using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    public AudioSource closeBGMusic;
    void Start()
    {

    }

    private void CloseBGMusic()
    {
        closeBGMusic = GameObject.Find("BackgroundSound").GetComponent<AudioSource>();
        closeBGMusic.Stop();

    }
}
