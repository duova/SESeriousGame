using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
public class QuestionEntry
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

    public QuestionEntry Clone()
    {
        var retVal = new QuestionEntry();
        retVal.uniqueName = uniqueName;
        retVal.question = question;
        retVal.questionType = questionType;
        retVal.questionRequirements = questionRequirements;
        retVal.wrongAnswers = wrongAnswers;
        retVal.correctAnswers = correctAnswers;
        retVal.numberOfWrongAnswersToInclude = numberOfWrongAnswersToInclude;
        retVal.numberOfRightAnswersToInclude = numberOfRightAnswersToInclude;
        retVal.acceptAllEdible = acceptAllEdible;
        return retVal;
    }
}

[Serializable]
public struct AnswerEntry
{
    //If true this answer is a feature and not a plant type.
    public bool isFeature;

    //Must be typed exactly.
    public string uniqueName;

    //Cache index on question system initialization so that we're not doing a search every question.
    [HideInInspector] public int typeIndex;

    //Cache index on question system initialization so that we're not doing a search every question.
    //Probably should use -1 for unused as it might not be a feature.
    [HideInInspector] public int featureIndex;
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

    public QuestionSystem(in PlantLibraryScriptableObject plantLibrary,
        in QuestionLibraryScriptableObject questionLibrary)
    {
        //Copy structs from libraries.
        foreach (QuestionEntry entry in questionLibrary.questionEntries)
        {
            _questionEntries.Add(entry.Clone());
        }

        _plantEntries = plantLibrary.plantEntries;

        //Search unique names and cache indexes for questions, add edible/non-edible plants accordingly to the struct.

        //Cache all edible if necessary.
        foreach (var question in _questionEntries)
        {
            if (!question.acceptAllEdible) continue;
            var featuresToAdd = _plantEntries.SelectMany(plantType => plantType.features)
                .Where(feature => feature.isEdible).ToList();
            foreach (var feature in featuresToAdd)
            {
                AnswerEntry entry = default;
                entry.isFeature = true;
                entry.uniqueName = feature.uniqueName;
                question.correctAnswers.Add(entry);
            }
        }

        //Cache answer references.
        foreach (var entry in _questionEntries)
        {
            for (int i = 0; i < entry.correctAnswers.Count; i++)
            {
                if (entry.correctAnswers[i].isFeature)
                {
                    var itAnswer = entry.correctAnswers[i];
                    itAnswer.typeIndex = GetIndexOfPlantType(itAnswer.uniqueName);
                    itAnswer.featureIndex = GetIndexOfPlantFeature(itAnswer.uniqueName);
                    entry.correctAnswers[i] = itAnswer;
                }
                else
                {
                    var itAnswer = entry.correctAnswers[i];
                    itAnswer.typeIndex = GetIndexOfPlantType(itAnswer.uniqueName);
                    itAnswer.featureIndex = -1;
                    entry.correctAnswers[i] = itAnswer;
                }
            }

            for (int i = 0; i < entry.wrongAnswers.Count; i++)
            {
                if (entry.wrongAnswers[i].isFeature)
                {
                    var itAnswer = entry.wrongAnswers[i];
                    itAnswer.typeIndex = GetIndexOfPlantType(itAnswer.uniqueName);
                    itAnswer.featureIndex = GetIndexOfPlantFeature(itAnswer.uniqueName);
                    entry.wrongAnswers[i] = itAnswer;
                }
                else
                {
                    var itAnswer = entry.wrongAnswers[i];
                    itAnswer.typeIndex = GetIndexOfPlantType(itAnswer.uniqueName);
                    itAnswer.featureIndex = -1;
                    entry.wrongAnswers[i] = itAnswer;
                }
            }
        }

        //Throw errors if the plant cannot be found.
    }

    public InstancedQuestion GetQuestion(PlantTypeLevelData levelData, QuestionType questionType,
        bool specificQuestion = false, string questionUniqueName = "")
    {
        //Return a InstancedQuestion with a question randomly selected of the question type. Or if specifiedQuestion is true
        //search for and return the specified question. Question requirements must be checked against levelData.
        //Random.Range(0, _plantEntries.Count);
        //Remember to make sure all AnswerEntries can actually be used like if the plant of an answer exceeds the level of the plant
        //it should not be a possible answer.=>feature level < leveldata
        //Set current question to the question, replacing one if one already exists.
        //Throw errors if the question cannot be found.

        InstancedQuestion retVal = default;
        QuestionEntry selectQuestion = new QuestionEntry();

        if (specificQuestion)
        {
            // search for and return the specified question
            for (int i = 0; i < _questionEntries.Count; i++)
            {
                if (_questionEntries[i].uniqueName == questionUniqueName)
                {
                    retVal.Question = _questionEntries[i].question;
                    selectQuestion = _questionEntries[i];
                    _currentQuestion = i;
                }
                else
                {
                    throw new ApplicationException("question unique name does not exist.");
                }
            }
        }
        else
        {
            // question randomly selected of the question type
            for (int i = 0; i < _questionEntries.Count; i++)
            {
                QuestionEntry randomQuestionEntry = GetRandomQuestionEntry();
                if (randomQuestionEntry.questionType == questionType)
                {
                    retVal.Question = randomQuestionEntry.question;
                    selectQuestion = randomQuestionEntry;
                    _currentQuestion = i;
                    break;
                }
            }
        }

        foreach (var answer in selectQuestion.correctAnswers)
        {
            if (answer.isFeature)
            {
                Debug.Log(answer.typeIndex);
                if (answer.featureIndex < levelData.Levels[answer.typeIndex])
                {
                    retVal.PossiblePlantFeatureAnswers
                        .Add(_plantEntries[answer.typeIndex].features[answer.featureIndex]);
                }
            }
            else
            {
                retVal.PossiblePlantTypeAnswers.Add(_plantEntries[answer.typeIndex]);
            }
        }

        // retVal.PossiblePlantFeatureAnswers = new List<PlantFeature>();
        // retVal.PossiblePlantTypeAnswers = new List<PlantTypeEntry>();
        return retVal;
    }

    private QuestionEntry GetRandomQuestionEntry()
    {
        return _questionEntries[Random.Range(0, _questionEntries.Count)];
    }

    public bool AttemptQuestion(string answerUniqueName)
    {
        //Check if the answer is correct and return the value.
        QuestionEntry questionEntry = _questionEntries[_currentQuestion];
        foreach (var correctAnswer in questionEntry.correctAnswers)
        {
            if (correctAnswer.uniqueName == answerUniqueName)
            {
                return true;
            }
        }

        return false;
    }

    private int GetIndexOfPlantType(string uniqueName)
    {
        var index = _plantEntries.FindIndex(plant => plant.uniqueName == uniqueName);
        if (index < 0)
        {
            throw new Exception("Plant type can't be found.");
        }
        return index;
    }

    private int GetIndexOfPlantFeature(string uniqueName)
    {
        for (int i = 0; i < _plantEntries.Count; i++)
        {
            for (int j = 0; j < _plantEntries[i].features.Count; j++)
            {
                if (_plantEntries[i].features[j].uniqueName == uniqueName)
                {
                    return j;
                }
            }
        }

        throw new Exception("UniqueName of plant feature does not exist.");
    }
}