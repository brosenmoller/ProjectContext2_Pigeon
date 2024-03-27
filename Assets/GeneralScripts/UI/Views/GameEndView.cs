using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndView : UIView
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI thankYouForPlayingText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private CinemachineVirtualCamera endVirtualCamera;

    private BirdMovement player;
    private FadeController fadeController;

    private void Awake()
    {
        player = FindObjectOfType<BirdMovement>();
        fadeController = FindObjectOfType<FadeController>();

        thankYouForPlayingText.enabled = false;
        continueText.enabled = false;
    }

    protected override void OnShow()
    {
        StartCoroutine(GameEndRoutine());
    }

    private IEnumerator GameEndRoutine()
    {
        player.enabled = false;
        fadeController.fadeTarget = CutsceneTrigger.fadeMax;

        yield return new WaitForSeconds(4);

        endVirtualCamera.Priority = 100;
        thankYouForPlayingText.enabled = true;

        fadeController.fadeTarget = CutsceneTrigger.fadeMin;

        yield return new WaitForSeconds(1);

        continueText.enabled = true;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
