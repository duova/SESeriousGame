using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AnswerEntry
{
    public string displayText;

    public Sprite sprite;

    public AnswerDisplayType answerDisplayType;

    public bool isCorrect;
    
    //Transient.
    public AnswerHandle Handle;
}

[Serializable]
public class MockQuestionEntry
{
    public string displayQuestion;

    public List<AnswerEntry> answerEntries;
    
    //Transient.
    public QuestionHandle Handle;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MockQuestionLibrary")]
public class MockQuestionLibraryScriptableObject : ScriptableObject
{
    public List<MockQuestionEntry> questionEntries;
}

public class MockBackend : IPlantBackend
{
    private List<MockQuestionEntry> _questions;

    private int _questionIterator;
    
    public MockBackend(MockQuestionLibraryScriptableObject library)
    {
        _questions = library.questionEntries;
        //Generate handles we can use to reference the questions and answers.
        foreach (var question in _questions)
        {
            question.Handle = new QuestionHandle();
            foreach (var answer in question.answerEntries)
            {
                answer.Handle = new AnswerHandle();
            }
        }
    }
    
    public QuestionSet GetQuestion()
    {
        if (_questions.Count >= _questionIterator) _questionIterator = 0;
        
        var retVal = new QuestionSet();
        retVal.DisplayQuestion = _questions[_questionIterator].displayQuestion;
        retVal.Handle = _questions[_questionIterator].Handle;
        foreach (var answerEntry in _questions[_questionIterator].answerEntries)
        {
            var possibleAnswer = new Answer();
            possibleAnswer.Handle = answerEntry.Handle;
            possibleAnswer.AnswerDisplayType = answerEntry.answerDisplayType;
            possibleAnswer.Sprite = answerEntry.sprite;
            possibleAnswer.DisplayText = answerEntry.displayText;
            retVal.PossibleAnswers.Add(possibleAnswer);
        }
        
        _questionIterator++;

        return retVal;
    }

    public bool AttemptQuestion(AnswerHandle handle)
    {
        if (!_questions.Any(question => question.answerEntries.Any(answer => answer.Handle == handle))) throw new Exception("Could not find answer by handle.");

        return _questions.SelectMany(question => question.answerEntries).First(answer => answer.Handle == handle)
            .isCorrect;
    }
}