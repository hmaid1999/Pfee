using UnityEngine;
using UnityEngine.UI; // Add this line to use the Button type
using UnityEngine.SceneManagement; // Add this line to use SceneManager

public class ReturnMenu : MonoBehaviour
{
    public Button ReturnButton;

    void Start()
    {
        // Add listener to the button
        ReturnButton.onClick.AddListener(() => LoadScene("MenuSelection"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
