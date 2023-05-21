using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManageGame : MonoBehaviour
{
    public AudioSource suaraMinionKenaSerang;
    public AudioSource suaraMinionNyerang;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        suaraMinionKenaSerang.volume = PlayerPrefs.GetFloat("volume");
        suaraMinionNyerang.volume = PlayerPrefs.GetFloat("volume");

        if(Input.GetKeyDown(KeyCode.Q)){
            if(Time.timeScale == 0){
                Time.timeScale = 1;
            }else{
                Time.timeScale = 0;
            }
        }
    }

    public void MinionKenaSerang(){
        if(!suaraMinionKenaSerang.isPlaying){
            suaraMinionKenaSerang.Play();
        }
    }

    public void MinionNyerang(){
        if(!suaraMinionNyerang.isPlaying){
            suaraMinionNyerang.Play();
        }
    }
}
