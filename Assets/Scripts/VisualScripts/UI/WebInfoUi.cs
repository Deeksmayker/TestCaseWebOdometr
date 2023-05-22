using System;
using System.IO;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class WebInfoUi : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField, portInputField, videoAddressInputField;

    private const string BaseIp = "185.246.65.199";
    private const string BasePort = "9090";
    private const string BaseVideoUrl = "file://D:/UnityProjects/TestCaseWebOdometr/Assets/StreamingAssets/Capibara.MP4";

    public static event Action OnConfigChanged;

    private void Awake()
    {
        SetInputFieldsValuesFromConfig();
    }

    public void HandleIpChanged(string ip)
    {
        HandleConfigChanged(ip, 0, "Адрес сервера");
    }

    public void HandlePortChanged(string port)
    {
        HandleConfigChanged(port, 1, "Порт");
    }

    public void HandleVideoAddressChanged(string viddeoAdress)
    {
        HandleConfigChanged(viddeoAdress, 2, "Видео адрес");
    }

    private void HandleConfigChanged(string newValue, int lineIndex, string title)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            return;
        }

        var file = Path.Combine(Application.streamingAssetsPath, "config.txt");
        var lines = File.ReadAllLines(file);
        lines[lineIndex] = $"{title}: {newValue}";
        File.WriteAllLines(file, lines);
        OnConfigChanged?.Invoke();
    }

    public void HandleRestoreButton()
    {
        HandleIpChanged(BaseIp);
        HandlePortChanged(BasePort);
        HandleVideoAddressChanged(BaseVideoUrl);
        SetInputFieldsValuesFromConfig();
        OnConfigChanged?.Invoke();
    }

    private void SetInputFieldsValuesFromConfig()
    {
        var file = Path.Combine(Application.streamingAssetsPath, "config.txt");
        var lines = File.ReadAllLines(file);

        ipInputField.text = lines[0].Substring(lines[0].IndexOf(':') + 1).Trim();
        portInputField.text = lines[1].Substring(lines[1].IndexOf(':') + 1).Trim();
        videoAddressInputField.text = lines[2].Substring(lines[2].IndexOf(':') + 1).Trim();
    }
}
