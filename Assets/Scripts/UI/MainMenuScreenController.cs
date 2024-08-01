using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScreenController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _settingsMenuCanvas;
    private bool isPaused = false;
    private GameObject inventoryUI;
    private EventSystem eventSystem;
    public string titleScene;
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
    }

    void Update() // listen for controller submit
    {
        if (InputManager.current.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    private void OnContinueClick()
    {
    }
    
    private void OnExitClick()
    {
    }
    
    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        OpenMainMenu();
    }
    
    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
    }

    private void OpenMainMenu()
    {
        inventoryUI.SetActive(false);
        _mainMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void CloseAllMenus()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        inventoryUI.SetActive(true);
    }

    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }
    
    public void OnRestartPress()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnContinuePress()
    {
        Unpause();
    }
    
    public void OnExitPress()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleScene);
    }
    
    public void OnSettingsExit()
    {
        OpenMainMenu();
    }
    
    private void OpenSettingsMenu()
    {
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
    }
}
