using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class FallingBackend : IPlantBackend
{
    public QuestionSet GetQuestion()
    {
        QuestionSet output = new QuestionSet();
        //Populate output with components
        return output;
    }

    public bool AttemptQuestion(AnswerHandle handle)
    {
        throw new NotImplementedException();
    }
}


public class ForagingController : MonoBehaviour
{
    private FallingBackend backend;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private void GenerateSprites(List<Answer> possibleAnswers )
    {
        throw new NotImplementedException();
    }

    private void LoadQuestion(string Question)
    {
        throw new NotImplementedException();
    }

    private void Start()
    {   
        QuestionSet Question = backend.GetQuestion();
        LoadQuestion(Question.DisplayQuestion);
        GenerateSprites(Question.PossibleAnswers);
    }

    private void OnMouseUpAsButton()
    {
        //If Click on sprite, create answer handle.
        AnswerHandle attempt = new AnswerHandle();
        bool outcome = backend.AttemptQuestion(attempt);
    }
}