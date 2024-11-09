using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum QuestionType
{
    Foraging,
    Falling,
    Check
}

[Serializable]
public struct QuestionRequirement
{
    public string plantTypeUniqueName;

    public int plantTypeMinimumLevel;
}

[Serializable]
public struct QuestionEntry
{
    public string uniqueName;
    
    public string question;

    public QuestionType questionType;

    public List<QuestionRequirement> questionRequirements;

    public List<AnswerEntry> wrongAnswers;

    public List<AnswerEntry> correctAnswers;

    public int numberOfWrongAnswersToInclude;

    public int numberOfRightAnswersToInclude;

    //Adds all edible plant features to correct answers. Cache this at construction.
    public bool acceptAllEdible;

    //Adds all non-edible plant features to correct answers. Cache this at construction.
    public bool acceptAllNonEdible;
}

[Serializable]
public struct AnswerEntry
{
    //If true this answer is a feature and not a plant type.
    public bool isFeature;
    
    //Must be typed exactly.
    public string uniqueName;
    
    //Cache index on question system initialization so that we're not doing a search every question.
    [HideInInspector]
    public int typeIndex;
    
    //Cache index on question system initialization so that we're not doing a search every question.
    //Probably should use -1 for unused as it might not be a feature.
    [HideInInspector]
    public int featureIndex;
}

public struct InstancedQuestion
{
    public string Question;

    public List<PlantTypeEntry> PossiblePlantTypeAnswers;

    public List<PlantFeature> PossiblePlantFeatureAnswers;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestionLibrary")]
public class QuestionLibraryScriptableObject : ScriptableObject
{
    public List<QuestionEntry> questionEntries;
}

public class QuestionSystem
{
    private List<PlantTypeEntry> _plantEntries = new List<PlantTypeEntry>();
    
    private List<QuestionEntry> _questionEntries = new List<QuestionEntry>();

    private int _currentQuestion;
    
    public QuestionSystem(in PlantLibraryScriptableObject plantLibrary, in QuestionLibraryScriptableObject questionLibrary)
    {
        //Copy structs from libraries.
        //Search unique names and cache indexes for questions, add edible/non-edible plants accordingly to the struct.
        //Throw errors if the plant cannot be found.
    }

    public InstancedQuestion GetQuestion(PlantTypeLevelData levelData, QuestionType questionType, bool specificQuestion = false, string questionUniqueName = "")
    {
        //Return a InstancedQuestion with a question randomly selected of the question type. Or if specifiedQuestion is true
        //search for and return the specified question. Question requirements must be checked against levelData.
        //Remember to make sure all AnswerEntries can actually be used like if the plant of an answer exceeds the level of the plant
        //it should not be a possible answer.
        //Set current question to the question, replacing one if one already exists.
        //Throw errors if the question cannot be found.
        InstancedQuestion retVal = default;
        retVal.PossiblePlantFeatureAnswers = new List<PlantFeature>();
        retVal.PossiblePlantTypeAnswers = new List<PlantTypeEntry>();
        return retVal;
    }

    public bool AttemptQuestion(string answerUniqueName)
    {
        //Check if the answer is correct and return the value.
        return false;
    }
}
