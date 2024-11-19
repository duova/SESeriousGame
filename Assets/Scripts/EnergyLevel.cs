using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public float cost = 0.05f;

    public void LevelUpdate(bool isInLevel){
        inLevel = isInLevel;
    }

    public void Eat(float eatenValue){

        if(Energy < (maxEnergy - eatenValue)){Energy += eatenValue;} 
        else{Energy = maxEnergy;}

        energyBar.fillAmount = Energy / maxEnergy;
    }

    private void Update(){

        if(inLevel){
            Energy -= Time.deltaTime;
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
                Energy -= cost;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Eat(10f);
            }

            if (Energy <= 0) {
                SceneManager.LoadScene("ClickingSample");
                inLevel = false;
            }
            energyBar.fillAmount = Energy/maxEnergy;
        }
        
    }
    public EnergyLevel(){
    }
}