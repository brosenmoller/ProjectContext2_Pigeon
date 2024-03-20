using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscenes played to element order")]
    [SerializeField] private Cutscene[] cutscenes;

    private int currentSceneIndex = -1;
    public Story currentStory;

    private CompassController compassController;

    private void Start()
    {
        compassController = FindObjectOfType<CompassController>();

        // Set first scene
        SetNextScene();
    }

    public void CutsceneCompleted()
    {
        cutscenes[currentSceneIndex].trigger.IsTarget = false;
        cutscenes[currentSceneIndex].journalist.Activate();
        compassController.objectiveObjectTransform = cutscenes[currentSceneIndex].journalist.transform;
    }

    public void JournalistCompleted()
    {
        SetNextScene();
    }

    public void SetNextScene()
    {
        // Check if there are cutscenes left
        if (currentSceneIndex >= cutscenes.Length - 1) 
        {
            GameManager.UIViewManager.Show(typeof(GameEndView));
            return;
        }

        // Set scene according to array order
        currentSceneIndex++;
        GameManager.Instance.InvokeCityLevelChange(currentSceneIndex);
        for (int i = 0; i < cutscenes.Length; i++)
        {
            if (currentSceneIndex == i)
            {
                compassController.objectiveObjectTransform = cutscenes[i].trigger.transform;
                cutscenes[i].trigger.IsTarget = true;
            }
            else
            {
                cutscenes[i].trigger.IsTarget = false;
            }
        }
    }

    [System.Serializable]
    public class Cutscene
    {
        public CutsceneTrigger trigger;
        public JournalistController journalist;
    }
}
