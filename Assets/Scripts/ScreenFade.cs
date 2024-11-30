using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance { get; private set; }

    [SerializeField]
    private Image blackScreen;

    private float _rate;

    private float _alpha;

    private bool _callbackCalled;

    private Action _savedFadedCallback;

    private void Awake()
    {
        Instance = this;
        blackScreen.enabled = true;
    }

    private void Start()
    {
        _alpha = 1f;
        FadeIn(0.5f);
    }

    public void FadeIn(float time)
    {
        _rate = -1f / time;
    }

    public void FadeOut(float time, Action fadedCallback)
    {
        _rate = 1f / time;
        _savedFadedCallback = fadedCallback;
    }

    private void Update()
    {
        _alpha = Mathf.Clamp(_alpha + _rate * Time.unscaledDeltaTime, 0f, 1f);

        if (_alpha > 0.99f)
        {
            if (!_callbackCalled)
            {
                _savedFadedCallback?.Invoke();
                _callbackCalled = true;
            }
        }
        else
        {
            _callbackCalled = false;
        }

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, _alpha);
    }
}