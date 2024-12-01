using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FallingObjectCatcher : MonoBehaviour
{
    [SerializeField]
    private float clampWidth;

    [SerializeField]
    private float velocity;

    [SerializeField]
    private TMP_Text textBox;

    private float _cachedFontSize;

    private int _streak;

    [SerializeField]
    private FallingObjectGenerator generator;

    [SerializeField]
    private GameObject correctIndicator;

    [SerializeField]
    private GameObject wrongIndicator;

    private void Start()
    {
        _cachedFontSize = textBox.fontSize;
    }

    void Update()
    {
        if (textBox.fontSize > _cachedFontSize)
        {
            textBox.fontSize -= Time.deltaTime * 150;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > -clampWidth / 2)
            {
                transform.position += Vector3.left * (velocity * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < clampWidth / 2)
            {
                transform.position += Vector3.right * (velocity * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            GameManager.Instance.energyLevel.energy -= GameManager.Instance.energyLevel.moveCost * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FallingObject objectTouched;
        if (!other.TryGetComponent<FallingObject>(out objectTouched)) return;
        bool success = generator.Backend.AttemptQuestion(objectTouched.Answer.Handle);
        if (success)
        {
            _streak++;
            textBox.fontSize *= 2;
            Instantiate(correctIndicator, transform.position, Quaternion.identity);
        }
        else
        {
            _streak = 0;
            Instantiate(wrongIndicator, transform.position, Quaternion.identity);
        }

        objectTouched.Hide();
        textBox.text = _streak.ToString();
    }
}