using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string[] tutorialMessages;
    [SerializeField] private GameObject tutorialTextObject;
    [SerializeField] private Button continueButton;
    [SerializeField] private string buttonText = "Continue"; // Default button text

    private bool hasBeenTriggered = false;
    private TextMeshProUGUI tmpText;
    private Text uiText;
    private TextMeshProUGUI buttonTmpText;
    private Text buttonUIText;
    private int currentMessageIndex = 0;
    private playerMovement playerMovementScript;
    private Throw2 playerThrowScript;
    private EventSystem eventSystem;

    private void Start()
    {
        tmpText = tutorialTextObject.GetComponent<TextMeshProUGUI>();
        uiText = tutorialTextObject.GetComponent<Text>();
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        continueButton.gameObject.SetActive(false);

        // Find the playerMovement and Throw2 scripts
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<playerMovement>();
            playerThrowScript = player.GetComponent<Throw2>();
        }

        // Get the button text component
        buttonTmpText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonUIText = continueButton.GetComponentInChildren<Text>();

        // Set the initial button text
        SetButtonText(buttonText);

        // Set the button text color to white
        SetButtonTextColor(Color.white);

        // Get the EventSystem
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        // Check for controller input to activate the button
        if (continueButton.gameObject.activeSelf && Input.GetButtonDown("Submit"))
        {
            OnContinueButtonClicked();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            currentMessageIndex = 0; // Reset message index for new zone
            DisablePlayerControls();
            ShowTutorialTip(tutorialMessages[currentMessageIndex]);
        }
    }

    private void ShowTutorialTip(string message)
    {
        Debug.Log($"Showing tutorial message: {message}");
        tutorialTextObject.SetActive(true);
        if (tmpText != null)
        {
            tmpText.text = message;
        }
        else if (uiText != null)
        {
            uiText.text = message;
        }
        continueButton.gameObject.SetActive(true);

        // Set the selected button for controller navigation
        eventSystem.SetSelectedGameObject(continueButton.gameObject);
    }

    private void OnContinueButtonClicked()
    {
        Debug.Log($"Button clicked, currentMessageIndex: {currentMessageIndex}");
        continueButton.gameObject.SetActive(false);
        tutorialTextObject.SetActive(false);

        currentMessageIndex++;
        if (currentMessageIndex < tutorialMessages.Length)
        {
            ShowTutorialTip(tutorialMessages[currentMessageIndex]);
        }
        else
        {
            Debug.Log("All messages shown, enabling player controls.");
            EnablePlayerControls();
        }
    }

    private void DisablePlayerControls()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.canMove = false;
        }
        if (playerThrowScript != null)
        {
            playerThrowScript.canThrow = false;
        }
    }

    private void EnablePlayerControls()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.canMove = true;
        }
        if (playerThrowScript != null)
        {
            playerThrowScript.canThrow = true;
        }
    }

    private void SetButtonText(string text)
    {
        if (buttonTmpText != null)
        {
            buttonTmpText.text = text;
        }
        else if (buttonUIText != null)
        {
            buttonUIText.text = text;
        }
    }

    private void SetButtonTextColor(Color color)
    {
        if (buttonTmpText != null)
        {
            buttonTmpText.color = color;
        }
        else if (buttonUIText != null)
        {
            buttonUIText.color = color;
        }
    }
}
