using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum AnswerDisplayType
{
    Sprite,
    TextAndSprite,
    Text
}

public class AnswerHandle
{
    public GUID Key { get; } = GUID.Generate();
}

public class QuestionHandle
{
    public GUID Key { get; } = GUID.Generate();
}

public class Answer
{
    public AnswerHandle Handle = new AnswerHandle();

    public bool IsCorrect;

    public string DisplayText;

    public Sprite Sprite;

    public AnswerDisplayType AnswerDisplayType;
}

public class QuestionSet
{
    public QuestionHandle Handle = new QuestionHandle();
    
    public string DisplayQuestion;
    
    public List<Answer> PossibleAnswers = new List<Answer>();
}

public interface IPlantBackend
{
    public QuestionSet GetQuestion(QuestionLocation questionLocation);

    public bool AttemptQuestion(AnswerHandle handle);

    public int GetStage(PlantEntryScriptableObject plant);
}