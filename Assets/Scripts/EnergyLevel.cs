using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnergyLevel
{
    public float tickedCost = 0.08f;
    public float moveCost = 1f;
    public float energy= 100f;
    public float maxEnergy = 100f;

    public void Eat(float nutritionalValue)
    {
        if(energy < (maxEnergy - nutritionalValue)) { energy += nutritionalValue; }
        else { energy = maxEnergy; }
    }
}