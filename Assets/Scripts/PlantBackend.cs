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
    public Guid Key { get; } = Guid.NewGuid();
}

public class QuestionHandle
{
    public Guid Key { get; } = Guid.NewGuid();
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

public class StageIncrease
{
    public PlantEntryScriptableObject Plant;

    public int NewStage;
}

public interface IPlantBackend
{
    public QuestionSet GetQuestion(QuestionLocation questionLocation);

    public bool AttemptQuestion(AnswerHandle handle);

    public int GetStage(PlantEntryScriptableObject plant);

    public List<StageIncrease> EndSession();
}