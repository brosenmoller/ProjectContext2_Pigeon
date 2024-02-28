using System.Collections;
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
    
    private CompassController compassController;

    private TypingEffect storyTextTypingEffect;

    private AudioSource audioSource;
    private const float subtitleDelay = 1.0f;
    void Start()
    {
        storyTextTypingEffect = storyText.GetComponent<TypingEffect>();
        compassController = FindObjectOfType<CompassController>();

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
        GameManager.Instance.InvokeCityLevelChange(currentScene);
        for (int i = 0; i < cutsceneTriggers.Length; i++)
        {
            if (currentScene == i)
            {
                compassController.objectiveObjectTransform = cutsceneTriggers[i].transform;
                cutsceneTriggers[i].IsTarget = true;
            }
            else
            {
                cutsceneTriggers[i].IsTarget = false;
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
        foreach (var story in currentStory.stories)
        {
            storyTextTypingEffect.SetText(story.subtitle, 3.0f);

            if (story.audioClip != null)
            {
                audioSource.PlayOneShot(story.audioClip);
                yield return new WaitForSeconds(story.audioClip.length + subtitleDelay);
            }
            else
            {
                yield return new WaitForSeconds(3.0f + subtitleDelay);
            }
        }

        storyTextTypingEffect.ResetText();

        yield return null;
    }
}
