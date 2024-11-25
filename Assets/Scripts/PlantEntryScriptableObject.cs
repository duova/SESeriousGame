using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JournalEntry
{
    public int stage;

    [TextArea]
    public string text;

    public Sprite sprite;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantEntry")]
public class PlantEntryScriptableObject : ScriptableObject
{
    public string displayName;
    
    public List<PlantFeatureScriptableObject> features;

    public Sprite image;

    [TextArea(10, 10)]
    public string description;

    public List<JournalEntry> journalEntries;
}