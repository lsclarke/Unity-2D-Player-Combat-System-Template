using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public interface IDamagable
{
    public void AddKnockBack(GameObject other);

    public IEnumerator CancelKnockBack(float time);

    public IEnumerator ResetAnimation();

    public IEnumerator FlashAnimation();

    public IEnumerator HitStop(float duration);
}
