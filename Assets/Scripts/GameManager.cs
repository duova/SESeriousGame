using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; private set; }

    public int environment;
    
    public PlantLibraryScriptableObject plantLibrary;
    public QuestionLibraryScriptableObject questionLibrary;
    public DefaultPlantBackend Backend;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        
        Instance = this;
        Backend = new DefaultPlantBackend(plantLibrary, questionLibrary);
    }
    
    [SerializeField]
    private bool fadeOut;

    [SerializeField]
    private float fadeTime = 0.5f;

    private int _requestedIndex;

    [SerializeField]
    private int resultScreenIndex;

    public void Change(int index)
    {
        _requestedIndex = index;
        if (!fadeOut)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            ScreenFade.Instance.FadeOut(fadeTime, PostFade);
        }
    }

    private void PostFade()
    {
        SceneManager.LoadScene(_requestedIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public EnergyLevel energyLevel;

    private void Update()
    {
        if (energyLevel.energy <= 0)
        {
            energyLevel.energy = 1;
            Change(resultScreenIndex);
        }
    }
}
