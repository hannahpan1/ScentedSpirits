using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string[] tutorialMessages;
    [SerializeField] private GameObject tutorialTextObject;
    [SerializeField] private float baseDisplayTime = 1.0f; // Base time to show the message
    [SerializeField] private float timePerCharacter = 0.1f; // Additional time per character

    private bool hasBeenTriggered = false;
    private TextMeshProUGUI tmpText;
    private Text uiText;
    private int currentMessageIndex = 0;
    private EventSystem eventSystem;

    private void Start()
    {
        tmpText = tutorialTextObject.GetComponent<TextMeshProUGUI>();
        uiText = tutorialTextObject.GetComponent<Text>();

        // Get the EventSystem
        eventSystem = EventSystem.current;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            currentMessageIndex = 0; // Reset message index for new zone
            StartCoroutine(ShowTutorialMessages());
        }
    }

    private IEnumerator ShowTutorialMessages()
    {
        while (currentMessageIndex < tutorialMessages.Length)
        {
            ShowTutorialTip(tutorialMessages[currentMessageIndex]);
            float displayTime = CalculateDisplayTime(tutorialMessages[currentMessageIndex]);
            yield return new WaitForSeconds(displayTime);

            currentMessageIndex++;
        }

        tutorialTextObject.SetActive(false);
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
    }

    private float CalculateDisplayTime(string message)
    {
        return baseDisplayTime + (message.Length * timePerCharacter);
    }
}
