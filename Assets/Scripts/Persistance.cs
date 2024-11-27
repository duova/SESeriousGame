using UnityEngine;

public class Persistance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlantLibraryScriptableObject plantLibrary;
    public QuestionLibraryScriptableObject questionLibrary;
    void Start()
    {
        DefaultPlantBackend backend = new DefaultPlantBackend(plantLibrary, questionLibrary);
        DontDestroyOnLoad(this.gameObject);
    }
    
}
