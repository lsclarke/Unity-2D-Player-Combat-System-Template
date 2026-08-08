using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "Scriptable Objects/PlayerMovement")]
public class PlayerMovement : ScriptableObject
{

    /// <summary>
    /// Vector variables
    /// </summary>
    public Vector2 inputDirection;

    /// <summary>
    /// Bool variables
    /// </summary>
    private bool movementSwitch = true;

    public bool canMove
    {
        get { return movementSwitch; }
        set { movementSwitch = value; }
    }

    public bool isMoving;
    public bool isWalking;
    public bool isRunning;
    public bool isSprinting;
    public bool isOnGround;
    public bool isJumping;
    public bool Landing;
    public bool isFalling;
    public bool canJump;
    public bool canRun;

    public bool jumpButtonPressed;
    public bool jumpButtonReleased;


    /// <summary>
    /// Float variables
    /// </summary>
    [Range(0f,7f)]
    public float movementSpeed;
    public float SpeedMultiplier;
    public float maxMovementSpeed = 5f;

    public float groundCheckDistance;
    public float jumpSpeed;
    public float jumpHeight;
    public float whenPlayerJumpTimer;


    /// <summary>
    /// KeyCode variables
    /// </summary>
    public KeyCode meleeButton;
    public KeyCode meleeButton2;
    public KeyCode jumpButton;
    public KeyCode runButton;
    public KeyCode cancelWeaponButton;

    /// <summary>
    /// KeyCode variables
    /// </summary>
    public float lineDistance;
    public LayerMask groundLayer;
}