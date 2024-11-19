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
    
    public static bool operator ==(AnswerHandle a, AnswerHandle b)
    {
        if (a == null || b == null) throw new NullReferenceException();
        return a.Key == b.Key;
    }

    public static bool operator !=(AnswerHandle a, AnswerHandle b)
    {
        return !(a == b);
    }
}

public class QuestionHandle
{
    public GUID Key { get; } = GUID.Generate();

    public static bool operator ==(QuestionHandle a, QuestionHandle b)
    {
        if (a == null || b == null) throw new NullReferenceException();
        return a.Key == b.Key;
    }

    public static bool operator !=(QuestionHandle a, QuestionHandle b)
    {
        return !(a == b);
    }
}

public class Answer
{
    public AnswerHandle Handle = new AnswerHandle();

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
    public QuestionSet GetQuestion();

    public bool AttemptQuestion(AnswerHandle handle);
}