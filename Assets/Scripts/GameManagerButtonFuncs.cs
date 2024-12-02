using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerButtonFuncs : MonoBehaviour
{
    public int spaceship;

    public int journal;
    
    public void ChangeLevel(int level)
    {
        GameManager.Instance.Change(level);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }

    public void ChangeLevelSpaceshipOrJournal()
    {
        List<JournalEntry> newJournalEntries = new List<JournalEntry>();
        
        foreach (var increase in GameManager.Instance.Backend.GetMostRecentStageIncreases())
        {
            newJournalEntries.AddRange(increase.Plant.journalEntries.Where(entry => entry.stage == increase.NewStage));
        }

        if (newJournalEntries.Count > 0)
        {
            GameManager.Instance.Change(journal);
        }
        else
        {
            GameManager.Instance.Change(spaceship);
        }
    }
}