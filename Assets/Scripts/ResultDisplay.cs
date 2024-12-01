using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highestStreakText;

    [SerializeField]
    private GameObject plantStageDisplayPrefab;

    [SerializeField] 
    private GameObject plantStageDisplayRoot;

    [SerializeField]
    private float plantStageDisplayOffset;

    [SerializeField]
    private Color increasedStageColor;

    private float _plantStageDisplayCurrentOffset;
    
    void Start()
    {
        highestStreakText.text = "Highest Streak: " + GameManager.Instance.lastSessionStreak;

        foreach (var plant in GameManager.Instance.plantLibrary.plantEntries.Where(plant => plant.environment == GameManager.Instance.environment))
        {
            var plantStage = GameManager.Instance.Backend.GetStage(plant);
            var display = Instantiate(plantStageDisplayPrefab, plantStageDisplayRoot.transform);
            display.transform.localPosition += new Vector3(0, _plantStageDisplayCurrentOffset, 0);
            display.GetComponentInChildren<TMP_Text>().text =
                plant.displayName + " Stage: " + plantStage;
            var barComp = display.GetComponentInChildren<Image>();
            var barTransform = barComp.rectTransform;
            barTransform.sizeDelta += new Vector2(20 * (plantStage - 1), 0);
            barTransform.anchoredPosition += new Vector2(50 * (plantStage - 1), 0);
            
            if (GameManager.Instance.Backend.GetMostRecentStageIncreases().Any(increase => increase.Plant == plant))
            {
                barComp.color = increasedStageColor;
            }

            _plantStageDisplayCurrentOffset += plantStageDisplayOffset;
        }
    }
}
