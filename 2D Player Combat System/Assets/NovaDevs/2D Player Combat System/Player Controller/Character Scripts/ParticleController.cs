using Unity.VisualScripting;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject LandingPartilce;
    public GameObject FootStepParticle;
    [SerializeField]
    private PlayerController playerController;
    public PlayerMovement playerMovement;
    [SerializeField]
    private PlayerAnimationEvents playerAnimationEvents;

    void Flip()
    {
        //Flip game object to direction of running 
        Vector3 currentScale = this.transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    public void SpawnParticle()
    {


        //Landing Particle
        if (playerAnimationEvents.hasLanded)
        {
            var location = new Vector2(this.transform.position.x, this.transform.position.y + .2f);
            Instantiate(LandingPartilce, location, Quaternion.identity);
        }

        //FootSteps Particle
        if (Mathf.Abs(playerMovement.inputDirection.x) > 0.1f && playerMovement.isOnGround)
        {
            var location = new Vector2(this.transform.position.x - .05f, this.transform.position.y + .15f);
            Flip();
            Instantiate(FootStepParticle, location, Quaternion.identity);
        }
        //FootSteps Particle
        if (Mathf.Abs(playerMovement.inputDirection.x) < -0.1f && playerMovement.isOnGround)
        {
            var location = new Vector2(this.transform.position.x - .05f, this.transform.position.y + .15f);
            Flip();
            Instantiate(FootStepParticle, location, Quaternion.identity);
        }

    }
}
