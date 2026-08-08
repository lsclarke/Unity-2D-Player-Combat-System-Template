using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DummySack : MonoBehaviour, IDamagable
{
    private bool isHit = false;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer sprite;
    public bool stunned;

    public bool hasSuperAmour;
    public float superAmourMeter;

    public bool resetDummy;
    public float timeScale;
    public float pauseTime;
    int dice = 0;

    private float canvasShowTime = 4f;
    private float canvasShowTimer;

    public GameObject DummyCanvasGroup;
    public DummyCanvas dummyCanvasObject;
    public ParticleSystem hitParticleSystem;
    public ParticleSystem hitParticleSystem2;
    public ParticleSystem hitParticleSystem3;
    public GameObject dizzyParticle;
    private float originalPauseTime;
    public bool isFacingRight;

    public Transform lookAt;
    public bool isAlert;

    public int ID;

    public GameObject lockOnCanvasObject;

    public void Start()
    {
        resetDummy = false;
        animator = GameObject.Find("Dummy Sprite").GetComponent<Animator>();
        sprite = GameObject.Find("Dummy Sprite").GetComponent<SpriteRenderer>();
        canvasShowTimer = 0f;
        dizzyParticle.SetActive(false);
        hitParticleSystem3.Stop();
        stunned = false;
        originalPauseTime = pauseTime;
        lockOnCanvasObject.SetActive(false);
    }    /// <summary>
         /// Flip gets the current local scale and flips it by multiplying it by -1
         /// and setting the value of isFacingRight to the opposite value (true or false)
         /// </summary>

    private void AnimationFlip()
    {

        if (lookAt.position.x < this.transform.position.x && isFacingRight)
        {
            Flip();
            DummyCanvasGroup.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        if (lookAt.position.x > this.transform.position.x && !isFacingRight)
        {
            Flip();
            DummyCanvasGroup.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }


        //if (isFacingRight)
        //{
        //    DummyCanvasGroup.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //}
        //else
        //{
        //    DummyCanvasGroup.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        //}
    }
    public void SetStunned(bool value)
    {
        stunned = value;
    }

    public bool isStunned()
    {
        return stunned;
    }

    private void Flip()
    {
        Vector3 currentLocalScale = this.transform.localScale;
        currentLocalScale.x *= -1;
        transform.localScale = currentLocalScale;
        isFacingRight = !isFacingRight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player Hit Box"))
        {
            isHit = true;
            animator.SetBool("isHit", isHit);

            if (stunned)
            {
                dummyCanvasObject.damage = 10f;
                StartCoroutine(StunnedHit());
            }
            else
            {
                dummyCanvasObject.damage = 5f;
                StartCoroutine(HitStop(pauseTime));
            }
            StartCoroutine(FlashAnimation());
            DummyCanvasGroup.SetActive(true);
            if (isHit)
            {
                dice = Random.Range(0, 3);
                dummyCanvasObject.onHit();
                animator.SetInteger("Dice", dice);
                StartCoroutine(ResetAnimation());

                if (!stunned) isAlert = true;
            }
            canvasShowTimer = canvasShowTime;
            canvasShowTimer -= Time.deltaTime;

            if (canvasShowTimer <= 0f)
            {
                DummyCanvasGroup.SetActive(false);
                isAlert = false;
                canvasShowTimer = 0f;
            }
        }
    }

    public void AddKnockBack(GameObject other) => throw new System.NotImplementedException();

    public IEnumerator CancelKnockBack(float time) => throw new System.NotImplementedException();
    public IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        isHit = false;
        dice = 0;
        animator.SetBool("isHit", isHit);
        animator.SetInteger("Dice", dice);
    }

    public IEnumerator FlashAnimation()
    {
        hitParticleSystem.Play();
        sprite.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(.15f);
        hitParticleSystem2.Play();
        sprite.material.SetInt("_Flash", 0);
        yield return new WaitForSeconds(.25f);
        hitParticleSystem.Stop();
        sprite.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(.15f);
        hitParticleSystem.Stop();
        sprite.material.SetInt("_Flash", 0);
        yield return new WaitForSeconds(.15f);
        sprite.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(.15f);
        sprite.material.SetInt("_Flash", 0);

    }

    public IEnumerator StunnedHit()
    {

        hitParticleSystem3.Play();
        yield return new WaitForSeconds(1.2f);
        hitParticleSystem3.Stop();

    }

    public IEnumerator HitStop(float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
    }

    public IEnumerator UnStagger(float duration)
    {
        yield return new WaitForSeconds(duration);
        dizzyParticle.SetActive(false);
        stunned = false;
        dummyCanvasObject.staggerMeterSlider.value = 0f;
    }

    public void SetLockOnUI(bool check)
    {
        lockOnCanvasObject.SetActive(check);
    }

    public void Update()
    {
        if (isAlert)
        {
            AnimationFlip();
        }
        if (dummyCanvasObject.staggerMeterSlider.value >= dummyCanvasObject.staggerMeterSlider.maxValue)
        {
            stunned = true;
            StartCoroutine(UnStagger(3f));
        }

        if (dummyCanvasObject.staggerMeterSlider.value == 0f)
        {
            stunned = false;
        }

        if (stunned)
        {
            pauseTime = 0.00035f;
            isAlert = false;
            dizzyParticle.SetActive(true);
        }
        else
        {
            pauseTime = originalPauseTime;
        }
    }

}
