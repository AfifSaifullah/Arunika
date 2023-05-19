using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{

    GameObject panPeng;
    GameObject panPause;
    GameObject btnPause;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        panPeng = GameObject.Find("Panel-Pengaturan");
        panPeng.SetActive(false);
        panPause = GameObject.Find("Panel-Pause");
        panPause.SetActive(false);
        btnPause = GameObject.Find("btn-pause");
        btnPause.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void kePause(){
        btnPause.SetActive(false);
        panPause.SetActive(true);    
        Time.timeScale = 0;    
    }

    public void kePengaturan(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        panPause.SetActive(false);
        panPeng.SetActive(true);
    }

    public void kePauseLagi(){
        panPause.SetActive(true);
        panPeng.SetActive(false);
    }

    public void lanjut(){
        btnPause.SetActive(true);
        panPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void ubahVolume(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetInt("diubah", 1);
    }
}
