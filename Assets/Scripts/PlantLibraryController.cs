using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections.Generic;
using TMPro;

public class PlantLibraryController : MonoBehaviour
{
    [Header("ScrollBars")]
    [SerializeField] private GameObject plantParent;

    [Header("ButtonPrefabs")]
    [SerializeField] private GameObject libraryEntry;

    [Header("Fields")]
    [SerializeField] private TextMeshProUGUI Display;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI JournalDescription;
    [SerializeField] private TextMeshProUGUI pageDisp;

    [Header("Images")]
    [SerializeField] private Image Image;
    [SerializeField] private Image JournalDiagram;
    
    [Header("Libraries")]
    [SerializeField] private PlantLibraryScriptableObject plantLibrary;

    private int Page;
    private PlantEntryScriptableObject currentPlant;
    private Dictionary<string, PlantLibraryEntry> currentLibrary = new Dictionary<string, PlantLibraryEntry>();

    private void Start()
    {
        foreach (PlantEntryScriptableObject plant in plantLibrary.plantEntries)
        {
            updateLibrary(plant, () => { });
        }
    }

    public void updateLibrary(PlantEntryScriptableObject plant, UnityAction action) {
        if (!currentLibrary.ContainsKey(plant.displayName))
        {
            InstantiatePlantEntry(plant, action);
        }
    }

    private PlantLibraryEntry InstantiatePlantEntry(PlantEntryScriptableObject plant, UnityAction action)
    {
        PlantLibraryEntry plantEntry = Instantiate(libraryEntry, plantParent.transform).GetComponent<PlantLibraryEntry>();
        plantEntry.name = plant.displayName;
        plantEntry.parentObject = plant;


        plantEntry.Initialize(plant.displayName, action);
        currentLibrary[plant.displayName] = plantEntry;

        plantEntry.button.onClick.AddListener(() => openPlant(plant, 0));
        return plantEntry;
    }

    private void openPlant(PlantEntryScriptableObject plant, int jpage)
    {
        Debug.Log(Persistance.instance.backend.GetStage(plant));
        this.currentPlant = plant;
        Display.text = plant.displayName;
        Description.text = plant.description;
        Image.sprite = plant.image;

        if ((plant.journalEntries.Count >= jpage) && (plant.journalEntries.Count != 0) && (Persistance.instance.backend.GetStage(plant) >= plant.journalEntries[jpage].stage))
        {
            JournalDescription.text = plant.journalEntries[jpage].text;
            JournalDiagram.sprite = plant.journalEntries[jpage].sprite;
            pageDisp.text = jpage.ToString();
            Page = jpage;
        }
        else
        {
            if(JournalDescription.text != "Not Unlocked!")
            {
                pageDisp.text = jpage.ToString();
                Page = jpage;
            }
            JournalDescription.text = "Not Unlocked!";
            JournalDiagram.sprite = null;
        }
    }

    public void updateJournalPage(int direction)
    {
        PlantEntryScriptableObject plant = currentLibrary[Display.text].parentObject;
        int newPage = Page + direction;
        Debug.Log("page: " + newPage + " " + Page);
        openPlant(plant, newPage);
    }
}