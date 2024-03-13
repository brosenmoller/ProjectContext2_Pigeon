using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text, float typingSpeed)
    {
        float speedPerCharacter = typingSpeed / text.Length;

        textMeshPro.text = "";
        StartCoroutine(TypeText(text, speedPerCharacter));
    }

    public void ResetText()
    {
        textMeshPro.text = "";
    }

    public void StopTypingEffect()
    {
        StopAllCoroutines();
    }

    private IEnumerator TypeText(string text, float speedPerCharacter)
    {
        foreach (char c in text)
        {
            textMeshPro.text += c.ToString();
            yield return new WaitForSeconds(speedPerCharacter);
        }
    }
}
