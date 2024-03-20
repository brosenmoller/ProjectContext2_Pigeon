using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutSceneView : UIView
{
    [Header("Refrences")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image characterPortrait;

    private TypingEffect typingEffect;

    private void Awake()
    {
        typingEffect = dialogueText.GetComponent<TypingEffect>();
    }

    public void SetTextDirect(string text)
    {
        typingEffect.StopTypingEffect();
        dialogueText.text = text;
    }

    public void SetText(string text, float typingSpeed) => typingEffect.SetText(text, typingSpeed);
    public void ResetText() => typingEffect.ResetText();

    public void SetSprite(Sprite sprite) => characterPortrait.sprite = sprite;
    public void ResetSprite() => characterPortrait.sprite = null;

}
