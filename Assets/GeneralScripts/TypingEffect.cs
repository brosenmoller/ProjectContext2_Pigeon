using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    private TMP_Text textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
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

    private IEnumerator TypeText(string text, float speedPerCharacter)
    {
        foreach (char c in text)
        {
            textMeshPro.text += c.ToString();
            yield return new WaitForSeconds(speedPerCharacter);
        }
    }
}
