using System;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    private float fallVelocity;

    [SerializeField]
    private Sprite goodImage;

    [SerializeField]
    private Sprite badImage;

    public bool isGood;

    private Rigidbody2D _rb;

    private SpriteRenderer _spriteRenderer;

    private float _destroyTimer;

    //I'm prefixing with in to differentiate between the param variable and the class variable.
    public void Setup(bool inIsGood)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        isGood = inIsGood;
        if (inIsGood)
        {
            _spriteRenderer.sprite = goodImage;
        }
        else
        {
            _spriteRenderer.sprite = badImage;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = new Vector2(0, -fallVelocity);
    }
    
    private void Update()
    {
        _destroyTimer += Time.deltaTime;
        //gameObject refers to the GameObject this component is on. Make sure you don't call Destroy(this) by accident
        //or you will only destroy the component and not the GameObject.
        if (_destroyTimer > 10) Destroy(gameObject);
    }
}
