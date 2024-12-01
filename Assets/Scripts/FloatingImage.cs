using System;
using UnityEngine;

public class FloatingImage : MonoBehaviour
{
    [SerializeField]
    private float upwardsSpeed;

    [SerializeField]
    private float fadeTime;

    private float _fadeTimer;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, upwardsSpeed * Time.deltaTime, 0);
        _spriteRenderer.color -= new Color(0, 0, 0, Time.deltaTime / fadeTime);
        
        _fadeTimer += Time.deltaTime;

        if (_fadeTimer >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}