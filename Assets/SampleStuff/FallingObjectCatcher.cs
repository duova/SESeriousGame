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

    private int _score;

    [SerializeField]
    private FallingObjectGenerator generator;

    void Update()
    {
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FallingObject objectTouched;
        if (!other.TryGetComponent<FallingObject>(out objectTouched)) return;
        bool success = generator.Backend.AttemptQuestion(objectTouched.Answer.Handle);
        if (success)
        {
            _score++;
        }
        else if (_score > 0)
        {
            _score--;
        }

        textBox.text = _score.ToString();
    }
}
