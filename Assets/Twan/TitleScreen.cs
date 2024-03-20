using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class TitleScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delayTillFade = 2f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("References")]
    [SerializeField] private VisualEffect pigeonVFXL;
    [SerializeField] private VisualEffect pigeonVFXR;
    [SerializeField] private AudioSource pigeonSound;
    [SerializeField] private Image blackScreen;

    private bool started;

    private void Start()
    {
        pigeonVFXL.Stop();
        pigeonVFXR.Stop();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !started) 
        {
            StartCoroutine(NextSceneSequence());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator NextSceneSequence()
    {
        started = true;

        pigeonVFXL.Play();
        pigeonVFXR.Play();
        pigeonSound.Play();

        yield return new WaitForSeconds(delayTillFade);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            Color newColor = Color.Lerp(Color.clear, Color.black, time / fadeDuration);
            blackScreen.color = newColor;
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}
