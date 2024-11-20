using UnityEngine;

[CreateAssetMenu(fileName = "EnergyLevel", menuName ="ScriptableObjects/EnergyLevel",  order = 0)]
public class EnergyLevel : ScriptableObject
{
    public float cost = 0.08f;
    public float Energy= 100f;
    public float MaxEnergy = 100f;

    public bool inLevel = false;  

    public void SceneSwap()
    {
        inLevel = !inLevel;
    }

    public void Eat(float nutritionalValue)
    {
        if(Energy < (MaxEnergy - nutritionalValue)) { Energy += nutritionalValue; }
        else { Energy = MaxEnergy; }
    }
}