using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMover : MonoBehaviour
{
    public Button sceneMover;
    public EnergyLevel level;

    private void Start()
    {
        Button btn = sceneMover.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (level.energy >= 70)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}