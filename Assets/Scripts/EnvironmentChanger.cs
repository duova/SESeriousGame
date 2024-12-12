using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct EnvQuestion
{
    public int environment;

    public string question;

    public string correctAnswer;

    public List<string> wrongAnswers;
}

/*
We want to take the plant features that they have access to and populate the environment with those images.
If they consume non-edible plants the words the *plant feature is not edible! and will decrease their energy.
If it is edible a simple question will ask them what the plant feature is with a combination of plant name concat feature name.
*/
public class EnvironmentChanger : MonoBehaviour
{
    private PlantLibraryScriptableObject _plantLib;

    private DefaultPlantBackend _backend;

    [SerializeField]
    private int stageForNewEnvironment;

    [SerializeField]
    private int lastEnvironment;

    [SerializeField]
    private GameObject introductoryObject;
    
    [SerializeField]
    private GameObject questionObject;

    [SerializeField]
    private GameObject newEnvironmentObject;

    [SerializeField]
    private GameObject correctAnswerObject;
    
    [SerializeField]
    private GameObject failedObject;

    [SerializeField]
    private TMP_Text questionText;
    
    [SerializeField]
    private List<TMP_Text> questionAnswerTexts = new List<TMP_Text>();

    [SerializeField]
    private List<EnvQuestion> questions = new List<EnvQuestion>();

    private int _correctAnswerText;

    private List<EnvQuestion> _queuedQuestions = new List<EnvQuestion>();

    private void Start()
    {
        _plantLib = GameManager.Instance.plantLibrary;
        _backend = GameManager.Instance.Backend;
        
        CloseAllPages();
        
        if (_plantLib.plantEntries.Where(plant => plant.environment == GameManager.Instance.environment)
            .All(plant => _backend.GetStage(plant) >= stageForNewEnvironment))
        {
            _queuedQuestions.AddRange(questions.Where(question => question.environment == GameManager.Instance.environment));

            if (_queuedQuestions.Count <= 0)
            {
                //End game if there are no questions and therefore likely no environment.
                GameManager.Instance.EndGame();
            }
            
            introductoryObject.SetActive(true);
        }
    }

    public void StartQuestions()
    {
        introductoryObject.SetActive(false);
        NextQuestion();
    }

    public void NextQuestion()
    {
        questionObject.SetActive(true);

        var currentQuestion = _queuedQuestions.First();
        
        questionText.text = currentQuestion.question;

        _correctAnswerText = Random.Range(0, questionAnswerTexts.Count);

        for (int i = 0; i < questionAnswerTexts.Count; i++)
        {
            if (_correctAnswerText == i)
            {
                questionAnswerTexts[i].text = currentQuestion.correctAnswer;
            }
            else
            {
                questionAnswerTexts[i].text = currentQuestion.wrongAnswers.FirstOrDefault();
                currentQuestion.wrongAnswers.RemoveAt(0);
            }
        }
        
        _queuedQuestions.RemoveAt(0);
    }

    public void AnswerQuestion(int index)
    {
        questionObject.SetActive(false);
        
        if (index == _correctAnswerText)
        {
            if (_queuedQuestions.Count > 0)
            {
                correctAnswerObject.SetActive(true);
            }
            else
            {
                newEnvironmentObject.SetActive(true);
                GameManager.Instance.environment++;
                if (GameManager.Instance.environment > lastEnvironment)
                {
                    GameManager.Instance.EndGame();
                }
            }
        }
        else
        {
            failedObject.SetActive(true);
            foreach (var plant in _plantLib.plantEntries.Where(plant =>
                         plant.environment == GameManager.Instance.environment))
            {
                _backend.SetPlantStage(plant, stageForNewEnvironment - 1);
            }
        }
    }

    public void CloseAllPages()
    {
        introductoryObject.SetActive(false);
        questionObject.SetActive(false);
        newEnvironmentObject.SetActive(false);
        correctAnswerObject.SetActive(false);
        failedObject.SetActive(false);
    }
}