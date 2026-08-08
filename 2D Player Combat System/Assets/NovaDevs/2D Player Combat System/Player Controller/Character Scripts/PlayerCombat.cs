using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    public bool meleeButtonPressed;
    public bool meleeButtonHold;

    public bool melee2ButtonPressed;

    public bool canReceiveInput;
    public PlayerMovement playerMovement;
    [SerializeField]
    private Animator playerAnimator1;

    [SerializeField]
    private Animator playerAnimator2;
    [SerializeField]
    private PlayerController playerController;

    public int buttonClick;
    public float resetModeTimer = 0f;
    public bool isAttacking;
    public bool isCombatMode;

    /// <summary>
    /// Input Action Vars
    /// </summary>
    public InputActionAsset inputSystemsActions;
    private InputAction melee1Action;
    private InputAction melee2Action;

    public float holdMeleeTimer;
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
        melee1Action = inputSystemsActions.FindAction("Melee 1");
        melee2Action = inputSystemsActions.FindAction("Melee 2");
    }

    //Start() is the called right before the first update frame. It is also called after all instances have completed their `Awake`function call.
    /// <summary>
    /// This function should check when the player presses the attack button. 
    /// When the player presses it continuosly the plyer will cycle through different attack combo animations
    /// </summary>

    private void Start()
    {
        canReceiveInput = true;
        resetModeTimer = 0f;
        holdMeleeTimer = 0;
    }

    /// <summary>
    /// This funtion checks for when the player presses the attack buttons
    /// </summary>
    public void CombatInputRecieved()
    {
        playerAnimator1.SetBool("Combat Mode", isCombatMode);
        playerAnimator1.SetInteger("Combat Button Clicks", buttonClick);
        playerAnimator1.SetBool("MeleeButton2Pressed", melee2ButtonPressed);

        playerAnimator2.SetBool("Combat Mode", isCombatMode);
        playerAnimator2.SetInteger("Combat Button Clicks", buttonClick);
        playerAnimator2.SetBool("MeleeButton2Pressed", melee2ButtonPressed);

        /// <summary>
        /// When the player presses the melee button and is able to recieve inputs it will signal the player to trigger the melee attack based on the buttonClick int var
        /// </summary>

        if (melee1Action.WasPressedThisFrame() && canReceiveInput)
        {
            buttonClick++;
            canReceiveInput = false;
            meleeButtonPressed = true;
            melee2ButtonPressed = false;
        }

        if (melee1Action.WasPerformedThisFrame() && canReceiveInput)
        {
            meleeButtonHold = true; 
        }

        if (melee2Action.WasPressedThisFrame() && canReceiveInput && buttonClick > 0)
        {
            buttonClick++;
            canReceiveInput = false;
            melee2ButtonPressed = true;
            meleeButtonPressed = false;
        }

        if (meleeButtonPressed || melee2ButtonPressed)
        {
            StartCoroutine("UpdateClickCount");
        }
    }

    public void ResetButtonClick()
    {
        buttonClick = 0;
        meleeButtonPressed = false;
        canReceiveInput = true;
    }

    public void ResetMeleeTwo()
    {
        melee2ButtonPressed = false;
    }

    public void CombatModeOn()
    {
        isCombatMode = true;
    }

    public void CombatModeOff()
    {
        isCombatMode = false;
    }

    public void ResetInput()
    {
        canReceiveInput = true;
        melee2ButtonPressed = false;
    }

    public void noInput()
    {
        canReceiveInput = false;
    }
    public IEnumerator UpdateClickCount()
    {
        playerAnimator1.SetTrigger($"Melee Attack {buttonClick}");
        playerAnimator2.SetTrigger($"Melee Attack {buttonClick}");
         yield return new WaitForSeconds(0.1f);
        //Player is now in combat mode! CombatModeOn() is an animation event in the first attack animation track
        //that allows the player to enter and stay in combat mode
    }

    private void Update()
    {
        CombatInputRecieved();

        //Reset back to adventure mode if player is not fighting
        if (isCombatMode && buttonClick == 0)
        {
            resetModeTimer += Time.deltaTime;

            if(resetModeTimer > 3f)
            {
                isCombatMode = !isCombatMode;
                resetModeTimer = 0;
            }
        }else resetModeTimer = 0;

        if (melee1Action.WasPressedThisFrame()) { holdMeleeTimer += Time.deltaTime; }
        if (melee1Action.WasReleasedThisFrame()) { holdMeleeTimer = Time.deltaTime; }
    }
}
