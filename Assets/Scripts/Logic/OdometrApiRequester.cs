using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Threading.Tasks;

public class OdometrApiRequester : MonoBehaviour
{
    private string _serverAddress;
    private int _serverPort;

    private bool _isConnected = false;
    private ClientWebSocket _webSocket;

    private CancellationTokenSource _cancellationTokenSource;

    public static event Action OnSuccessfulConnect;
    public static event Action OnFailedConnect;
    public static event Action<float> OnNewOdometrValue;


    private void Start()
    {
        InitializeWebSocket();
        ConnectToServer();
    }

    private void OnEnable()
    {
        WebInfoUi.OnConfigChanged += HandleConfigChanged;
    }

    private void OnDisable()
    {
        WebInfoUi.OnConfigChanged -= HandleConfigChanged;
    }

    private void LoadConfig()
    {
        string configFile = Path.Combine(Application.streamingAssetsPath, "config.txt");
        string[] lines = File.ReadAllLines(configFile);
        foreach (string line in lines)
        {
            if (line.StartsWith("Адрес сервера:"))
            {
                _serverAddress = line.Substring(line.IndexOf(':') + 1).Trim();
            }
            else if (line.StartsWith("Порт:"))
            {
                int.TryParse(line.Substring(line.IndexOf(':') + 1).Trim(), out _serverPort);
            }
        }
    }

    private void InitializeWebSocket()
    {
        _webSocket = new ClientWebSocket();
    }

    private async void ConnectToServer()
    {
        while (true)
        {
            if (!_isConnected)
            {
                LoadConfig();
                InitializeWebSocket();
                try
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    await _webSocket.ConnectAsync(new Uri($"ws://{_serverAddress}:{_serverPort}/ws"), _cancellationTokenSource.Token);
                    _isConnected = true;
                    Debug.Log("Connection established.");
                    ReceiveOdometerValue();

                    OnSuccessfulConnect?.Invoke();
                }
                catch (WebSocketException ex)
                {
                    Debug.Log("WebSocket exception: " + ex.Message);
                    OnFailedConnect?.Invoke();
                }
            }
            await Task.Delay(2000);
        }
    }

    private async void ReceiveOdometerValue()
    {
        byte[] buffer = new byte[4096];
        while (_isConnected)
        {
            try
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                if (result.CloseStatus.HasValue)
                {
                    Debug.Log("Connection closed by the server.");
                    _isConnected = false;
                    OnFailedConnect?.Invoke();
                    break;
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessWebSocketMessage(message);
                }
            }
            catch (WebSocketException ex)
            {
                Debug.Log("WebSocket exception: " + ex.Message);
                _isConnected = false;
                OnFailedConnect?.Invoke();
                break;
            }
        }
    }

    private void ProcessWebSocketMessage(string message)
    {
        var newValue = SimpleJSON.JSON.Parse(message)["value"];
        if (newValue != null)
        {
            OnNewOdometrValue?.Invoke(newValue);
        }
    }

    [ContextMenu("Imitate server failed")]
    public void ImitateServerFailed()
    {
        OnFailedConnect?.Invoke();
        _isConnected = false;
    }

    private void HandleConfigChanged()
    {
        _isConnected = false;
    }

    private void OnApplicationQuit()
    {
        if (_isConnected)
        {
            _cancellationTokenSource.Cancel();
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Соединение закрыто", CancellationToken.None).Wait();
        }
    }
}