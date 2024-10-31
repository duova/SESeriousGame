using UnityEngine;

public class FallingObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private float interval;

    [SerializeField]
    private float spawnAreaWidth;

    [SerializeField]
    private GameObject prefabToSpawn;

    //Note that c# zero-initializes by default.
    private float _intervalTimer;

    // Update is called once per frame
    void Update()
    {
        _intervalTimer += Time.deltaTime;

        if (_intervalTimer > interval)
        {
            //Reset timer.
            _intervalTimer = 0;

            float randomizedX = transform.position.x + Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            Vector3 spawnLocation = new Vector3(randomizedX, transform.position.y, 0);
            GameObject FallingObject = Instantiate(prefabToSpawn, spawnLocation, Quaternion.identity);
            FallingObject.GetComponent<FallingObject>().Setup(Random.Range(0, 2) != 0);
        }
    }
}
