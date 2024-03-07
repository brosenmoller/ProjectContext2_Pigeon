using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image blackScreenUI;
    [SerializeField] private float fadeSpeed;

    [Header("Debug (E = fade in, F = fade out)")]
    [SerializeField] private bool debugMode;

    [HideInInspector] public float fadeTarget;
    private float fadeIntensity;

    private void Start()
    {
        fadeIntensity = 0;
        fadeTarget = -0.5f;
        SetAlpha(fadeIntensity);
    }

    private void Update()
    {
        // lerp fade intensity to desired target (-0.5 for no fade, 1.5 for fade)
        fadeIntensity = Mathf.Clamp(Mathf.Lerp(fadeIntensity, fadeTarget, fadeSpeed * Time.unscaledDeltaTime), 0, 1);
        SetAlpha(fadeIntensity);

        // if true, debug by pressing E or F
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                fadeTarget = 1.5f;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                fadeTarget = -0.5f;
            }
        }
    }

    private void OnDestroy()
    {
        SetAlpha(0);
    }

    private void SetAlpha(float alpha)
    {
        Color color = blackScreenUI.color;
        color.a = alpha;
        blackScreenUI.color = color;
    }
}
