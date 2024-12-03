using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public struct ForagingAnswer
{
    public PlantEntryScriptableObject Plant;

    public PlantFeatureScriptableObject Feature;
}

/*
We want to take the plant features that they have access to and populate the environment with those images.
If they consume non-edible plants the words the *plant feature is not edible! and will decrease their energy.
If it is edible a simple question will ask them what the plant feature is with a combination of plant name concat feature name.
*/
public class ForagingController : MonoBehaviour
{
    private PlantLibraryScriptableObject _plantLib;

    private DefaultPlantBackend _backend;
    
    [SerializeField]
    private GameObject questionObject;

    [SerializeField]
    private TMP_Text questionText;
    
    [SerializeField]
    private List<TMP_Text> questionAnswerTexts = new List<TMP_Text>();
    
    public List<ForagingFood> foods = new List<ForagingFood>();

    [SerializeField]
    private TMP_Text informationText;

    [SerializeField]
    private float eatValue;

    [SerializeField]
    private Color correctColor;

    [SerializeField]
    private Color wrongColor;

    private int _correctAnswerText;

    readonly Dictionary<PlantFeatureScriptableObject, PlantEntryScriptableObject> usablePlantFeatures = new Dictionary<PlantFeatureScriptableObject, PlantEntryScriptableObject>();
    readonly Dictionary<PlantFeatureScriptableObject, PlantEntryScriptableObject> ediblePlantFeatures = new Dictionary<PlantFeatureScriptableObject, PlantEntryScriptableObject>();

    private GameObject _selectedFood;

    public bool disableFoodClicking;

    public static ForagingController Instance { get; private set; }

    private bool _setup;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_setup)
        {
            _setup = true;
            //informationText.color = new Color(informationText.color.r, informationText.color.g, informationText.color.b, 0);

            GameManager.Instance.energyLevel.energy = GameManager.Instance.energyLevel.maxEnergy * 0.5f;
            
            _backend = GameManager.Instance.Backend;
            _plantLib = GameManager.Instance.plantLibrary;
            questionObject.SetActive(false);
        
            foreach (var plant in _plantLib.plantEntries)
            {
                if (plant.environment != GameManager.Instance.environment) continue;
                foreach (var feature in plant.features)
                {
                    if (feature.sprites.Count <= 0) continue;
                    usablePlantFeatures.Add(feature, plant);
                }
            }

            foreach (var fruit in _plantLib.fruitEntries)
            {
                if (fruit.environment != GameManager.Instance.environment) continue;
                foreach (var feature in fruit.features)
                {
                    if (feature.sprites.Count <= 0) continue;
                    ediblePlantFeatures.Add(feature, fruit);
                }
            }

            var count = 0;
            var ediblePlacement1 = Random.Range(0, foods.Count);
            var ediblePlacement2 = Random.Range(0, foods.Count);
            foreach (var food in foods)
            {
                if (count == ediblePlacement1 || count == ediblePlacement2)
                {
                    var rand = Random.Range(0, ediblePlantFeatures.Count);
                    var i = 0;
                    foreach (var pair in ediblePlantFeatures)
                    {
                        if (i == rand)
                        {
                            food.SetFood(this, pair.Value, pair.Key,
                                pair.Key.sprites[Random.Range(0, pair.Key.sprites.Count)]);
                            break;
                        }

                        i++;
                    }
                }
                else
                {
                    var rand = Random.Range(0, usablePlantFeatures.Count);
                    var i = 0;
                    foreach (var pair in usablePlantFeatures)
                    {
                        if (i == rand)
                        {
                            food.SetFood(this, pair.Value, pair.Key,
                                pair.Key.sprites[Random.Range(0, pair.Key.sprites.Count)]);
                            break;
                        }

                        i++;
                    }
                }

                count++;
            }
        }
        
        informationText.color -= new Color(0, 0, 0, 0.15f * Time.deltaTime);
    }

    public void SelectFood(PlantEntryScriptableObject plant, PlantFeatureScriptableObject feature, GameObject food)
    {
        if (feature.isEdible)
        {
            disableFoodClicking = true;
            _selectedFood = food;
            questionObject.SetActive(true);
            questionText.text = "Only eat what you know for certain! What is this?";
            //Temporarily remove the correct feature to only have incorrect features.
            //usablePlantFeatures.Remove(feature);
            List<ForagingAnswer> incorrectAnswers = new();
            //-1 as we need one less incorrect answer than all answers.
            for (int i = 0; i < questionAnswerTexts.Count - 1; i++)
            {
                var rand = Random.Range(0, usablePlantFeatures.Count);
                int j = 0;
                foreach (var pair in usablePlantFeatures)
                {
                    if (j == rand)
                    {
                        var answer = new ForagingAnswer
                        {
                            Feature = pair.Key,
                            Plant = pair.Value
                        };
                        incorrectAnswers.Add(answer);
                        usablePlantFeatures.Remove(pair.Key);
                        break;
                    }
                    j++;
                }
            }
            //usablePlantFeatures.Add(feature, plant);
            foreach (var answer in incorrectAnswers)
            {
                usablePlantFeatures.Add(answer.Feature, answer.Plant);
            }
            _correctAnswerText = Random.Range(0, questionAnswerTexts.Count);
            for (int i = 0; i < questionAnswerTexts.Count; i++)
            {
                if (i != _correctAnswerText)
                {
                    var answerToUse = incorrectAnswers.First();
                    questionAnswerTexts[i].text = answerToUse.Plant.displayName + " " + answerToUse.Feature.displayName;
                    incorrectAnswers.RemoveAt(0);
                }
                else
                {
                    questionAnswerTexts[i].text = plant.displayName + " " + feature.displayName;
                }
            }
        }
        else
        {
            informationText.text = "The " + plant.displayName + " " + feature.displayName + " is not edible!";
            informationText.color = new Color(wrongColor.r, wrongColor.g, wrongColor.b, 1f);
            Destroy(food);
        }
    }

    public void AnswerQuestion(int index)
    {
        if (_selectedFood)
        {
            Destroy(_selectedFood);
        }

        disableFoodClicking = false;
        
        questionObject.SetActive(false);

        if (index == _correctAnswerText)
        {
            informationText.text = "Correct! You get more energy!";
            informationText.color = new Color(correctColor.r, correctColor.g, correctColor.b, 1f);
            GameManager.Instance.energyLevel.Eat(eatValue);
        }
        else
        {
            informationText.text = "That wasn't it, try to find other food!";
            informationText.color = new Color(wrongColor.r, wrongColor.g, wrongColor.b, 1f);
        }
    }
}