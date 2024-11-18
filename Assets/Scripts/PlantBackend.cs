using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AnswerHandle
{
    public GUID Key { get; } = GUID.Generate();

    //== override.
}

public class QuestionHandle
{
    public GUID Key { get; } = GUID.Generate();

    //== override.
}

public class Answer
{
    public AnswerHandle Handle = new AnswerHandle();

    public string DisplayName;

    public Sprite Sprite;
}

public class QuestionSet
{
    public QuestionHandle Handle = new QuestionHandle();
    
    public string DisplayQuestion;
    
    public List<Answer> PossibleAnswers = new List<Answer>();
}

public interface IPlantBackend
{
    public QuestionSet GetQuestion();

    public bool AttemptQuestion(AnswerHandle handle);
}