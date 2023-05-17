using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private Collider2D AttackTrigger;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask enemyLayer;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();
    const float groundedRadius = 0.1f;

    private Vector2 dashTarget;
    private float dashDistance = 5f;
    private float dashDuration = 0.3f;
    private float dashTimeLeft;
    private float dashCooldown = 1f;
    private float dashCooldownLeft;
    private float dashCounter;
    private float attackWaitTime = 0f;
    [HideInInspector] public int attackNo = 0;

    private float currentSpeed = 20f;
    private bool facingRight = true;
    [HideInInspector] public bool isRunning = true;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool grounded;
    
    [Range(0, 1)] public float airControl;
    public float runningSpeed;
    public float walkingSpeed;
    public float jumpStrength; 
    public float attackDamage;
    public float nyawa = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        groundCheck();

        if(dashCooldownLeft > 0)
            dashCooldownLeft -= Time.deltaTime;

        if(attackWaitTime > 0) {
            attackWaitTime -= Time.deltaTime;
        }
        else {
            isAttacking = false;
            attackNo = 0;
        }
    }

    // ==========================================================
    // Combat Mechanics
    // ==========================================================

    public void kuranginNyawa(float damage)
    {
        nyawa -= damage;

        if(nyawa <= 0)
            Destroy(gameObject);
    }

    public void startAttack()
    {
        if(isAttacking)
            attackNo = (attackNo + 1) % 2;

        attackWaitTime = 1f;
        isAttacking = true;
        isDashing = false;
    }

    public void dealDamage()
    {
        foreach(Collider2D enemyCollider in enemiesInRange)
        {
            Musuh enemy = enemyCollider.GetComponent<Musuh>();
            enemy.Attacked(attackDamage);
            Debug.Log("Enemy Hit");
        }
    }

    // ==========================================================
    // Locomotion Mechanics
    // ==========================================================

    // Mengubah state jalan/lari
    public void changeRunningState()
    {
        isRunning = !isRunning;
        currentSpeed = (isRunning ? runningSpeed : walkingSpeed);
    }

    // Memulai gerakan dash
    public void startDash(float direction)
    {
        if(dashCooldownLeft > 0 && dashCounter == 2) return;
        
        if(dashCounter == 2) dashCounter = 0;

        dashTarget = myRigidBody.position + new Vector2(dashDistance * (facingRight ? 1 : -1), 0);
        dashTimeLeft = dashDuration;
        dashCooldownLeft = dashCooldown;
        dashCounter++;
        isDashing = true;
        isAttacking = false;
    }

    // Bergerak secara horizontal
    public void MoveHorizontally(float direction)
    {
        if(!facingRight && direction > 0)
            Flip();
        if(facingRight && direction < 0)
            Flip();

        if(isDashing)
        {
            myRigidBody.velocity = new Vector2((dashTarget.x - myRigidBody.position.x)*10, 0);
            dashTimeLeft -= Time.deltaTime;
            isDashing = (dashTimeLeft > 0);
            return;
        }

        if(direction != 0)
            isAttacking = false;

        float deltaVelocity = (currentSpeed * direction - myRigidBody.velocity.x) * (grounded ? 1 : airControl * (direction == 0 ? 0 : 1));
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x + deltaVelocity, myRigidBody.velocity.y);
    }

    // Melompat dengan menambah impuls vektor ke aarah atas
    public void Jump()
    {
        myRigidBody.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        isAttacking = false;
        isDashing = false;
    }

    // Membalikan arah objek dan sprite pemain menghadap
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    // ==========================================================
    // Other Mechanics
    // ==========================================================

    // Mengencek apakah objek pemain menyentuh tanah
    public void groundCheck() {
        // grounded = myCollider.IsTouchingLayers(whatIsGround.value);
        
        Vector2 circleCenter = transform.position;
        circleCenter.y -= 2f;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCenter, groundedRadius, whatIsGround);
    
        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject == gameObject)
                continue;
            
            grounded = true;
            return;
        }

        grounded = false;
    }

    // Mengembalikan koordinat 2d posisi tengah objek pemain
    public Vector2 getPos() {
        return myRigidBody.position;
    }

    // Mengembalikan vektor 2d kecepatan objek pemain
    public Vector2 getVel() {
        return myRigidBody.velocity;
    }

    // Dijalankan ketika sebuah objek memasuki boundary trigger collider pemain
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Masuk");
        if(!other.isTrigger && ((enemyLayer.value & (1 << other.gameObject.layer)) != 0))
        {
            Debug.Log("Musuh Masuk");
            enemiesInRange.Add(other);
        }
    }

    // Dijalankan ketika sebuah objek keluar dari boundary trigger collider pemain
    void OnTriggerExit2D(Collider2D other)
    {
        if(enemiesInRange.Contains(other))
            enemiesInRange.Remove(other);
    }
}
