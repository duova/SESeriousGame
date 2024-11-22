using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fact
{
    public int stage;

    [TextArea]
    public string text;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantFeature")]
public class PlantFeatureScriptableObject : ScriptableObject
{
    public string displayName;

    public string displayNamePlural;

    public string alternateDisplayName;

    public string alternateDisplayNamePlural;
    
    public PlantFeatureTypeScriptableObject featureType;

    public List<Sprite> sprites;
    
    [TextArea]
    public string description;
    
    public List<Fact> facts;
}