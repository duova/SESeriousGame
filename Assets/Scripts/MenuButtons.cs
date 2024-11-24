using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playButton()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
