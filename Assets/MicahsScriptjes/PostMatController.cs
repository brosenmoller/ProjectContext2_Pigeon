using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostMatController : MonoBehaviour
{
    [SerializeField] private Material postMat;
    private float fadeIntensity;
    [HideInInspector] public float fadeTarget;
    [SerializeField] private float fadeSpeed;
    [Header("Debug (E = fade in, F = fade out)")]
    [SerializeField] private bool debugMode;
    void Start()
    {
        // make sure that _FadeIntensity always starts at 0
        fadeIntensity = 0;
        fadeTarget = -0.5f;
        postMat.SetFloat("_FadeIntensity", fadeIntensity);
    }
    void Update()
    {
        // lerp fade intensity to desired target (-0.5 for no fade, 1.5 for fade)
        fadeIntensity = Mathf.Clamp(Mathf.Lerp(fadeIntensity, fadeTarget, fadeSpeed * Time.unscaledDeltaTime), 0, 1);
        postMat.SetFloat("_FadeIntensity", fadeIntensity);

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
        // reset when game view is exited
        postMat.SetFloat("_FadeIntensity", 0);
    }
}
