using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.tutorialDone)
        {
            gameObject.SetActive(false);
        }

        GameManager.Instance.tutorialDone = true;
    }
}
