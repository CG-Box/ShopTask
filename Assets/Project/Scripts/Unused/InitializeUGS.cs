using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
 
public class InitializeUGS : MonoBehaviour {
    
    public const string k_Environment = "production";
 
    /*
    async void Start() {
        try {
            var options = new InitializationOptions()
                .SetEnvironmentName(k_Environment);
 
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception) {
            Debug.Log(exception);
        }
    }*/

    void Awake()
    {
        // Uncomment this line to initialize Unity Gaming Services.
        Initialize(OnSuccess, OnError);
    }

    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }

    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        Debug.Log(text);
    }

    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        Debug.LogError(text);
    }

    void Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            var text =
                "Error: Unity Gaming Services not initialized.\n" +
                "To initialize Unity Gaming Services, open the file \"InitializeGamingServices.cs\" " +
                "and uncomment the line \"Initialize(OnSuccess, OnError);\" in the \"Awake\" method.";
            Debug.LogError(text);
        }
    }
}