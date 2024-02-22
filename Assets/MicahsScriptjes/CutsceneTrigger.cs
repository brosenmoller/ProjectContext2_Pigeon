using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    private PostMatController postMatController;
    private CutsceneManager cutsceneManager;
    private const float fadeMax = 1.5f;
    private const float fadeMin = -1.5f;
    private const float timeScaleConst = 1f;
    private const float fixedDeltaTimeConst = 0.02f;
    [SerializeField] private Story story;
    [SerializeField, Range(0, 1)] private float slowMotionIntensity;
    [SerializeField] private float cutsceneDelay;
    [SerializeField] private float cutsceneLength;
    [SerializeField] private Transform cutsceneLocation;
    [SerializeField] private Transform endLocation;
    [HideInInspector] public bool isTarget;
    void Start()
    {
        postMatController = FindObjectOfType<PostMatController>();
        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }
    private void OnTriggerEnter(Collider col)
    {
        // if player enters cutscene zone, play cutscene
        if (col.TryGetComponent(out BirdMovement birdMovement) && isTarget)
        {
            GameObject playerRef = col.gameObject;
            StartCoroutine(CutsceneRoutine(birdMovement, playerRef));
            cutsceneManager.currentStory = story;
            isTarget = false;
        }
    }

    private IEnumerator CutsceneRoutine(BirdMovement birdMovement, GameObject playerRef)
    {
        // fade out to cutscene
        postMatController.fadeTarget = fadeMax;
        Time.timeScale = slowMotionIntensity;
        Time.fixedDeltaTime = fixedDeltaTimeConst * (1-slowMotionIntensity);
        yield return new WaitForSecondsRealtime(cutsceneDelay);
        // move player to cutscene location
        playerRef.transform.position = cutsceneLocation.position;
        playerRef.transform.rotation = cutsceneLocation.rotation;
        birdMovement.enabled = false;
        ICinemachineCamera virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        virtualCamera.OnTargetObjectWarped(Camera.main.transform, cutsceneLocation.position);
        // fade in to cutscene
        postMatController.fadeTarget = fadeMin;
        Time.timeScale = timeScaleConst;
        Time.fixedDeltaTime = fixedDeltaTimeConst;
        cutsceneManager.StoryIntro(cutsceneLength);
        yield return new WaitForSecondsRealtime(cutsceneLength);
        // fade out to cutscene
        postMatController.fadeTarget = fadeMax;
        yield return new WaitForSecondsRealtime(cutsceneDelay);
        // move player back to flying position
        postMatController.fadeTarget = fadeMin;
        birdMovement.enabled = true;
        playerRef.transform.position = endLocation.position;
        playerRef.transform.rotation = endLocation.rotation;
        virtualCamera.OnTargetObjectWarped(Camera.main.transform, endLocation.position);
        cutsceneManager.PlayStory();
        // set new cutscene
        cutsceneManager.SetCurrentScene();
        yield return null;
    }
}