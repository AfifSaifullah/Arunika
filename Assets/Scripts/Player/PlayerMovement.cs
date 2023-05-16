using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private LayerMask whatIsGround;

    private Vector2 dashTarget;
    private float dashDistance = 5f;
    private float dashDuration = 0.3f;
    private float dashTimeLeft;
    private float dashCooldown = 1f;
    private float dashCooldownLeft;
    private float dashCounter;
    // private float dashCD;

    private float currentSpeed = 20f;
    private bool facingRight = true;
    [HideInInspector] public bool isRunning = true;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool grounded;

    
    [Range(0, 1)] public float airControl;
    public float runningSpeed;
    public float walkingSpeed;
    public float jumpStrength; 

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
    }

    public void kuranginNyawa()
    {
        if(nyawa < 1f){
            Destroy(gameObject);
        }else{
            nyawa -= .3f;
        }
    }

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

        float deltaVelocity = (currentSpeed * direction - myRigidBody.velocity.x) * (grounded ? 1 : airControl * (direction == 0 ? 0 : 1));
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x + deltaVelocity, myRigidBody.velocity.y);
    }

    // Melompat dengan menambah impuls vektor ke aarah atas
    public void Jump()
    {
        myRigidBody.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
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

    public void groundCheck() {
        grounded = myCollider.IsTouchingLayers(whatIsGround.value);
    }

    public bool isGrounded() {
        return grounded;
    }

    public Vector2 getPos() {
        return myRigidBody.position;
    }

    public Vector2 getVel() {
        return myRigidBody.velocity;
    }
}
