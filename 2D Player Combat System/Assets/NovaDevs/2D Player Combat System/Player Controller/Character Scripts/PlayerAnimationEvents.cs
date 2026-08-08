using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private PlayerCombat playerCombat;
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private PlayerMovement playerMovement;
    public ParticleController particleController;
    public bool isFacingRight;
    public bool hasLanded;
    public Transform playerCanvas1;

    public ComboSheetTextController comboSheet;
    public PlayerLockOnSystem lockOnSystem;
    /// <summary>
    /// Flip gets the current local scale and flips it by multiplying it by -1
    /// and setting the value of isFacingRight to the opposite value (true or false)
    /// </summary>

    private void AnimationFlip()
    {
        if (playerMovement.inputDirection.x < -0.01f && isFacingRight)
        {
            Flip();
        }

        if (playerMovement.inputDirection.x > 0.01f && !isFacingRight)
        {
            Flip();
        }


        if (isFacingRight)
        {
            playerCanvas1.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            playerCanvas1.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void Flip()
    {
        Vector3 currentLocalScale = this.transform.localScale;
        currentLocalScale.x *= -1;
        transform.localScale = currentLocalScale;   
        isFacingRight = !isFacingRight;
    }

    public void CheckForNextInput()
    {
        playerCombat.ResetInput();
        playerCombat.meleeButtonPressed = false;
        playerCombat.melee2ButtonPressed = false;
    }

    public void CheckWhenPlayerLands()
    {
        playerController.JumpPressedOverride();
        playerController.resetFallTimer();

    }

    public void FootStepVFX()
    {
        particleController.SpawnParticle();
    }

    public void CheckPlayerLanding()
    {
        hasLanded = true;
        particleController.SpawnParticle();
    }
    public void CheckPlayerLandingNot()
    {
        hasLanded = false;
    }

    public void PlayerAttackingBoolCheckOn()
    {
        playerCombat.isAttacking = true;
    }

    public void PlayerAttackingBoolCheckOff()
    {
        playerCombat.isAttacking = false;
    }

    public void EnterCombatMode()
    {
        playerCombat.CombatModeOn();
    }

    public void LeaveCombatMode()
    {
        playerCombat.CombatModeOff();
    }

    public void UpdateComboListText()
    {
        if (playerCombat.melee2ButtonPressed)
        {
            comboSheet.stringName = "Melee 2";
        }

        if (playerCombat.meleeButtonPressed)
        {
            comboSheet.stringName = "Melee 1";
        }

    }

    public void ButtonClickResetOveride()
    {
        playerCombat.ResetButtonClick();
    }

    public void TimeOutInput()
    {
        playerCombat.noInput();
        playerCombat.meleeButtonPressed = false;
    }

    public void enterCombatIdle()
    {
        playerCombat.meleeButtonPressed = false;
        playerCombat.ResetInput();
        playerCombat.ResetButtonClick();
    }

    private void Update()
    {
        AnimationFlip();
    }
}
