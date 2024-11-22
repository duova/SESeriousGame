using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantEntry")]
public class PlantEntryScriptableObject : ScriptableObject
{
    public string displayName;
    
    public List<PlantFeatureScriptableObject> features;

    public Sprite image;

    [TextArea]
    public string description;
}