using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FallingObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private float interval;

    [SerializeField]
    private float spawnAreaWidth;

    [SerializeField]
    private GameObject prefabToSpawn;
    
    private float _intervalTimer;

    public IPlantBackend Backend; //Insert reference to global backend.

    private QuestionSet _currentQuestion = null;

    [HideInInspector]
    public int UninstantiatedAnswers;

    public List<FallingObject> instantiatedAnswers = new List<FallingObject>();
    
    [SerializeField]
    private TMP_Text questionTextBox;

    [SerializeField]
    private float glowTime;

    [SerializeField]
    private float glowCooldown;

    [SerializeField]
    private TMP_Text glowCooldownText;

    [SerializeField]
    private Image ladybugImage;

    [SerializeField]
    private Color glowColor;

    [SerializeField]
    private Color defaultColor;
    
    private float _glowTimer;

    public float FallingSpeed = 2;
    

    void Start()
    {
        Backend = GameManager.Instance.Backend;
    }

    private void FixedUpdate()
    {
        if (_glowTimer > glowCooldown - glowTime)
        {
            foreach (var answer in instantiatedAnswers)
            {
                if (answer.Answer.IsCorrect)
                {
                    answer.childRenderer.color = glowColor;
                }
            }
        }
        
        if (_glowTimer > 0)
        {
            glowCooldownText.text = ((int)_glowTimer).ToString();
            ladybugImage.color = new Color(ladybugImage.color.r, ladybugImage.color.g, ladybugImage.color.b, 0.5f);
        }
        else
        {
            glowCooldownText.text = "";
            ladybugImage.color = new Color(ladybugImage.color.r, ladybugImage.color.g, ladybugImage.color.b, 1f);
        }
    }

    public void UseLadybug()
    {
        if (_glowTimer > 0) return;
        _glowTimer = glowCooldown;
    }

    void Update()
    {
        FallingSpeed += Time.deltaTime * 0.05f;
        
        //Tick energy.
        GameManager.Instance.energyLevel.energy -= GameManager.Instance.energyLevel.tickedCost * Time.deltaTime;
        
        //Tick glow.
        _glowTimer -= Time.deltaTime;
        
        //Get a question if there isn't an active question.
        if (_currentQuestion == null)
        {
            _currentQuestion = Backend.GetQuestion(QuestionLocation.Falling);
            UninstantiatedAnswers = _currentQuestion.PossibleAnswers.Count;
            questionTextBox.text = _currentQuestion.DisplayQuestion;
            _intervalTimer = interval;
        }
        
        //Instantiate answers by interval until there are no more uninstantiated answers.
        if (UninstantiatedAnswers > 0)
        {
            _intervalTimer += Time.deltaTime;
            if (_intervalTimer > interval)
            {
                _intervalTimer = 0;

                float randomizedX = transform.position.x + Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                Vector3 spawnLocation = new Vector3(randomizedX, transform.position.y, 0);
                GameObject fallingObject = Instantiate(prefabToSpawn, spawnLocation, Quaternion.identity);
                FallingObject fallingObjectComp = fallingObject.GetComponent<FallingObject>();
                fallingObjectComp.Setup(_currentQuestion.PossibleAnswers[UninstantiatedAnswers - 1]);
                instantiatedAnswers.Add(fallingObjectComp);
                fallingObjectComp.generator = this;

                UninstantiatedAnswers--;
            }
        }

        //Reset question when there are no more answers.
        if (instantiatedAnswers.Count == 0)
        {
            _currentQuestion = null;
        }
    }
}