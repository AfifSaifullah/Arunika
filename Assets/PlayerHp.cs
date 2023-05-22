using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public Image playerHpBar;

    public Image playerHpBar2;

    public float  playerHp = 100f;

    public float hp2 = 100f;


    public void Update(){
        PlayerBar2(playerHp);
    }
    public void PlayerBar(float hp)
    {
        playerHpBar.fillAmount = hp/100;
        playerHp = hp;
    }
    public void PlayerBar2(float hp)
    {
        if (hp<hp2){
            hp2 -= 5 * Time.deltaTime;
            playerHpBar2.fillAmount = hp2/100;
        }
    }
}
