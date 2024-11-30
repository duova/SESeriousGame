using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnergyBar : MonoBehaviour
{
    public Image energyBar;

    private void Start()
    {
        energyBar.fillAmount = GameManager.Instance.energyLevel.energy / GameManager.Instance.energyLevel.maxEnergy;
    }

    private void Update()
    {
        energyBar.fillAmount = GameManager.Instance.energyLevel.energy / GameManager.Instance.energyLevel.maxEnergy;
    }
}