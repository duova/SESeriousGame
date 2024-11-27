using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantLibrary")]
public class PlantLibraryScriptableObject : ScriptableObject
{
    public List<PlantEntryScriptableObject> plantEntries;
}