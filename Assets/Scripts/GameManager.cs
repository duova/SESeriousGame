using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.IO;

[Serializable]
public class EnergyLevel
{
    public float tickedCost = 0.08f;
    public float moveCost = 1f;
    public float energy= 100f;
    public float maxEnergy = 100f;

    public void Eat(float nutritionalValue)
    {
        if(energy < (maxEnergy - nutritionalValue)) { energy += nutritionalValue; }
        else { energy = maxEnergy; }
    }
}

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; private set; }

    public int environment;

    public int maxStage;
    
    public PlantLibraryScriptableObject plantLibrary;
    public QuestionLibraryScriptableObject questionLibrary;
    public DefaultPlantBackend Backend;

    public int lastSessionStreak;

    public bool tutorialDone;
    public bool foragingTutorialDone;
    public bool fallingTutorialDone;

    public int endSceneIndex;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        
        Instance = this;
        Backend = new DefaultPlantBackend(plantLibrary, questionLibrary);
    }
    
    [SerializeField]
    private bool fadeOut;

    [SerializeField]
    private float fadeTime = 0.5f;

    private int _requestedIndex;

    [SerializeField]
    private int resultScreenIndex;

    public void Change(int index)
    {
        _requestedIndex = index;
        if (!fadeOut)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            ScreenFade.Instance.FadeOut(fadeTime, PostFade);
        }
    }

    private void PostFade()
    {
        SceneManager.LoadScene(_requestedIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public EnergyLevel energyLevel;

    private void Update()
    {
        if (energyLevel.energy <= 0)
        {
            energyLevel.energy = 50;
            Backend.EndSession();
            Change(resultScreenIndex);
        }
    }

    public void EndGame()
    {
        Change(endSceneIndex);
    }
}


