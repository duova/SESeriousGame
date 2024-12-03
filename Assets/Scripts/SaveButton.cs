using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.IO;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    private static readonly string SaveLocation = Application.dataPath + "/Saves";
    
    public void Save()
    {
        List<int> plantEntries = new();

        foreach (var plant in GameManager.Instance.plantLibrary.plantEntries)
        {
            plantEntries.Add(GameManager.Instance.Backend.GetStage(plant));
        }
       
        SaveTemplate newSave = new SaveTemplate
        {
            plantStages = plantEntries,
            TutorialDone = GameManager.Instance.tutorialDone,
        };

        string saveData = JsonUtility.ToJson(newSave);

        Debug.Log(GameManager.Instance.tutorialDone);
        Debug.Log(saveData);

        if (!Directory.Exists(SaveLocation))
        {
            Directory.CreateDirectory((SaveLocation));
        }
        
        File.WriteAllText(SaveLocation + "/save.txt", saveData);
    }

    public void Load()
    {
        string saveData = File.ReadAllText(SaveLocation + "/save.txt");
        SaveTemplate loadedData = JsonUtility.FromJson<SaveTemplate>(saveData);
        var x = 0;

        foreach (var plant in GameManager.Instance.plantLibrary.plantEntries)
        {
            GameManager.Instance.Backend.SetPlantStage(plant, loadedData.plantStages[x]);
        }
        GameManager.Instance.tutorialDone = loadedData.TutorialDone;
    }
    
    private class SaveTemplate
    {
        public List<int> plantStages;
        public bool TutorialDone;
    }
}