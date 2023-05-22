using UnityEngine;

[RequireComponent(typeof(Light))]
public class LampSwitcher : MonoBehaviour
{
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        OdometrApiRequester.OnSuccessfulConnect += HandleSuccessfulConnect;
        OdometrApiRequester.OnFailedConnect += HandleFailedConnect;
    }

    private void OnDisable()
    {
        OdometrApiRequester.OnSuccessfulConnect -= HandleSuccessfulConnect;
        OdometrApiRequester.OnFailedConnect -= HandleFailedConnect;
    }

    private void HandleSuccessfulConnect()
    {
        _light.color = Color.green;
    }

    private void HandleFailedConnect()
    {
        _light.color = Color.red;
    }
}
