using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private PlayerMovement controller;
    [SerializeField] private Animator anime;
    private float moveDirection;
    private int jumpCounter = 0, maxJumpCount = 2;
    private bool jumpBegin = false, dashBegin = false;

    // Kontrol Animasi ===============================================
    public enum PlayerAnimation {
        Idle,
        Walk,
        Run,
        Dash,
        Jump,
        Fall
    }

    public Dictionary<PlayerAnimation, string> animationName = new Dictionary<PlayerAnimation, string>(){
        {PlayerAnimation.Idle, "idle"},
        {PlayerAnimation.Walk, "walking"},
        {PlayerAnimation.Run, "running"},
        {PlayerAnimation.Dash, "dash"},
        {PlayerAnimation.Jump, "jump"},
        {PlayerAnimation.Fall, "fall"}
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

        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (jumpCounter != maxJumpCount || controller.isGrounded()))
        {
            jumpBegin = true;

            if(controller.isGrounded()) jumpCounter = 0;
            jumpCounter++;
        }

        if(Input.GetKeyDown(KeyCode.Space))
            dashBegin = true;
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
        
        controller.MoveHorizontally(moveDirection);


        // Jalankan Animasi ==============================================
        if(controller.isDashing)
        {
            changeAnimationState(PlayerAnimation.Dash);
        }
        else if(controller.grounded)
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
        // ===============================================================
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

        while(currentAnimationState == PlayerAnimation.Fall && !controller.isGrounded())
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
