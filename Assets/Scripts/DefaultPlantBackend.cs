using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlantData
{
    public int Stage = 1;
    public HashSet<char> CompletedSyllabus = new HashSet<char>();
}

public class DefaultPlantBackend : IPlantBackend
{
    private PlantLibraryScriptableObject _plantLibrary;

    private QuestionLibraryScriptableObject _questionLibrary;

    private List<PlantData> _plantDatas = new List<PlantData>();

    private List<QuestionSet> _createdQuestionSets = new List<QuestionSet>();

    private List<QuestionEntry> _createdQuestionEntries = new List<QuestionEntry>();

    public DefaultPlantBackend(PlantLibraryScriptableObject plantLibrary,
        QuestionLibraryScriptableObject questionLibrary)
    {
        _plantLibrary = plantLibrary;
        _questionLibrary = questionLibrary;
        foreach (var plantEntries in _plantLibrary.plantEntries)
        {
            _plantDatas.Add(new PlantData());
        }
    }

    public int GetPlantIndex(PlantEntryScriptableObject entry)
    {
        return _plantLibrary.plantEntries.FindIndex(item => item == entry);
    }

    public QuestionSet GetQuestion(QuestionLocation questionLocation)
    {
        List<QuestionEntry> questions = new List<QuestionEntry>();
        foreach (var question in _questionLibrary.questionEntries)
        {
            if (_plantDatas[GetPlantIndex(question.plant)].Stage != question.stage) continue;

            if (_plantDatas[GetPlantIndex(question.plant)].CompletedSyllabus
                .Contains(question.syllabusReference)) continue;

            // add to list
            questions.Add(question);
        }

        // if the list has no question for a particular plant, increase the stage of plant
        if (questions.Count == 0)
        {
            HandleIncreasePlantLevel();
        }

        // random choose a question from list
        QuestionEntry randomQuestion = questions[Random.Range(0, questions.Count)];
        QuestionSet questionSet = new();
        questionSet.DisplayQuestion = FillQuestionString(randomQuestion);

        // get possible answers (right & wrong)
        List<Answer> possibleAnswers = new List<Answer>();
        possibleAnswers.Add(ConvertPlantFeatureToAnswer(randomQuestion.feature, true, randomQuestion));
        List<PlantFeatureScriptableObject> wrongPlantFeatures = GetWrongAnswers(randomQuestion);
        foreach (var feature in wrongPlantFeatures)
        {
            possibleAnswers.Add(ConvertPlantFeatureToAnswer(feature, false, randomQuestion));
        }

        questionSet.PossibleAnswers = possibleAnswers;

        _createdQuestionSets.Add(questionSet);
        _createdQuestionEntries.Add(randomQuestion);
        return questionSet;
    }

    private Answer ConvertPlantFeatureToAnswer(PlantFeatureScriptableObject plantFeature, bool isCorrect,
        QuestionEntry questionEntry)
    {
        Answer answer = new Answer();
        answer.IsCorrect = isCorrect;
        answer.DisplayText = plantFeature.displayName;
        if (questionEntry.useSpecificImage)
        {
            answer.Sprite = plantFeature.sprites[questionEntry.specificImageIndex];
        }
        else
        {
            answer.Sprite = plantFeature.sprites[Random.Range(0, plantFeature.sprites.Count)];
        }

        // TODO: Check AnswerDisplayType with Laura later
        answer.AnswerDisplayType = AnswerDisplayType.Sprite;

        return answer;
    }

    private List<PlantFeatureScriptableObject> GetWrongAnswers(QuestionEntry randomQuestion)
    {
        List<PlantFeatureScriptableObject> wrongAnswers = new List<PlantFeatureScriptableObject>();
        foreach (var plant in _plantLibrary.plantEntries)
        {
            foreach (var plantFeature in plant.features)
            {
                if (plantFeature.featureType == randomQuestion.feature.featureType)
                {
                    wrongAnswers.Add(plantFeature);
                }
            }
        }

        return wrongAnswers;
    }

    private string FillQuestionString(QuestionEntry question)
    {
        // Use [H] for hint, [P] for plant, [O] for feature/object with the square brackets.
        var baseQuestion = question.questionType.question;
        baseQuestion = baseQuestion.Replace("[H]", question.hint);
        baseQuestion = baseQuestion.Replace("[P]", question.plant.displayName);
        string feature;
        if (question.questionType.useFeaturePluralName)
        {
            feature = Random.Range(0f, 0.99f) < question.chanceToUseAlternateName
                ? question.feature.alternateDisplayNamePlural
                : question.feature.displayNamePlural;
        }
        else
        {
            feature = Random.Range(0f, 0.99f) < question.chanceToUseAlternateName
                ? question.feature.alternateDisplayName
                : question.feature.displayName;
        }

        baseQuestion = baseQuestion.Replace("[O]", feature);

        return baseQuestion;
    }

    private void HandleIncreasePlantLevel()
    {
        throw new System.NotImplementedException();
    }

    public bool AttemptQuestion(AnswerHandle handle)
    {
        for (int i = 0; i < _createdQuestionSets.Count; i++)
        {
            // find answer base on answerHandle
            foreach (var possibleAnswer in _createdQuestionSets[i].PossibleAnswers)
            {
                if (handle.Key != possibleAnswer.Handle.Key) continue;
                if (possibleAnswer.IsCorrect)
                {
                    // if correct add complete syllabus
                    _plantDatas[GetPlantIndex(_createdQuestionEntries[i].plant)].CompletedSyllabus
                        .Add(_createdQuestionEntries[i].syllabusReference);
                    return true;
                }
                return false;
            }
        }
        throw new Exception("Could not find answer handle");
    }
    
    // TODO: Clear the cache when changing scenes
    public void ClearQuestionCache()
    {
        _createdQuestionSets.Clear();
        _createdQuestionEntries.Clear();
    }

    public int GetStage(PlantEntryScriptableObject plant)
    {
        return _plantDatas[GetPlantIndex(plant)].Stage;
    }
}