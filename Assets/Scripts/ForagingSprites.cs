using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantSprite : MonoBehaviour
{

    public IPlantBackend Backend;
    public EnergyLevel level;
    public int questionNum;
    public int answer;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private PlantData _selectedPlant;
    
    [SerializeField]
    private QuestionLibraryScriptableObject questionLibrary;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _spriteRenderer.sprite = questionLibrary.questionEntries[questionNum].answerEntries[answer].sprite;
        _boxCollider2D.size = _spriteRenderer.sprite.bounds.size;
    }

    private void OnMouseUpAsButton()
    {
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = false;
        }

        if (questionLibrary.questionEntries[questionNum].answerEntries[answer].isCorrect)
        {
            level.Eat(30f);
        }
        else
        {
            level.Eat(-5.0f);
        }
        
    }
}