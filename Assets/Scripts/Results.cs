using UnityEngine;

public class Results : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.Backend.EndSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}