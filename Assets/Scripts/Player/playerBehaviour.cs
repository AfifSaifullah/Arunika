using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehaviour : MonoBehaviour
{
    public MovementControl controller;
    private float moveDirection = 0;
    private bool grounded ;
    private bool jump = false;
    
    public Animator anime;
    private string currentAnimeState; // string untuk menyimpan state animasi sekarang

    // Daftar state animasi ================================ 
    const string PLAYER_IDLE = "idle";
    const string PLAYER_WALK = "walking";
    const string PLAYER_RUN = "running";
    const string PLAYER_DASH = "dash";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_FALL = "fall";
    // =====================================================

    // Cek coroutine fungsi pause sudah dipanggil atau belum
    bool isJumpPauseRunning = false;
    bool isFallPauseRunning = false;


    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.LeftControl))
            controller.changeRunningState();

        if(Input.GetKeyDown(KeyCode.Space))
            jump = true;
           
        if(Input.GetKeyDown(KeyCode.Mouse1))
            controller.startDash();
    }

    // Update is called once per certain time
    void FixedUpdate()
    {
        // KONTROL GERAKAN =================================================================
        grounded = controller.checkGround();
        
        if(jump) {
            controller.Jump();
            jump = false;
        }

        controller.HMove(moveDirection);
        // =================================================================================

        // KONTROL ANIMASI =================================================================
        // (Note: Kalau bisa nanti dipisahkan ke script yang berbeda untuk ngurangin kompleksitas kode)
        switch(currentAnimeState)
        {
            default:
                if(controller.dash) {
                    changeAnimationState(PLAYER_DASH);
                    break;
                }
                    
                if(!grounded && controller.getVelocity().y > 0) {
                    changeAnimationState(PLAYER_JUMP);
                    break;
                }

                if(!grounded && controller.getVelocity().y < 0) {
                    changeAnimationState(PLAYER_FALL);
                    break;
                }
                    
                if(grounded && moveDirection != 0) {
                    changeAnimationState(controller.isRunning ? PLAYER_RUN : PLAYER_WALK);
                    break;
                }

                changeAnimationState(PLAYER_IDLE);
                break;
        }
        // =================================================================================
    }

    // Mengubah state animasi menjadi state baru dan menjalankan animasi yang baru
    void changeAnimationState(string newState)
    {
        if(currentAnimeState == newState)
            return;

        anime.Play(newState);
        currentAnimeState = newState;
    }

    // digunakan untuk menghentikan animasi lompat pada satu frame untuk sementara
    public IEnumerator jumpPause()
    {
        if(isJumpPauseRunning)
            yield break;

        isJumpPauseRunning = true;
        anime.speed = 0f;

        while(controller.getVelocity().y > 1f) {
            yield return null;
        }

        isJumpPauseRunning = false;
        anime.speed = 1f;
    }

    // Dipanggil untuk mengehentikan animasi jatuh pada satu frame untuk semsentara
    public IEnumerator fallPause()
    {
        if(isFallPauseRunning)
            yield break;

        isFallPauseRunning = true;
        anime.speed = 0f;

        while(!controller.checkGround()) {
            yield return null;
        }

        isFallPauseRunning = false;
        anime.speed = 1f;
    }

    // Digunakan untuk menghentikan animasi pada frame tertentu
    public void pauseAnime() {
        anime.speed = 0f;
    }

    // Digunakan untuk menjalankan animasi kembali setelah pause dijalankan
    public void continueAnime() {
        anime.speed = 1f;
    }
}
