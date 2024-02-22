using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField, Header("Cutscenes played to element order")] private CutsceneTrigger[] cutsceneTriggers;
    private int currentScene = -1;
    public Story currentStory;
    [SerializeField] private GameObject titleObject;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject storyObject;
    [SerializeField] private TextMeshProUGUI storyText;
    private AudioSource audioSource;
    private const float subtitleDelay = 0.3f;
    void Start()
    {
        // set first scene
        SetCurrentScene();
        titleObject.SetActive(false);
        storyObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetCurrentScene()
    {
        // check if there are cutscenes left
        if (currentScene >= cutsceneTriggers.Length) 
        {
            return;
        }
        // set scene according to array order
        currentScene++;
        for (int i = 0; i < cutsceneTriggers.Length; i++)
        {
            if (currentScene == i)
            {
                cutsceneTriggers[i].isTarget = true;
            }
            else
            {
                cutsceneTriggers[i].isTarget = false;
            }
        }
    }

    public void StoryIntro(float cutsceneLength)
    {
        StartCoroutine(ShowTitle(cutsceneLength));
    }

    private IEnumerator ShowTitle(float duration)
    {
        titleObject.SetActive(true);
        titleText.text = currentStory.title; 
        yield return new WaitForSeconds(duration);
        titleText.text = null;
        titleObject.SetActive(false);
    }

    public void PlayStory()
    {
        StartCoroutine(NarrationRoutine());
    }

    private IEnumerator NarrationRoutine()
    {
        storyObject.SetActive(true);
        for (int i = 0; i < currentStory.stories.Length; i++)
        {
            storyText.text = currentStory.stories[i];
            audioSource.PlayOneShot(currentStory.narrationClips[i]);
            yield return new WaitForSeconds(currentStory.narrationClips[i].length + subtitleDelay);
        }
        storyObject.SetActive(false);
        yield return null;
    }
}
