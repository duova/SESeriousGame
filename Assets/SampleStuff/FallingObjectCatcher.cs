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
    
    // Update is called once per frame
    void Update()
    {
        //Note that I'm clamping with a middle of 0 x here which is often not the case but just for simplicity.
        
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
        //Slightly new syntax here basically out specifies that a variable is used explicitly as an output, meaning it is
        //guaranteed to be written to in the function.
        FallingObject objectTouched;
        if (!other.TryGetComponent<FallingObject>(out objectTouched)) return;
        if (objectTouched.isGood)
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
