using Cinemachine;
using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Story")]
    [SerializeField] private Story story;

    [Header("Cutscene Settings")]
    [SerializeField, Range(0, 1)] private float slowMotionIntensity;
    [SerializeField] private float cutsceneDelay;
    [SerializeField] private float cutsceneLength;
    [SerializeField] private float subtitleDelay;
    [SerializeField] private bool requireKeyPressForNextLine;

    [Header("References")]
    [SerializeField] private Transform cutsceneElements;
    [SerializeField] private Transform playerEndLocation;
    [SerializeField] private MeshRenderer pillarRenderer;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioSource narrationSource;

    private const float fadeMax = 1.5f;
    private const float fadeMin = -1.5f;
    private const float timeScaleConst = 1f;
    private const float fixedDeltaTimeConst = 0.02f;

    private FadeController fadeController;
    private CutsceneManager cutsceneManager;
    private CutSceneView cutSceneView;

    private bool isTarget;
    public bool IsTarget 
    { 
        get { return isTarget; }
        set
        {
            isTarget = value;
            pillarRenderer.enabled = value;
        }
    }

    private void Start()
    {
        fadeController = FindObjectOfType<FadeController>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();

        cutSceneView = (CutSceneView)GameManager.UIViewManager.GetView(typeof(CutSceneView));
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out BirdMovement birdMovement) && IsTarget)
        {
            GameObject player = col.gameObject;
            StartCoroutine(CutsceneRoutine(birdMovement, player));
            cutsceneManager.currentStory = story;
            IsTarget = false;
        }
    }

    private IEnumerator CutsceneRoutine(BirdMovement birdMovement, GameObject player)
    {
        // fade out to cutscene
        fadeController.fadeTarget = fadeMax;
        Time.timeScale = slowMotionIntensity;
        Time.fixedDeltaTime = fixedDeltaTimeConst * (1-slowMotionIntensity);

        PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();
        playerRespawn.enabled = false;

        yield return new WaitForSecondsRealtime(cutsceneDelay);

        // fade in to cutscene
        fadeController.fadeTarget = fadeMin;
        Time.timeScale = timeScaleConst;
        Time.fixedDeltaTime = fixedDeltaTimeConst;

        // Setup Cutscene space
        virtualCamera.Priority = 10;
        cutsceneElements.gameObject.SetActive(true);
        GameManager.UIViewManager.Show(typeof(CutSceneView));

        player.transform.GetChild(0).gameObject.SetActive(false);
        birdMovement.enabled = false;

        yield return StartCoroutine(StoryRoutine());


        // fade out to cutscene
        fadeController.fadeTarget = fadeMax;
        
        // move player
        player.transform.SetPositionAndRotation(playerEndLocation.position, playerEndLocation.rotation);

        yield return new WaitForSecondsRealtime(cutsceneDelay);

        // fade into normal gameplay
        fadeController.fadeTarget = fadeMin;

        // reset Cutscene Elements
        virtualCamera.Priority = 0;
        cutsceneElements.gameObject.SetActive(false);
        GameManager.UIViewManager.Show(typeof(GameView));

        player.transform.GetChild(0).gameObject.SetActive(true);
        birdMovement.enabled = true;
        playerRespawn.enabled = true;

        // set new target
        cutsceneManager.CutsceneCompleted();
    }

    private IEnumerator StoryRoutine()
    {
        cutSceneView.ResetText();
        cutSceneView.ResetSprite();

        foreach (Story.StoryLine storyLine in story.stories)
        {
            if (storyLine.characterSprite != null)
            {
                cutSceneView.SetSprite(storyLine.characterSprite);
            }

            float waitTime;

            if (storyLine.audioClip != null)
            {
                waitTime = storyLine.audioClip.length;
                narrationSource.PlayOneShot(storyLine.audioClip);
            }
            else
            {
                waitTime = 3.0f;
            }

            cutSceneView.SetText(storyLine.subtitle, waitTime);
            
            float timer = 0;
            bool textCompleted = false;
            bool continueToNext = false;
            while (timer < waitTime + subtitleDelay)
            {
                timer += Time.deltaTime;

                if (GameManager.InputManager.controls.GamePlay.Continue.WasPressedThisFrame())
                {
                    if (textCompleted)
                    {
                        continueToNext = true;
                        break;
                    }
                    else
                    {
                        cutSceneView.SetTextDirect(storyLine.subtitle);
                        textCompleted = true;
                    }
                }

                yield return null;
            }

            if (continueToNext)
            {
                continue;
            }

            while (requireKeyPressForNextLine && !GameManager.InputManager.controls.GamePlay.Continue.WasPressedThisFrame())
            {
                yield return null;
            }
        }

        cutSceneView.ResetText();
    }
}
