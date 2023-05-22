using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{

    GameObject panPeng;
    GameObject panPause;
    GameObject btnPause;
    GameObject btnKanan;
    GameObject btnKiri;
    GameObject btnLompat;
    GameObject btnDash;
    GameObject btnSerang;
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
        btnLompat = GameObject.Find("btn-lompat");
        btnKanan = GameObject.Find("btn-kanan");
        btnKiri = GameObject.Find("btn-kiri");
        btnDash = GameObject.Find("btn-dash");
        btnSerang = GameObject.Find("btn-serang");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void kePause(){
        btnPause.SetActive(false);
        panPause.SetActive(true);   

        btnLompat.SetActive(false);
        btnSerang.SetActive(false);
        btnDash.SetActive(false);
        btnKanan.SetActive(false);
        btnKiri.SetActive(false);

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

        btnLompat.SetActive(true);
        btnSerang.SetActive(true);
        btnDash.SetActive(true);
        btnKanan.SetActive(true);
        btnKiri.SetActive(true);

        Time.timeScale = 1;
    }

    public void ubahVolume(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetInt("diubah", 1);
    }
}
