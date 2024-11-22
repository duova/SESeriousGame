using System.Collections.Generic;
using UnityEngine;

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
}