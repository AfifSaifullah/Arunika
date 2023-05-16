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
    public bool deteksi = false;
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

        // if(serang && deteksi){
        //     anim.SetBool("serang", true);
        // }else{
        //     anim.SetBool("serang", false);
        //     anim.SetBool("diam", false);
        //     anim.SetBool("serang", false);
        //     jalan = true;
        // }

        // if(!deteksi){
        //     anim.SetBool("diam", false);
        //     anim.SetBool("serang", false);
        //     jalan = true;
        // }
        // deteksi = false;

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
            //jalan = false;
            serang = true;
            anim.SetBool("serang", true);
        }else{
            print(collision.gameObject.name);
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
