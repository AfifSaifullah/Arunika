using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuAwal : MonoBehaviour
{
    public void SceneMulai() {  
        SceneManager.LoadScene("SampleScene");  
    } 
    public void exitgame() {  
        Application.Quit();  
    } 
}
