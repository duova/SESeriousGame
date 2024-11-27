using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ForagingController : MonoBehaviour
{
    public Text Question;
    private QuestionSet _currentQuestion;

    public IPlantBackend backend;
    public EnergyLevel energy;

    [SerializeField]
    private MockQuestionLibraryScriptableObject questionLibrary;
    
    private void LoadQuestion(string retrievedQuestion)
    {
        Question.text = retrievedQuestion;
    }

    private void Start()
    {

        LoadQuestion(_currentQuestion.DisplayQuestion);
        //GenerateSprites(_currentQuestion.PossibleAnswers);
    }

    private void OnMouseUpAsButton()
    {
        

    }
}