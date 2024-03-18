using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NewsPaperView : UIView
{
    [SerializeField] private Image newspaperImage;
    [SerializeField] private Animator animator;

    public void SetNewsPaper(Sprite sprite, float pauseDuration, Action onComplete)
    {
        newspaperImage.sprite = sprite;
        StartCoroutine(NewspaperPopup(pauseDuration, onComplete));
    }

    private IEnumerator NewspaperPopup(float pauseDuration, Action onComplete)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(pauseDuration);

        animator.SetTrigger("End");

        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.1f);

        onComplete?.Invoke();
    }
}
