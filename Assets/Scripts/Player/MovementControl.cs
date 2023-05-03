using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    [SerializeField] private float walkingSpeed;  // Kecepatan gerak horizontal pemain berjalan
    [SerializeField] private float runningSpeed;  // Kecepatan gerak horizontal pemain berlari
    [SerializeField] private float dashDistance;  // Jarak lompatan posisi ketika melakukan dash
    [SerializeField] private LayerMask whatIsGround;    // Layer yang akan dianggap sebagai lantai/ground
    [SerializeField] private Rigidbody2D myRigidBody;   
    [SerializeField] private Collider2D playerCollider;
    private float currentSpeed = 20f;               // Kecepatan gerakam horizotal pemain
    private int jumpCounter = 0;                    // Jumlah aksi jump yang sudah dilakukan (digunakan untuk mengatur double jump)
    private bool facingRight = true;                // Arah objek pemain menghadap (1 -> kanan, 0 -> kiri)
    private bool grounded = true;                   // Apakah objek pemain menyentuh tanah
    private Vector2 dashTarget;                     // Target posisi ketika melakukan dash
    private float dashTime;                         // Waktu yang berjalan setelah memulai dash
    private int dashCounter = 0;                    // Counter untuk membatasi aksi dash di udara
    public bool dash = false;                      // Apakah dash sedangn dilakukan

    [Range(0, 1)]public float airControl;   // Pengaruh kontrol gerakan horizontal ketika di udara
    public float jumpForce;                 // Besar gaya melompat
    public bool isRunning = true;           // Apakah pemain berlari/jalan (1 -> lari, 0 -> jalan)

    // Mengubah state gerakan horizontal biasa (antara lari atau jalan)
    public void changeRunningState()
    {
        isRunning = !isRunning;
        currentSpeed = (isRunning ? runningSpeed : walkingSpeed);
    }

    // Memulai aksi gerakan dash
    public void startDash()
    {   
        if(dashCounter == 1)
            return;

        dashTarget = myRigidBody.position + new Vector2(dashDistance * (facingRight ? 1f : -1f), 0);
        dashCounter++;
        dashTime = 0;
        dash = true;
    }

    // menjalankan gerakan horizontal berdasarkan arah yang dimasukkan
    public void HMove(float direction)
    {
        if(direction > 0 && !facingRight)
            Flip();
        if(direction < 0 && facingRight)
            Flip();

        // Gerakan dash ke target posisi selama variabel dash bernilai true
        if(dash)
        {
            myRigidBody.velocity = new Vector2((dashTarget.x - myRigidBody.position.x)*10f, 0);
            dash = (Mathf.Abs(myRigidBody.position.x - dashTarget.x) > 0.5f) && (dashTime <= 0.5f);
            dashTime += Time.deltaTime;

            return;
        }
      
        // Gerakan horizontal (jalan/lari) jika pemain menyentuh lantai atau berada di udara
        if(grounded)
        {
            myRigidBody.velocity = new Vector2(direction * currentSpeed, myRigidBody.velocity.y);
        }
        else
        {
            float diff = (direction == 0 ? 0 : runningSpeed * direction - myRigidBody.velocity.x);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x + diff * airControl, myRigidBody.velocity.y);
        }
    }

    // Fungsi lompat dengan menambahkan impuls gaya ke atas pada vector velocity myRigidBody
    public void Jump()
    {
        if(jumpCounter == 2 && !grounded)
            return;

        if(grounded)
            jumpCounter = 0;

        dash = false;
        myRigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpCounter++;
    }

    // Membalikan arah objek pemain menghadap
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    // Cek apakah objek pemain mengalami collision dengan objek pada layer tanah/ground
    public bool checkGround()
    {
        grounded = playerCollider.IsTouchingLayers(whatIsGround.value);

        if(grounded && dashCounter != 0)
            dashCounter = 0;

        return grounded;
    }

    // Getter
    public Vector2 getPosition()
    {
        return myRigidBody.position;
    }

    public Vector2 getVelocity()
    {
        return myRigidBody.velocity;
    }
}
