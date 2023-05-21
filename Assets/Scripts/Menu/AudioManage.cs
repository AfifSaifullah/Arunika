using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public AudioSource musik;

    // Start is called before the first frame update
    void Start()
    {
        musik.volume = PlayerPrefs.GetFloat("volume");
        musik.Play();
    }

    // Update is called once per frame
    void Update()
    {
        musik.volume = PlayerPrefs.GetFloat("volume");
    }
}
