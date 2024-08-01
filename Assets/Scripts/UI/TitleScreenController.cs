using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour
{
    public Button startButton;
    public string levelToLoad;
    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        eventSystem = EventSystem.current;

        // Set the selected button for controller navigation
        eventSystem.SetSelectedGameObject(startButton.gameObject);
    }

    void Update()
    {
        // Check for controller input to activate the button
        if (Input.GetButtonDown("Submit"))
        {
            OnStartClick();
        }
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
