using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionLocation
{
    Falling,
    Foraging,
    Check
}

[Serializable]
public class QuestionEntry
{
    public QuestionTypeScriptableObject questionType;

    public QuestionLocation questionLocation;
    
    public string hint;

    public PlantEntryScriptableObject plant;
    
    public PlantFeatureScriptableObject feature;
    
    [Tooltip("0-1.")]
    public float chanceToUseAlternateName;

    public int stage;

    public bool useSpecificImage;
    
    public int specificImageIndex;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestionLibrary")]
public class QuestionLibraryScriptableObject : ScriptableObject
{
    public List<QuestionEntry> questionEntries;
}