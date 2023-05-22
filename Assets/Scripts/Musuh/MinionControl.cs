using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionControl : Musuh
{
    [SerializeField] private PlayerMovement PlayerObj;
    [SerializeField] private AudioManageGame AudioMan;
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private LayerMask whatIsGround;
    private Vector2 anchorPos;
    private float maxStrayDistance = 10f;
    private float maxStrayTime = 10f;
    [SerializeField] private float strayTimer = 0f;
    private float movementCD = 2.5f;
    [SerializeField] private float flipTimer = 0f;
    
    [SerializeField] private bool attackMode = false;
    private bool jalan = true;
    private bool arahKanan = true;
    private bool serang = false;
    private bool kenaserang = false;
    public float kecepatan = 3f;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anchorPos = myRigidBody.position;
    }

    void Awake()
    {
        nyawa = 50f;
        attackVal = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        float strayDistance = Vector2.Distance(myRigidBody.position, anchorPos);
        float playerDistance = Vector2.Distance(myRigidBody.position, PlayerObj.getPos());
        bool anchorDir = myRigidBody.position.x < anchorPos.x;
        attackMode = (playerDistance <= maxStrayDistance);

        // Cek apakah monster keluar batas areanya
        if(strayDistance >= maxStrayDistance) {
            if(strayTimer > 0)
                strayTimer -= Time.deltaTime;

            if(strayTimer <= 0) {
                myRigidBody.position = anchorPos;
                strayTimer = maxStrayTime;
                attackMode = false;
            }
        }

        
        if(flipTimer > 0)
            flipTimer -= Time.deltaTime;

        if(flipTimer <= 0)
        {
            flipTimer = movementCD;

            if(attackMode)
                arahKanan = (PlayerObj.getPos().x > myRigidBody.position.x);
            else
                arahKanan = !arahKanan;
        }

        if((strayDistance >= maxStrayDistance) && arahKanan != anchorDir)
            arahKanan = anchorDir;

        if(jalan && !kenaserang){
            anim.SetBool("lari", true);
        
            if(arahKanan)
                gerakKanan();
            else
                gerakKiri();
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

    private bool groundCheck()
    {
        return myCollider.IsTouchingLayers(whatIsGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   
        if(collision.gameObject.name == "Player" && !collision.isTrigger) {
            jalan = false;
            serang = true;
        }

        if(collision.gameObject.name == "tembokKanan") {
            arahKanan = false;
        }

        if(collision.gameObject.name == "tembokKiri") {
            arahKanan = true;
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
        PlayerObj.kuranginNyawa(attackVal);
        AudioMan.MinionNyerang();
    }

    public override void Attacked(float damage)
    {
        kenaserang = true;
        jalan = false;
        nyawa -= damage;

        Debug.Log(nyawa);

        if(nyawa <= 0) {
            Destroy(gameObject);
        }

        myRigidBody.AddForce(new Vector2(10f * (arahKanan ? -1 : 1), 5f), ForceMode2D.Impulse);

        AudioMan.MinionKenaSerang();

    }

    public void BangunLagi()
    {
        kenaserang = false;
    }

    public void gerakKanan()
    {
        myRigidBody.velocity = Vector2.right * kecepatan;

        if(transform.localScale.x < 0)
            balikBadan();
    }

    public void gerakKiri()
    {
        myRigidBody.velocity = Vector2.right * -kecepatan;

        if(transform.localScale.x > 0)
            balikBadan();
    }

    void balikBadan()
    {
        Vector3 player = transform.localScale;
        player.x *= -1;
        transform.Translate((player.x > 0) ? (Vector2.right * 2) : (Vector2.left * 2));

        transform.localScale = player;
    }
}
