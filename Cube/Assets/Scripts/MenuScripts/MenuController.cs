using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Button firstGameButton;
    public Button secondGameButton;
    public TMP_Text connectionStatusText;

    private ESP32BLEApp esp32BLEApp;

    void Start()
    {
        // Add listeners to the buttons
        firstGameButton.onClick.AddListener(() => LoadScene("FirstGameScene"));
        secondGameButton.onClick.AddListener(() => LoadScene("SecondGameScene"));

        // Get reference to the ESP32BLEApp component
        esp32BLEApp = FindObjectOfType<ESP32BLEApp>();

        // Update button states
        UpdateButtonStates();
    }

    void Update()
    {
        // Periodically check the connection status and update button states
        if (esp32BLEApp != null)
        {
            UpdateButtonStates();
        }
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void UpdateButtonStates()
    {
        bool isConnected = esp32BLEApp != null && esp32BLEApp.IsConnected();
        firstGameButton.interactable = isConnected;
        secondGameButton.interactable = isConnected;

        // Update the connection status text
        connectionStatusText.text = isConnected ? "Connected" : "Disconnected";
    }
}
