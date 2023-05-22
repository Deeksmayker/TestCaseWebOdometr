using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    public static event Action OnVideoPlaying;
    public static event Action OnVideoWithSoundPlaying;
    public static event Action OnVideoEnded;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    public void HandlePlayButton()
    {
        var file = Path.Combine(Application.streamingAssetsPath, "config.txt");
        var lines = File.ReadAllLines(file);

        var url = lines[2].Substring(lines[2].IndexOf(':') + 1).Trim();

        _videoPlayer.url = url;
        _videoPlayer.Play();
        OnVideoPlaying?.Invoke();

        if (_videoPlayer.controlledAudioTrackCount > 0)
        {
            OnVideoWithSoundPlaying?.Invoke();
        }
    }

    public void HandlePauseButton()
    {
        _videoPlayer.Stop();
        OnVideoEnded?.Invoke();
    }
}
