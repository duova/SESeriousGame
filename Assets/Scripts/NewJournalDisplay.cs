using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NewJournalDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject journalDisplayPrefab;

    [SerializeField] 
    private GameObject journalDisplayRoot;

    [SerializeField]
    private float journalDisplayOffset;
    
    private float _journalDisplayCurrentOffset;
    
    void Start()
    {
        List<JournalEntry> newJournalEntries = new List<JournalEntry>();
        
        foreach (var increase in GameManager.Instance.Backend.GetMostRecentStageIncreases())
        {
            newJournalEntries.AddRange(increase.Plant.journalEntries.Where(entry => entry.stage == increase.NewStage));
        }

        float centeringOffset = ((newJournalEntries.Count - 1) / 2f) * journalDisplayOffset;
        
        foreach (var entry in newJournalEntries)
        {
            var display = Instantiate(journalDisplayPrefab, journalDisplayRoot.transform);
            display.transform.localPosition += new Vector3(_journalDisplayCurrentOffset, 0, 0);
            display.transform.localPosition -= new Vector3(centeringOffset, 0, 0);
            display.GetComponentInChildren<TMP_Text>().text = entry.text;
            display.GetComponentInChildren<Image>().sprite = entry.sprite;

            _journalDisplayCurrentOffset += journalDisplayOffset;
        }
    }
}
