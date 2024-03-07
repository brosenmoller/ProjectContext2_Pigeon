using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscenes played to element order")]
    [SerializeField] private CutsceneTrigger[] cutsceneTriggers;

    private int currentScene = -1;
    public Story currentStory;

    private CompassController compassController;

    private void Start()
    {
        compassController = FindObjectOfType<CompassController>();

        // set first scene
        SetCurrentScene();
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
}
