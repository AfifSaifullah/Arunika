using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class MenuAwal : MonoBehaviour
{
    GameObject panPeng;
    public Slider volumeSlider;

    void Start()
    {
        panPeng = GameObject.Find("Panel-Pengaturan");
        panPeng.SetActive(false);

        if(PlayerPrefs.GetInt("diubah") == 0){
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
    }

    public void bukaPengaturan(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        panPeng.SetActive(true);
    }

    public void tutupPengaturan(){
        panPeng.SetActive(false);
    }

    public void ubahVolume(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetInt("diubah", 1);
    }

    public void SceneMulai() {  
        SceneManager.LoadScene("SampleScene");  
    } 
    public void exitgame() {  
        Application.Quit();  
    } 
}
