using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeUi : MonoBehaviour
{
	[SerializeField] private AudioMixer mixer;

	private float _currentSoundVolume, _currentMusicVolume;

	private bool _soundOn = true;
	private bool _musicOn = true;

    private void Awake()
    {
		_currentMusicVolume = Mathf.Log10(1) * 20;
        _currentSoundVolume = Mathf.Log10(1) * 20;
    }

    private void OnEnable()
    {
		VideoController.OnVideoWithSoundPlaying += HandleVideoWithSoundPlaying;
		VideoController.OnVideoEnded += HandleVideoEnded;
    }

    private void OnDisable()
    {
        VideoController.OnVideoWithSoundPlaying -= HandleVideoWithSoundPlaying;
        VideoController.OnVideoEnded -= HandleVideoEnded;
    }

    public void HandleSoundSilderChange(float newValue)
	{
		_currentSoundVolume = Mathf.Log10(newValue) * 20;
		if (_soundOn)
			mixer.SetFloat("SoundVolume", _currentSoundVolume);
	}

    public void HandleMusicSilderChange(float newValue)
    {
        _currentMusicVolume = Mathf.Log10(newValue) * 20;
        if (_musicOn)
            mixer.SetFloat("MusicVolume", _currentMusicVolume);
    }

	public void HandleSoundLabelChanged(bool newValue)
	{
		_soundOn = newValue;
		if (_soundOn)
			mixer.SetFloat("SoundVolume", _currentSoundVolume);
		else
			mixer.SetFloat("SoundVolume", Mathf.Log10(0.00001f) * 20);
	}

	public void HandleMusicLabelChanged(bool newValue)
	{
        _musicOn = newValue;
        if (_musicOn)
            mixer.SetFloat("MusicVolume", _currentMusicVolume);
        else
            mixer.SetFloat("MusicVolume", Mathf.Log10(0.00001f) * 20);
    }

	private bool _previousMusicLabel;
	private void HandleVideoWithSoundPlaying()
	{
		_previousMusicLabel = _musicOn;
		HandleMusicLabelChanged(false);
	}

	private void HandleVideoEnded()
	{
		if (_previousMusicLabel)
			HandleMusicLabelChanged(true);
	}
}