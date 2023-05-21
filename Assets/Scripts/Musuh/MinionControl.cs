using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionControl : Musuh
{
    [SerializeField] private Rigidbody2D myRigidBody;
    public float kecepatan = .1f;
    public int arah = 1;
    public bool jalan = false;
    public bool arahKanan = true;
    public bool serang = false;
    public bool kenaserang = false;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        nyawa = 50f;
        attackVal = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if(jalan){
            anim.SetBool("lari", true);
        
            if(arahKanan) {
                gerakKanan();
            }
            else {
                gerakKiri();
            }
        }
        else {
            anim.SetBool("lari", false);
            anim.SetBool("diam", true);
        }

        if(serang){
            anim.SetBool("serang", true);
            anim.SetBool("lari", false);
            anim.SetBool("ketarKetir", false);
            anim.SetBool("diam", false);
        }
        else {
            anim.SetBool("serang", false);
        }

        if(kenaserang) {
            anim.SetBool("ketarKetir", true);
            anim.SetBool("diam", false);
            anim.SetBool("serang", false);
            anim.SetBool("lari", false);
        }
        else {
            anim.SetBool("ketarKetir", false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   

        if(collision.gameObject.name == "Tilemap") {
            jalan = true;
        }
        else if(collision.gameObject.name == "TembokKanan") {
            arahKanan = false;
        }
        else if(collision.gameObject.name == "TembokKiri") {
            arahKanan = true;
        }
        else if(collision.gameObject.name == "Player" && !collision.isTrigger) {
            GameObject go = GameObject.Find("Player");
            PlayerMovement sc = (PlayerMovement) go.GetComponent(typeof(PlayerMovement));

            jalan = false;
            serang = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            jalan = true;
            serang = false;
        }
    }

    public void AttackPlayer()
    {
        GameObject go = GameObject.Find("Player");
        PlayerMovement sc = (PlayerMovement) go.GetComponent(typeof(PlayerMovement));
        sc.kuranginNyawa(attackVal);
        
        GameObject aud = GameObject.Find("AudioManager");
        AudioManageGame sc_aud = (AudioManageGame) aud.GetComponent(typeof(AudioManageGame));
        sc_aud.MinionNyerang();
    }

    public override void Attacked(float damage)
    {
        kenaserang = true;
        nyawa -= damage;

        Debug.Log(nyawa);

        if(nyawa <= 0) {
            Destroy(gameObject);
        }

        // myRigidBody.AddForce(new Vector2(5f * -arah, 5f), ForceMode2D.Impulse);

        GameObject go = GameObject.Find("AudioManager");
        AudioManageGame sc = (AudioManageGame) go.GetComponent(typeof(AudioManageGame));
        sc.MinionKenaSerang();

    }

    public void BangunLagi()
    {
        kenaserang = false;
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
