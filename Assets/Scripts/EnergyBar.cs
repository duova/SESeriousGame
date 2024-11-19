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
        energyBar.fillAmount = energyLevel.energy / energyLevel.maxEnergy;

        if (scene.name == "SampleScene")
        { energyLevel.inLevel = true; }
    }

    private void Update()
    {
     
        if (energyLevel.inLevel)
        {
            energyLevel.energy -= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            { energyLevel.energy -= energyLevel.cost; }

            if (Input.GetKeyDown(KeyCode.E))
            { energyLevel.Eat(10f); }

            energyBar.fillAmount = energyLevel.energy / energyLevel.maxEnergy;

            if (energyLevel.energy <= 0)
            {
                energyLevel.energy = 0;
                energyLevel.inLevel = false;
                SceneManager.LoadScene("ClickingSample");
            }
        }
    }
}