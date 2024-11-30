using System;
using UnityEngine;

public class ForagingFood : MonoBehaviour
{
    private PlantEntryScriptableObject _plant;

    private PlantFeatureScriptableObject _feature;

    private ForagingController _controller;

    private SpriteRenderer _spriteRenderer;

    private BoxCollider2D _boxCollider2D;

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void SetFood(ForagingController controller, PlantEntryScriptableObject plant, PlantFeatureScriptableObject feature, Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
        _boxCollider2D.size = _spriteRenderer.sprite.bounds.size;
        _controller = controller;
        _plant = plant;
        _feature = feature;
    }
    
    public void Clicked()
    {
        _controller.SelectFood(_plant, _feature, gameObject);
    }

    public void OnMouseUp()
    {
        if (!_controller.disableFoodClicking)
        {
            Clicked();
        }
    }
}
