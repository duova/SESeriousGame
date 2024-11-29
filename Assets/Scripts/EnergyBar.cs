using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnergyBar : MonoBehaviour
{
    public EnergyLevel energyLevel;
    public Image energyBar;

    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        energyBar.fillAmount = energyLevel.Energy / energyLevel.MaxEnergy;

        if (scene.name == "SampleScene")
        { energyLevel.inLevel = true; }
    }

    private void Update()
    {
        if (energyLevel.inLevel)
        {
            energyLevel.Energy -= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            { energyLevel.Energy -= energyLevel.cost; }

            if (Input.GetKeyDown(KeyCode.E))
            { energyLevel.Eat(30f); }

            if (energyLevel.Energy <= 0)
            {
                energyLevel.Energy = 0;
                energyLevel.inLevel = false;
                SceneManager.LoadScene("ResultScene");
            }
        }
        energyBar.fillAmount = energyLevel.Energy / energyLevel.MaxEnergy;
    }
}