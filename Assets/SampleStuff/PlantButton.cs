using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct PlantData
{
    public string name;
    
    public Sprite sprite;
}

public class PlantButton : MonoBehaviour
{
    [SerializeField]
    private List<PlantData> PlantDatas = new List<PlantData>();

    private SpriteRenderer _spriteRenderer;

    private BoxCollider2D _boxCollider2D;

    private PlantData _selectedPlant;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        int randomIndex = Random.Range(0, PlantDatas.Count);
        _selectedPlant = PlantDatas[randomIndex];

        _spriteRenderer.sprite = _selectedPlant.sprite;
        _boxCollider2D.size = _spriteRenderer.sprite.bounds.size;
    }

    private void OnMouseUpAsButton()
    {
        _spriteRenderer.enabled = false;
        print(_selectedPlant.name + " was collected.");
    }
}
