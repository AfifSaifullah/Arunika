using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerakMusuh : MonoBehaviour
{
    public float kecepatan = .1f;
    public bool balik = false;
    public int arah = 1;
    public bool jalan = false;
    public bool arahKanan = true;
    public bool serang = false;
    public bool kenaserang = false;
    public bool deteksi = false;
    public float nyawa = 50;
    public AudioSource suaraKenaSerang;
    public AudioSource suaraNyerang;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(jalan){
            anim.SetBool("lari", true);
        
            if(arahKanan){
                gerakKanan();
            }else{
                gerakKiri();
            }
        }else{
            anim.SetBool("lari", false);
            anim.SetBool("diam", true);
        }

        if(serang){
            anim.SetBool("serang", true);
            anim.SetBool("lari", false);
            anim.SetBool("ketarKetir", false);
            anim.SetBool("diam", false);
            GameObject go = GameObject.Find("Player");
            PlayerMovement sc = (PlayerMovement) go.GetComponent(typeof(PlayerMovement));
            sc.kuranginNyawa();
            if(!suaraNyerang.isPlaying){
                suaraNyerang.Play();
            }
        }else{
            anim.SetBool("serang", false);
        }

        if(kenaserang){
            anim.SetBool("ketarKetir", true);
            anim.SetBool("diam", false);
            anim.SetBool("serang", false);
            anim.SetBool("lari", false);
            if(nyawa < 1){
                Destroy(gameObject);
            }else{
                nyawa -= .5f;
            }

            if(!suaraKenaSerang.isPlaying){
                suaraKenaSerang.Play();
            }
        }else{
            anim.SetBool("ketarKetir", false);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {   

        if(collision.gameObject.name == "Tilemap"){
            jalan = true;
        }else if(collision.gameObject.name == "TembokKanan"){
            arahKanan = false;
        }else if(collision.gameObject.name == "TembokKiri"){
            arahKanan = true;
        }else if(collision.gameObject.name == "Player"){

            GameObject go = GameObject.Find("Player");
            PlayerMovement sc = (PlayerMovement) go.GetComponent(typeof(PlayerMovement));

            if (sc.isDashing){
                jalan = false;
                kenaserang = true;
            }else{
                jalan = false;
                serang = true;
            }

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player"){
            jalan = true;
            serang = false;
            kenaserang = false;
        }
    }

    public void gerakKanan()
    {
        arah = 1;
        transform.Translate(Vector2.right * kecepatan * Time.deltaTime);

        balikBadan();
    }

    public void gerakKiri()
    {
        arah = -1;
        transform.Translate(Vector2.right * -kecepatan * Time.deltaTime);

        balikBadan();
    }

    void balikBadan()
    {
        Vector3 player = transform.localScale;
        
        if(arah > 0 && player.x < 0){
            player.x *= -1;
        }else if(arah < 0 && player.x > 0){
            player.x *= -1;
        }
        
        transform.localScale = player;
    }
}
