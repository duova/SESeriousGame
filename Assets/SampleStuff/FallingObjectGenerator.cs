using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private float interval;

    [SerializeField]
    private float spawnAreaWidth;

    [SerializeField]
    private GameObject prefabToSpawn;
    
    private float _intervalTimer;

    public IPlantBackend Backend; //Insert reference to global backend.

    private QuestionSet _currentQuestion = null;

    private int _uninstantiatedAnswers;

    public List<FallingObject> instantiatedAnswers = new List<FallingObject>();
    
    [SerializeField]
    private TMP_Text questionTextBox;

    void Start()
    {
        Backend = Persistance.instance.backend;
    }

    void Update()
    {
        //Get a question if there isn't an active question.
        if (_currentQuestion == null)
        {
            _currentQuestion = Backend.GetQuestion(QuestionLocation.Falling);
            _uninstantiatedAnswers = _currentQuestion.PossibleAnswers.Count;
            questionTextBox.text = _currentQuestion.DisplayQuestion;
            _intervalTimer = interval;
        }
        
        //Instantiate answers by interval until there are no more uninstantiated answers.
        if (_uninstantiatedAnswers > 0)
        {
            _intervalTimer += Time.deltaTime;
            if (_intervalTimer > interval)
            {
                _intervalTimer = 0;

                float randomizedX = transform.position.x + Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                Vector3 spawnLocation = new Vector3(randomizedX, transform.position.y, 0);
                GameObject fallingObject = Instantiate(prefabToSpawn, spawnLocation, Quaternion.identity);
                FallingObject fallingObjectComp = fallingObject.GetComponent<FallingObject>();
                fallingObjectComp.Setup(_currentQuestion.PossibleAnswers[_uninstantiatedAnswers - 1]);
                instantiatedAnswers.Add(fallingObjectComp);
                fallingObjectComp.generator = this;

                _uninstantiatedAnswers--;
            }
        }

        //Reset question when there are no more answers.
        if (instantiatedAnswers.Count == 0)
        {
            _currentQuestion = null;
        }
    }
}