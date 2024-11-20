using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ForagingController : MonoBehaviour
{
    public Text Question;
    public ForagingQuestion questionStore;

    private QuestionSet _currentQuestion;

    public IPlantBackend backend;
    public EnergyLevel energy;

    [SerializeField]
    private QuestionLibraryScriptableObject questionLibrary;

    private SpriteRenderer sprite1Renderer;
    private BoxCollider2D _sprite1BoxCollider2D;

    private void GenerateSprites(List<Answer> possibleAnswers)
    {
        GameObject sprite1 = new GameObject("Sprite1", typeof(SpriteRenderer));
        sprite1.AddComponent<BoxCollider2D>();

        sprite1Renderer = sprite1.GetComponent<SpriteRenderer>();
        sprite1Renderer.sprite = possibleAnswers[0].Sprite;

        sprite1Renderer.transform.position = new Vector3(-3, -4, 0);
        sprite1Renderer.transform.localScale = new Vector3(0.05f, 0.05f,0);
        sprite1Renderer.sortingOrder = 10;
        sprite1Renderer.sortingLayerName = "Default";
        
        _sprite1BoxCollider2D = sprite1.GetComponent<BoxCollider2D>();
        _sprite1BoxCollider2D.size = sprite1Renderer.sprite.bounds.size;
        Debug.Log(sprite1Renderer.sprite.bounds.size);
    }

    private void LoadQuestion(string retrievedQuestion)
    {
        Question.text = retrievedQuestion;
    }

    private void Start()
    {
        backend = new MockBackend(questionLibrary);
        _currentQuestion = backend.GetQuestion();

        LoadQuestion(_currentQuestion.DisplayQuestion);
        GenerateSprites(_currentQuestion.PossibleAnswers);
    }

    private void OnMouseUpAsButton()
    {
        //If Click on sprite, create answer handle.
        Debug.Log("Hit");
        if (backend.AttemptQuestion(_currentQuestion.PossibleAnswers[0].Handle))
        {
            sprite1Renderer.enabled = false;
            energy.Eat(10f);
        }

    }
}