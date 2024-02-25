using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f; // Adjust the speed as needed
    private TMP_Text textMeshPro;
    private string fullText;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        fullText = textMeshPro.text;
        textMeshPro.text = ""; // Set the text to empty initially

        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textMeshPro.text += c.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
