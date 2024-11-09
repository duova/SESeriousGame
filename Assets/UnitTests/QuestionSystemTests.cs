using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class QuestionSystemTests
{
    [Test]
    public void QuestionSystemTestsSimplePasses()
    {
        //Create a simple mock plant library.
        PlantLibraryScriptableObject mockPlantLib = ScriptableObject.CreateInstance<PlantLibraryScriptableObject>();
        
        //Add oak.
        PlantTypeEntry oakPlant = default;
        oakPlant.uniqueName = "Oak";
        oakPlant.features = new List<PlantFeature>();
        PlantFeature oakLeaf = default;
        oakLeaf.uniqueName = "OakLeaf";
        oakLeaf.isEdible = true;
        PlantFeature oakSeed = default;
        oakSeed.uniqueName = "OakSeed";
        oakSeed.isEdible = false;
        PlantFeature oakTwig = default;
        oakTwig.uniqueName = "OakTwig";
        oakTwig.isEdible = false;
        oakPlant.features.Add(oakLeaf);
        oakPlant.features.Add(oakSeed);
        oakPlant.features.Add(oakTwig);
        mockPlantLib.plantEntries = new List<PlantTypeEntry>();
        mockPlantLib.plantEntries.Add(oakPlant);
        
        //Add strawberry.
        PlantTypeEntry strawberryPlant = new PlantTypeEntry();
        strawberryPlant.uniqueName = "Strawberry";
        PlantFeature strawberryFruit = new PlantFeature();
        strawberryFruit.uniqueName = "StrawberryFruit";
        strawberryFruit.isEdible = true;

        //Create a simple mock question library.
        QuestionLibraryScriptableObject mockQuestionLib = ScriptableObject.CreateInstance<QuestionLibraryScriptableObject>();

        QuestionEntry question = default;
        question.questionType = QuestionType.Foraging;
        //Test accept all edible as well.
        question.acceptAllEdible = true;
        question.uniqueName = "TestQuestion";
        AnswerEntry OakLeafAnswer = default;
        OakLeafAnswer.uniqueName = "OakLeaf";
        OakLeafAnswer.isFeature = true;
        AnswerEntry OakTwigAnswer = default;
        OakTwigAnswer.uniqueName = "OakTwig";
        OakTwigAnswer.isFeature = true;
        AnswerEntry OakSeedAnswer = default;
        OakSeedAnswer.uniqueName = "OakSeed";
        OakSeedAnswer.isFeature = true;
        
        question.correctAnswers = new List<AnswerEntry>();
        question.correctAnswers.Add(OakLeafAnswer);
        question.wrongAnswers = new List<AnswerEntry>();
        question.wrongAnswers.Add(OakTwigAnswer);
        question.wrongAnswers.Add(OakSeedAnswer);
        
        //In theory there should be 2 right answers because strawberries should be added with the accept all edible option.
        question.numberOfRightAnswersToInclude = 2;
        question.numberOfWrongAnswersToInclude = 2;

        mockQuestionLib.questionEntries = new List<QuestionEntry>();
        mockQuestionLib.questionEntries.Add(question);

        PlantTypeLevelData lowLevelData = default;
        lowLevelData.Levels = new List<int> { 1, 1 };
        
        PlantTypeLevelData highLevelData = default;
        lowLevelData.Levels = new List<int> { 3, 1 };

        QuestionSystem questionSystem = new QuestionSystem(mockPlantLib, mockQuestionLib);

        InstancedQuestion instancedQuestion = questionSystem.GetQuestion(lowLevelData, QuestionType.Foraging, true, "TestQuestion");

        Assert.True(instancedQuestion.PossiblePlantFeatureAnswers.Count == 2, "Check that levels limit answers.");

        Assert.True(questionSystem.AttemptQuestion("OakLeaf"), "Check that answering correctly will return from AttemptQuestion.");
        
        Assert.True(questionSystem.AttemptQuestion("StrawberryFruit"), "Check that the add edible option works.");

        InstancedQuestion nextInstancedQuestion =
            questionSystem.GetQuestion(highLevelData, QuestionType.Foraging, true, "TestQuestion");
        
        Assert.True(nextInstancedQuestion.PossiblePlantFeatureAnswers.Count == 4, "Check that having enough levels will give all possible answers.");
        
        Assert.True(!questionSystem.AttemptQuestion("OakSeed"), "Check that wrong answers return false and higher level answers exist after using a higher level.");
    }
}
