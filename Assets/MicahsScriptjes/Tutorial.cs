using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private string[] messages;
    [SerializeField] private float fadeSpeed;
    private float lerpTarget = -0.5f;
    private float alpha;
    private float maxDistance;

    void Start()
    {
        alpha = tutorialText.color.a;
        maxDistance = Screen.width / 3;
        StartTutorial();
    }

    void Update()
    {
        LerpAlpha();
    }
    public void StartTutorial()
    {
        StartCoroutine(TutorialRoutine());
    }

    private IEnumerator TutorialRoutine()
    {
        // instruction 1 (looking with mouse)
        SetText(messages[0]);
        yield return new WaitForSeconds(1);
        Vector2 mousePos = Input.mousePosition;
        float distance = Vector2.Distance(mousePos, Input.mousePosition);
        while (distance < maxDistance)
        {
            distance = Vector2.Distance(mousePos, Input.mousePosition);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SetText(messages[0]);

        // instruction 2 (how to rest)
        yield return new WaitForSeconds(2);
        SetText(messages[1]);
        yield return new WaitForSeconds(1);
        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SetText(messages[1]);

        // instruction 3 (how to boost)
        yield return new WaitForSeconds(2);
        SetText(messages[2]);
        yield return new WaitForSeconds(1);
        while(!Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SetText(messages[2]);

        // instruction 4 (fly to the marker)
        yield return new WaitForSeconds(2);
        SetText(messages[3]);
        yield return new WaitForSeconds(5);
        SetText(messages[3]);

        // disable script
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Tutorial>().enabled = false;
        yield return null;
    }

    private void SetText(string text)
    {
        tutorialText.text = text;
        lerpTarget = (lerpTarget - 0.5f) * -1 + 0.5f;
    }

    private void LerpAlpha()
    {
        alpha = Mathf.Clamp01(Mathf.Lerp(alpha, lerpTarget, fadeSpeed * Time.deltaTime));
        tutorialText.color = new Vector4(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, alpha);
    }
}
