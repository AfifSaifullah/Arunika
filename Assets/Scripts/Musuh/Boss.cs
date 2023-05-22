using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Musuh
{
    // [SerializeField] private Rigidbody2D myRig;
    // [SerializeField] private Collider2D myCold;
    // [SerializeField] private Collider2D myTrig;
    // [SerializeField] private Transform PlayerP;
    // private bool facingRight;
    // private float health;
    // private float attackDamage;
    // public float speed;
    
    ///

    [SerializeField] private PlayerMovement PlayerObj;
    [SerializeField] private AudioManageGame AudioMan;
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Animator anime;
    private Vector2 anchorPos;
    private float maxStrayDistance = 20f;
    private float maxStrayTime = 10f;
    [SerializeField] private float strayTimer = 0f;
    private float movementCD = 1f;
    [SerializeField] private float flipTimer = 0f;
    
    [SerializeField] private bool attackMode = false;
    private bool jalan = true;
    private bool arahKanan = false;
    private bool serang = false;
    private bool kenaserang = false;
    public float kecepatan = 5f;


    public enum BossAnim {
        Idle,
        Walk,
        NATK1,
        Mati,
        KetarKerit
    }

    public Dictionary<BossAnim, string> animationName = new Dictionary<BossAnim, string>(){
        {BossAnim.Idle, "idle"},
        {BossAnim.Walk, "walk"},
        {BossAnim.NATK1, "serang"},
        {BossAnim.Mati, "dead"},
        {BossAnim.KetarKerit, "ketarketir"}
    };

    BossAnim currentAnimationState;


    // Start is called before the first frame update
    void Start()
    {
        anchorPos = myRigidBody.position;
    }

    void Awake()
    {
        nyawa = 80f;
        attackVal = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        if(nyawa <= 0) return;

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
        else
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
            // anim.SetBool("lari", true);
            changeAnimationState(BossAnim.Walk);
        
            if(arahKanan)
                gerakKanan();
            else
                gerakKiri();
        }
        else if(!serang){
            changeAnimationState(BossAnim.Idle);
        }

        if(serang){
            // anim.SetBool("serang", true);
            // anim.SetBool("lari", false);
            // anim.SetBool("ketarKetir", false);
            // anim.SetBool("diam", false);
            changeAnimationState(BossAnim.NATK1);
        }
        else {
            // anim.SetBool("serang", false);
        }

        if(kenaserang) {
            // anim.SetBool("ketarKetir", true);
            // anim.SetBool("diam", false);
            // anim.SetBool("serang", false);
            // anim.SetBool("lari", false);
        }
        else {
            // anim.SetBool("ketarKetir", false);
        }
    }

    void changeAnimationState(BossAnim newState)
    {
        if(currentAnimationState == newState)
            return;
        
        if(newState == BossAnim.NATK1)
            Debug.Log("serang");
        anime.Play(animationName[newState]);
        currentAnimationState = newState;
    }

    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(1000);
        Debug.Log("Delay!");  
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
            Debug.Log(jalan ? "JALANWOI" : "STOPPPPP");
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            jalan = true;
            serang = false;
            Debug.Log(jalan ? "JALANWOI" : "STOPPPPP");
        }
    }

    public void AttackPlayer()
    {
        PlayerObj.kuranginNyawa(attackVal);
        AudioMan.BossNyerang();
    }

    public override void Attacked(float damage)
    {
        if(nyawa <= 0) return;

        kenaserang = true;
        nyawa -= damage;

        Debug.Log(nyawa);

        if(nyawa <= 0) {
            changeAnimationState(BossAnim.Mati);
        }
        else {
            changeAnimationState(BossAnim.KetarKerit);
        }

        myRigidBody.AddForce(new Vector2(10f * (arahKanan ? -1 : 1), 5f), ForceMode2D.Impulse);

        AudioMan.BossKenaSerang();
    }

    public void killme()
    {
        Destroy(gameObject);
    }

    public void BangunLagi()
    {
        kenaserang = false;

    }

    public void gerakKanan()
    {
        myRigidBody.velocity = Vector2.right * kecepatan;

        if(transform.localScale.x > 0)
            balikBadan();
    }

    public void gerakKiri()
    {
        myRigidBody.velocity = Vector2.right * -kecepatan;

        if(transform.localScale.x < 0)
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
