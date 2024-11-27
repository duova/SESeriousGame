using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;
using TMPro;

public class PlantLibraryController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject parent;

    [Header("Plant Button")]
    [SerializeField] private GameObject libraryEntry;


    [SerializeField] private TextMeshProUGUI Display;
    [SerializeField] private TextMeshProUGUI Description;

    private Dictionary<string, PlantLibraryEntry> currentLibrary = new Dictionary<string, PlantLibraryEntry>();

    private void Start()
    {
        for (int i = 0; i < 10; i++) {
            PlantEntryScriptableObject testObject = ScriptableObject.CreateInstance<PlantEntryScriptableObject>();
            testObject.displayName = i.ToString();

            PlantLibraryEntry entry =  updateLibrary(testObject, () => {});
           }
    }

    public PlantLibraryEntry updateLibrary(PlantEntryScriptableObject plant, UnityAction action) {
        PlantLibraryEntry plantLibraryEntry = null;
        if (!currentLibrary.ContainsKey(plant.displayName))
        {
            plantLibraryEntry = InstantiatePlantEntry(plant, action);
        }
        else {
            plantLibraryEntry = currentLibrary[plant.displayName];
        } return plantLibraryEntry;
    }

    private PlantLibraryEntry InstantiatePlantEntry(PlantEntryScriptableObject plant, UnityAction action)
    {
        PlantLibraryEntry plantEntry = Instantiate(libraryEntry, parent.transform).GetComponent<PlantLibraryEntry>();
        plantEntry.name = plant.displayName;
        plantEntry.parentObject = plant;
      

        plantEntry.Initialize(plant.displayName, action);
        currentLibrary[plant.displayName] = plantEntry;

        plantEntry.button.onClick.AddListener(() => openPlant(plant));
        return plantEntry;
    }

    private void openPlant(PlantEntryScriptableObject plant)
    {
        Display.text = plant.displayName;
        Description.text = plant.description;
    }

}