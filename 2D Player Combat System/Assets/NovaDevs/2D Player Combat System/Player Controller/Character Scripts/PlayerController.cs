using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    //Scriptable Object
    public PlayerMovement playerMovement;
    public PlayerAnimationEvents playerAnimationEvents;

    public Rigidbody2D rigidbody2D;
    //World Space Canvas
    public Transform playerCanvas1;

   /// <summary>
   /// Inputs and Action Conditions
   /// </summary>
    private float inputX;
    private float inputY;
    private bool jumpButtonPressed = false;
    private bool horizontalButtonPressed = false;


    /// <summary>
    /// Animations
    /// </summary>
    [SerializeField]
    private Animator player1Animator;
    [SerializeField]
    private Animator player2Animator;
    private int dice = 0;
    private bool rolled = false;


    /// <summary>
    /// Ground Detection
    /// </summary>
    RaycastHit2D hitGround;
    public float lineDistance;
    public LayerMask groundLayer;
    [SerializeField]
    private Transform groundCheckObj;
    public float fallTimer = 0f;
    private float originalCheckDistance;

    /// <summary>
    /// Input Action Vars
    /// </summary>
    public InputActionAsset inputSystemsActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    private void OnEnable()
    {
        inputSystemsActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputSystemsActions.FindActionMap("Player").Disable();
    }

    //Awake() is called as soon as the script has loaded in the scene or been instantiated.
    private void Awake()
    {
        moveAction = inputSystemsActions.FindAction("Move");
        jumpAction = inputSystemsActions.FindAction("Jump");
        runAction = inputSystemsActions.FindAction("Sprint");
    }

    //Start() is the called right before the first update frame. It is also called after all instances have completed their `Awake`function call.
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerMovement.groundCheckDistance = 0.15f;
        originalCheckDistance = playerMovement.groundCheckDistance;
    }

    /// <summary>
    /// Main method responsible for the the input controls and handling all player actions
    /// </summary>
    private void Controller()
    {
        playerMovement.inputDirection.x = moveAction.ReadValue<Vector2>().x;
        playerMovement.inputDirection.y = moveAction.ReadValue<Vector2>().y;

        inputX = playerMovement.inputDirection.x;
        inputY = playerMovement.inputDirection.y;


        //Jump Button
        if (jumpAction.WasPressedThisFrame() && playerMovement.canJump)
        {
            ApplyjumpForce();
            Debug.Log($"Jump was pressed: {jumpButtonPressed}");
        }

        //Run Button
        if (runAction.WasPressedThisFrame() && playerMovement.canRun)
        {
            playerMovement.isRunning = true;
            dice = UnityEngine.Random.Range(1,3);
            //player1Animator.SetInteger("AltRunDice", dice);
            Debug.Log($"Alt Run Animation Dice Value: {dice}");
        }
        else if(runAction.WasReleasedThisFrame()){
            dice = 0;
            //player1Animator.SetInteger("AltRunDice", dice);
            Debug.Log($"Alt Run Animation Dice Value: {dice}");
            playerMovement.isRunning = false;
        }

        LinkToAnimator();
        JumpFallLogic();
    }

    private void PlayerPhysics()
    {
        CheckGround();//Responsible for detecting the ground and applying additional logic
        ///Player movement based on AddForce, if the player is not moving then apply force in the horizontal direction being pressed.
        ///When the player releases the button the force will be reduced to 0
        if (playerMovement.canMove)
        {
            if (inputX != 0f)
            {
                Friction();
                //Apply horizontal mvoement to player (AddForce method)
                rigidbody2D.AddForce(inputX * playerMovement.SpeedMultiplier * Vector2.right, ForceMode2D.Force);
                
                SpeedController();
            }
            else
            {
                const float dec = .1f;
                playerMovement.movementSpeed -= dec;

                if (playerMovement.movementSpeed <= 0f)
                {
                    playerMovement.isWalking = false;
                    playerMovement.movementSpeed = 0f;
                }

                //Apply deceleration Speeed
                rigidbody2D.AddForce(inputX * -playerMovement.SpeedMultiplier * Vector2.right, ForceMode2D.Force);
            }
        }
    }

    /// <summary>
    /// Calculates the speed, acceleration and deceleration of player when the player is in motion
    /// </summary>
    private void SpeedController()
    {
        const float acc = 0.05f;
        const float dec = 0.05f;

        if (playerMovement.movementSpeed != playerMovement.maxMovementSpeed)
        {
            playerMovement.isSprinting = false;
        }
        else
        {
            playerMovement.isSprinting = true;
        }

        if (playerMovement.movementSpeed != playerMovement.maxMovementSpeed && playerMovement.isRunning)
        {
            playerMovement.movementSpeed += acc;
            playerMovement.isWalking = false;

            if (playerMovement.movementSpeed >= playerMovement.maxMovementSpeed)
            {
                playerMovement.movementSpeed = playerMovement.maxMovementSpeed;
            }
        }
        else if (!playerMovement.isRunning) //Not  running
        {
            playerMovement.isWalking = true;
            playerMovement.isSprinting = false;

            if (playerMovement.movementSpeed > 2f)
            {
                playerMovement.movementSpeed -= dec;
                if (playerMovement.movementSpeed <= 2f)
                {
                    playerMovement.movementSpeed = 2f;
                }
            }
            if (playerMovement.movementSpeed < 2f)
            {
                playerMovement.isWalking = true;
                playerMovement.movementSpeed += acc;

                if (playerMovement.movementSpeed >= 2f && !playerMovement.isRunning)
                {
                    playerMovement.movementSpeed = 2f;
                    playerMovement.isWalking = true;
                }
            }
        }

    }

    /// <summary>
    /// Checks for when the player makes contact with the ground. The funtion will create a raycast that detects the gameobject with the groundLayer value.
    /// When the player is grounded the player be able to jump and can run.
    /// </summary>
    public void CheckGround()
    {
        hitGround = Physics2D.Raycast(groundCheckObj.position, Vector2.down, lineDistance,groundLayer);
        Debug.DrawRay(groundCheckObj.position, -transform.up * lineDistance, Color.green);

        if (hitGround.collider != null)
        {
            playerMovement.isOnGround = true; 
            playerMovement.canJump = true;
            playerMovement.isJumping = false;
            playerMovement.canRun = true;
            
        }
        else { 
            
            playerMovement.isOnGround = false;
            playerMovement.canJump = false;
        }

        if (playerMovement.isOnGround)
        {
            playerMovement.isJumping = false;
        }
    }

    /// <summary>
    /// This method is responsible for calculating and applying friction to the player movement.
    /// And limiting the player speed in the air and giving the player some air control
    /// </summary>
    public void Friction()
    {
        if (playerMovement.isOnGround || jumpButtonPressed)
        {
            float continuedMovement = playerMovement.inputDirection.x * playerMovement.movementSpeed;

            if (Mathf.Abs(rigidbody2D.linearVelocity.x) > 0)
            {
                continuedMovement -= 0.00001f;
                rigidbody2D.linearVelocity = new Vector2(continuedMovement, rigidbody2D.linearVelocity.y);
            }
        }
        else
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, rigidbody2D.linearVelocity.y);

        }
    }

    private void ApplyjumpForce()
    {
        jumpButtonPressed = true;
        rigidbody2D.AddForce(Vector2.up * playerMovement.jumpSpeed, ForceMode2D.Impulse);
    }

    public void JumpFallLogic()
    {
        //More logic for when button is pressed
        if (jumpButtonPressed && rigidbody2D.linearVelocityY > 0f)
        {
            playerMovement.isJumping = true;
        }

        //When player is falling and landing logic
        if (Mathf.Abs(rigidbody2D.linearVelocityY) > 0f && !playerMovement.isOnGround)
        {
            fallTimer += Time.deltaTime;
        }
    }

    private void LinkToAnimator()
    {
        player1Animator.SetFloat("X Velocity", Mathf.Abs(playerMovement.movementSpeed));
        player1Animator.SetFloat("Y Velocity", Mathf.Abs(rigidbody2D.linearVelocityY));

        player1Animator.SetFloat("Input X", inputX);
        player1Animator.SetFloat("Input Y", Convert.ToInt32(inputY));

        player1Animator.SetBool("On Ground", playerMovement.isOnGround);
        player1Animator.SetInteger("AltRunDice", dice);
        player1Animator.SetFloat("Fall Timer", fallTimer);
        player1Animator.SetBool("JumpButtonPressed", jumpButtonPressed);

        player2Animator.SetFloat("X Velocity", Mathf.Abs(playerMovement.movementSpeed));
        player2Animator.SetFloat("Y Velocity", Mathf.Abs(rigidbody2D.linearVelocityY));

        player2Animator.SetFloat("Input X", inputX);
        player2Animator.SetFloat("Input Y", Convert.ToInt32(inputY));

        player2Animator.SetBool("On Ground", playerMovement.isOnGround);
        player2Animator.SetInteger("AltRunDice", dice);
        player2Animator.SetFloat("Fall Timer", fallTimer);
        player2Animator.SetBool("JumpButtonPressed", jumpButtonPressed);
    }

    #region
    public void JumpPressedOverride()
    {
        jumpButtonPressed = false;
    }

    public void resetFallTimer()
    {
        fallTimer = 0;
    }
    #endregion

    private void FixedUpdate()
    {
        PlayerPhysics();//The main method for controlling the player and all movement actions in the script
    }

    private void Update()
    {
        Controller();
    }
}
