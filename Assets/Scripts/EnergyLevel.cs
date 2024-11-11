using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
/*

- Go Down over time, when level start.
- Go up in single bursts when prompted.
- Go down when moving.

*/
public class EnergyLevel : MonoBehaviour
{

    public Image energyBar;

    public float Energy = 100f;
    public bool inLevel = false;
    public float maxEnergy = 100f;
    private float cost = 10f;

    public void levelUpdate(bool isInLevel){
        inLevel = isInLevel;
    }

    public void eat(float eatenValue){

        if(Energy <= (maxEnergy - eatenValue)){
            Energy += eatenValue;
        } 
        else{Energy = maxEnergy;}

        energyBar.fillAmount = Energy/maxEnergy;
        
    }

    void Update(){
        Debug.Log("AA");

        if(Input.GetKeyDown("a")){
            Energy -= cost;
        }

        if(Input.GetKeyDown("d")){
            Energy -= cost;
        }

        if(inLevel){
            Energy -= Time.deltaTime;
            /*Do something when 0*/
        }

        energyBar.fillAmount = Energy/maxEnergy;
    }

    public EnergyLevel(){
        Debug.Log("AA");
    }
}