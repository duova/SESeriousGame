using System;
using System.Collections.Generic;
using UnityEngine;

/*
//Stuff like oak leaf, oak seed, oak twig.
[Serializable]
public struct PlantFeature
{
    //Must be unique across features and types.
    public string uniqueName;

    public string displayName;

    public Sprite image;

    public bool isEdible;
}

//The type of plant like oak, beech, maple.
[Serializable]
public struct PlantTypeEntry
{
    //Must be unique across features and types.
    public string uniqueName;

    public string displayName;

    public Sprite image;

    //Must be in order of level, eg. index 0 is level 1 and index 3 is level 4.
    public List<PlantFeature> features;
}

public struct PlantTypeLevelData
{
    //Index corresponds to index of plant type entries in a plant library.
    //0 means not unlocked.
    public List<int> Levels;
}

//Object that can be placed in the file system to be used from anywhere.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantLibrary")]
public class PlantLibraryScriptableObject : ScriptableObject
{
    public List<PlantTypeEntry> plantEntries;
}
*/