using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ConnectionsAudioController : MonoBehaviour
{
	[SerializeField] private AudioClip successfulConnectAudioClip;
	[SerializeField] private AudioClip failedConnectAudioClip;

	private AudioSource _connectedAudioSource;

    private void Awake()
    {
        _connectedAudioSource = GetComponent<AudioSource>();
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
        _connectedAudioSource.clip = successfulConnectAudioClip;
        _connectedAudioSource.PlayOneShot(successfulConnectAudioClip);
    }

    private void HandleFailedConnect()
    {
        _connectedAudioSource.clip = failedConnectAudioClip;
        _connectedAudioSource.PlayOneShot(failedConnectAudioClip);
    }
}