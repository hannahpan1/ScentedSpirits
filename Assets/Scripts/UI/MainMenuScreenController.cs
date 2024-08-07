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
    public AudioSource masterAudio;
    public AudioSource audioSource1;
    public AudioClip menuMusic;
    public AudioSource audioSource2;
    public AudioClip buttonSelected;

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
        masterAudio.Pause();
        audioSource1.clip = menuMusic;
        audioSource1.Play();
        inventoryUI.SetActive(false);
        _mainMenuCanvas.SetActive(true);
        _settingsMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void CloseAllMenus()
    {
        audioSource1.Pause();
        masterAudio.Play();
        EventSystem.current.SetSelectedGameObject(null);
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(false);
        inventoryUI.SetActive(true);
    }

    public void OnSettingsPress()
    {
        PlaySelectedButtonAudio();
        OpenSettingsMenu();
    }

    public void OnRestartPress()
    {
        PlaySelectedButtonAudio();
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnContinuePress()
    {
        PlaySelectedButtonAudio();
        Unpause();
    }

    public void OnExitPress()
    {
        PlaySelectedButtonAudio();
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleScene);
    }

    public void OnSettingsExit()
    {
        PlaySelectedButtonAudio();
        OpenMainMenu();
    }

    private void OpenSettingsMenu()
    {
        PlaySelectedButtonAudio();
        _mainMenuCanvas.SetActive(false);
        _settingsMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
    }

    private void PlaySelectedButtonAudio()
    {
        audioSource2.clip = buttonSelected;
        audioSource2.Play();
    }
}
