using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static TimerManager TimerManager { get; private set; }
    public static InputManager InputManager { get; private set; }
    public static UIViewManager UIViewManager { get; private set; }

    private Manager[] activeManagers;


    public event Action<int> OnCityLevelChange;
    public void InvokeCityLevelChange(int level) => OnCityLevelChange?.Invoke(level);


    private void Awake()
    {
        SingletonSetup();
        ManagerSetup();
    }

    private void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ManagerSetup()
    {
        TimerManager = new TimerManager();
        InputManager = new InputManager();
        UIViewManager = new UIViewManager();

        activeManagers = new Manager[] {
            TimerManager,
            InputManager,
            UIViewManager,
        };

        foreach (Manager manager in activeManagers)
        {
            manager.Setup();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnSceneLoad();
        }
    }
    
    private void FixedUpdate()
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnFixedUpdate();
        }
    }
}

