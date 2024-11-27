using UnityEngine;

public class Persistance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static Persistance instance { get; private set; }
    
    public PlantLibraryScriptableObject plantLibrary;
    public QuestionLibraryScriptableObject questionLibrary;
    public DefaultPlantBackend backend;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;
        backend = new DefaultPlantBackend(plantLibrary, questionLibrary);
    }
    
}
