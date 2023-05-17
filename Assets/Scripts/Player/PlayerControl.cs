using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private PlayerMovement controller;
    [SerializeField] private Animator anime;
    private float moveDirection;
    private int jumpCounter = 0, maxJumpCount = 2;
    private bool jumpBegin = false, dashBegin = false, attackBegin = false;

    // Kontrol Animasi ===============================================
    public enum PlayerAnimation {
        Idle,
        Walk,
        Run,
        Dash,
        Jump,
        Fall,
        NATK1,
        NATK2
    }

    public Dictionary<PlayerAnimation, string> animationName = new Dictionary<PlayerAnimation, string>(){
        {PlayerAnimation.Idle, "idle"},
        {PlayerAnimation.Walk, "walking"},
        {PlayerAnimation.Run, "running"},
        {PlayerAnimation.Dash, "dash"},
        {PlayerAnimation.Jump, "jump"},
        {PlayerAnimation.Fall, "fall"},
        {PlayerAnimation.NATK1, "normal_attack_1"},
        {PlayerAnimation.NATK2, "normal_attack_2"}
    };

    PlayerAnimation currentAnimationState = PlayerAnimation.Idle;
    private bool isAnimationPaused = false;
    // ===============================================================

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.LeftControl))
            controller.changeRunningState();

        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (jumpCounter != maxJumpCount || controller.grounded))
        {
            jumpBegin = true;

            if(controller.grounded) jumpCounter = 0;
            jumpCounter++;
        }

        if(Input.GetKeyDown(KeyCode.Space))
            dashBegin = true;

        if(Input.GetKeyDown(KeyCode.X))
            attackBegin = true;

    }

    void FixedUpdate()
    {
        // Start actions
        if(dashBegin) 
        {
            controller.startDash(moveDirection);
            dashBegin = false;
        }
           
        if(jumpBegin) 
        {
            controller.Jump();
            jumpBegin = false;
        }

        if(attackBegin)
        {
            controller.startAttack();
            attackBegin = false;
        }
        
        controller.MoveHorizontally(moveDirection);

        animate();
    }
    
    // Jalankan animasi
    void animate()
    {
        if(controller.isDashing) {
            changeAnimationState(PlayerAnimation.Dash);
            return;
        }

        if(controller.isAttacking) {
            switch(controller.attackNo) {
                case 0:
                    changeAnimationState(PlayerAnimation.NATK1);
                    break;
                case 1:
                    changeAnimationState(PlayerAnimation.NATK2);
                    break;
            }
            return;
        }
        
        if(controller.grounded)
        {
            if(moveDirection != 0)
                changeAnimationState(controller.isRunning ? PlayerAnimation.Run : PlayerAnimation.Walk);
            else
                changeAnimationState(PlayerAnimation.Idle);
        }
        else
        {
            float yVelocity = controller.getVel().y;

            if(yVelocity > 0)
                changeAnimationState(PlayerAnimation.Jump);
            else
                changeAnimationState(PlayerAnimation.Fall);
        }
    }

    void changeAnimationState(PlayerAnimation newState)
    {
        if(currentAnimationState == newState)
            return;
        
        anime.Play(animationName[newState]);
        currentAnimationState = newState;
    }

    public IEnumerator jumpAnimationPause()
    {
        if(isAnimationPaused)
            yield break;
        
        isAnimationPaused = true;
        pauseAnimation();

        while(currentAnimationState == PlayerAnimation.Jump && controller.getVel().y > 1f)
            yield return null;
        
        isAnimationPaused = false;
        continueAnimation();
    }

    public IEnumerator fallAnimationPause()
    {
        if(isAnimationPaused)
            yield break;
        
        isAnimationPaused = true;
        pauseAnimation();

        while(currentAnimationState == PlayerAnimation.Fall && !controller.grounded)
            yield return null;
        
        isAnimationPaused = false;
        continueAnimation();
    }

    public IEnumerator attackAnimationPause()
    {
        if(isAnimationPaused)
            yield break;
        
        isAnimationPaused = true;
        pauseAnimation();

        while(controller.isAttacking && !attackBegin)
            yield return null;
        
        isAnimationPaused = false;
        continueAnimation();
    }

    public void pauseAnimation()
    {
        anime.speed = 0;
    }

    public void continueAnimation()
    {
        anime.speed = 1f;
    }
}
