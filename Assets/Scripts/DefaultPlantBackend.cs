using System.Collections.Generic;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantData
{
    public int Stage = 1;
    public Dictionary<char, SyllabusData> Syllabus = new Dictionary<char, SyllabusData>();
    public bool PendingStageIncrease = false;
}

public class SyllabusData
{
    public int Streak = 0;
    public int CorrectAnswers = 0;
}

public class DefaultPlantBackend : IPlantBackend
{
    private PlantLibraryScriptableObject _plantLibrary;

    private QuestionLibraryScriptableObject _questionLibrary;

    private List<PlantData> _plantDatas = new List<PlantData>();

    private List<QuestionSet> _createdQuestionSets = new List<QuestionSet>();

    private List<QuestionEntry> _createdQuestionEntries = new List<QuestionEntry>();

    private List<StageIncrease> _mostRecentStageIncreases = new List<StageIncrease>();

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

    public void SetPlantStage(PlantEntryScriptableObject plant, int stage)
    {
        _plantDatas[GetPlantIndex(plant)].Stage = stage;
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
            if (question.environment != GameManager.Instance.environment) continue;
            
            // Debug.Log("_plantLibrary: " + _plantLibrary.plantEntries.Count); // 1
            // Debug.Log("_plantDatas: " + _plantDatas.Count);
            // Debug.Log("GetPlantIndex: " + GetPlantIndex(question.plant)); // -1
            // Debug.Log("_plantLibrary[0]: " + _plantLibrary.plantEntries[0]); // Oak
            // Debug.Log("question.plant: " + question.plant); // SilverBirch
            if (GetPlantIndex(question.plant) == -1 || _plantDatas[GetPlantIndex(question.plant)].Stage != question.stage) continue;

            Dictionary<char, SyllabusData> syllabus = _plantDatas[GetPlantIndex(question.plant)].Syllabus;
            if (syllabus.ContainsKey(question.syllabusReference) &&
                (syllabus[question.syllabusReference].Streak >= 2 ||
                 syllabus[question.syllabusReference].CorrectAnswers >= 3)) continue;

            // add to list
            questions.Add(question);
        }

        questions.RemoveAll(question => question.feature.sprites.Count <= 0);

        // if the list has no question for a particular plant, increase the stage of plant
        foreach (var plantEntry in _plantLibrary.plantEntries)
        {
            if (questions.All(question => question.plant != plantEntry))
            {
                HandleIncreasePlantLevel(plantEntry);
            }
        }

        // random choose a question from list
        QuestionEntry randomQuestion = questions[Random.Range(0, questions.Count)];
        QuestionSet questionSet = new();

        PlantData plantData = _plantDatas[GetPlantIndex(randomQuestion.plant)];
        bool includeHint = plantData.Syllabus.ContainsKey(randomQuestion.syllabusReference);
        questionSet.DisplayQuestion = FillQuestionString(randomQuestion, includeHint);

        // get possible answers (right & wrong)
        List<Answer> possibleAnswers = new List<Answer>();
        possibleAnswers.Add(ConvertPlantFeatureToAnswer(randomQuestion.feature, true, randomQuestion));
        List<PlantFeatureScriptableObject> wrongPlantFeatures = GetWrongAnswers(randomQuestion);
        foreach (var feature in wrongPlantFeatures)
        {
            possibleAnswers.Add(ConvertPlantFeatureToAnswer(feature, false, randomQuestion));
        }

        //Shuffle.
        possibleAnswers = possibleAnswers.OrderBy(_ => Random.Range(0, 1f)).ToList();
        
        questionSet.PossibleAnswers = possibleAnswers;

        _createdQuestionSets.Add(questionSet);
        _createdQuestionEntries.Add(randomQuestion);
        if (!_plantDatas[GetPlantIndex(randomQuestion.plant)].Syllabus.ContainsKey(randomQuestion.syllabusReference))
        {
            _plantDatas[GetPlantIndex(randomQuestion.plant)].Syllabus
                .Add(randomQuestion.syllabusReference, new SyllabusData());
        }
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
                if (plantFeature.sprites.Count <= 0) continue;
                if (plantFeature.featureType == randomQuestion.feature.featureType && plantFeature != randomQuestion.feature)
                {
                    wrongAnswers.Add(plantFeature);
                }
            }
        }

        return wrongAnswers;
    }

    private string FillQuestionString(QuestionEntry question, bool includeHint)
    {
        // Use [H] for hint, [P] for plant, [O] for feature/object with the square brackets.
        var baseQuestion = question.questionType.question;
        baseQuestion = baseQuestion.Replace(" [H]", includeHint ? " " + question.hint : "");
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

    private void HandleIncreasePlantLevel(PlantEntryScriptableObject plantEntry)
    {
        // Reset & add pending level to increase
        _plantDatas[GetPlantIndex(plantEntry)].PendingStageIncrease = true;
        foreach (var syllabus in _plantDatas[GetPlantIndex(plantEntry)].Syllabus)
        {
            syllabus.Value.CorrectAnswers = 0;
            syllabus.Value.Streak = 0;
        }
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
                    // if correct add syllabus streak+1 and correctAnswer+1
                    _plantDatas[GetPlantIndex(_createdQuestionEntries[i].plant)].Syllabus
                        [_createdQuestionEntries[i].syllabusReference].Streak += 1;
                    _plantDatas[GetPlantIndex(_createdQuestionEntries[i].plant)].Syllabus
                        [_createdQuestionEntries[i].syllabusReference].CorrectAnswers += 1;
                    return true;
                }

                _plantDatas[GetPlantIndex(_createdQuestionEntries[i].plant)].Syllabus
                    [_createdQuestionEntries[i].syllabusReference].Streak = 0;
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

    public List<StageIncrease> EndSession()
    {
        List<StageIncrease> stageIncreases = new List<StageIncrease>();
        for (int i=0; i<_plantDatas.Count; i++)
        {
            PlantData plantData = _plantDatas[i];
            if (plantData.PendingStageIncrease)
            {
                plantData.Stage++;
                plantData.PendingStageIncrease = false;
                plantData.Syllabus.Clear();
                
                StageIncrease stageIncrease = new StageIncrease
                {
                    Plant = _plantLibrary.plantEntries[i],
                    NewStage = plantData.Stage
                };
                stageIncreases.Add(stageIncrease);
            }
        }

        _mostRecentStageIncreases = stageIncreases;
        return stageIncreases;
    }

    public List<StageIncrease> GetMostRecentStageIncreases()
    {
        return _mostRecentStageIncreases;
    }
}