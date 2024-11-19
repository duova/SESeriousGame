using UnityEngine;

[CreateAssetMenu(fileName = "EnergyLevel", menuName ="ScriptableObjects/EnergyLevel",  order = 0)]
public class EnergyLevel : ScriptableObject
{
    public float cost = 0.08f;
    public float energy= 100f;
    public float maxEnergy = 100f;
    public bool inLevel = false;  

    public void Eat(float nutritionalValue)
    {
        if(energy < (maxEnergy - nutritionalValue)) { energy += nutritionalValue; }
        else { energy = maxEnergy; }
    }
}