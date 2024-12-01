using System;
using TMPro;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    private float fallVelocity;

    public Answer Answer;

    private Rigidbody2D _rb;

    private SpriteRenderer _spriteRenderer;

    public SpriteRenderer childRenderer;

    [SerializeField]
    private float screenBottomY;
    
    [SerializeField]
    private TMP_Text answerTextBox;

    [HideInInspector]
    public FallingObjectGenerator generator;

    public void Setup(Answer answer)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (answer.AnswerDisplayType is AnswerDisplayType.Sprite or AnswerDisplayType.TextAndSprite)
        {
            _spriteRenderer.sprite = answer.Sprite;
            Answer = answer;
        }
        if (answer.AnswerDisplayType is AnswerDisplayType.Text or AnswerDisplayType.TextAndSprite)
        {
            answerTextBox.text = answer.DisplayText;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = new Vector2(0, -fallVelocity);
    }
    
    private void Update()
    {
        if (transform.position.y < screenBottomY)
        {
            Destroy(gameObject);
            if (generator.instantiatedAnswers.Contains(this))
            {
                generator.instantiatedAnswers.Remove(this);
            }
        }
    }

    public void Hide()
    {
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = false;
        }
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }
}