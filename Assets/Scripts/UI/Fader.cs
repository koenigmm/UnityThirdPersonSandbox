using System;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private float noTransparencyDuration = 2f;
    [SerializeField] private float fadingDuration = 2f;
    [SerializeField] private Image fadingBackgroundImage;
    [SerializeField] private Canvas fadingCanvas;
    private float _timer;
    private bool _isFading;

    private void OnEnable()
    {
        fadingCanvas.enabled = true;
    }

    private void Update()
    {
        var tempColor = fadingBackgroundImage.color;
        var alphaAsPercentage = 1f;
        _timer += Time.deltaTime;

        if (!_isFading && _timer >= noTransparencyDuration)
        {
            _isFading = true;
            _timer = 0f;
        }
        
        if (_isFading) alphaAsPercentage = 1 - _timer / fadingDuration;
        tempColor.a = alphaAsPercentage;

        fadingBackgroundImage.color = tempColor;

        const float toleranceToNoTransparency = 0.05f;
        if (_isFading && fadingBackgroundImage.color.a <= toleranceToNoTransparency) gameObject.SetActive(false);
    }
}