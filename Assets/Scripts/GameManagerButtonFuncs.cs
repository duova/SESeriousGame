using UnityEngine;

public class GameManagerButtonFuncs : MonoBehaviour
{
    public void ChangeLevel(int level)
    {
        GameManager.Instance.Change(level);
    }
}