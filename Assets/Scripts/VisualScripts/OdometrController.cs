using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OdometrController : MonoBehaviour
{
    [SerializeField] private float changeValueTime = 2f;

    private float _timer;
    private float _oldValue;
    private float _currentValue;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OdometrApiRequester.OnNewOdometrValue += HandleNewOdometrValue;
    }

    private void OnDisable()
    {
        OdometrApiRequester.OnNewOdometrValue -= HandleNewOdometrValue;
    }

    private void Update()
    {
        if (_timer > 0 )
        { 
            var value = Mathf.Lerp(_oldValue, _currentValue, 1 - _timer);
            _text.text = value.ToString();
            _timer -= Time.deltaTime / changeValueTime;
        }
    }

    private void HandleNewOdometrValue(float newValue)
    {
        _oldValue = _currentValue;
        _currentValue = newValue;
        _timer = 1f;
    }
}