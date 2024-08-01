using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeathScreenController : MonoBehaviour
{
    public Button restartButton;
    private Vector3 checkpoint;
    private GameObject player;
    private GameObject inventoryUI;
    private InventoryManager inventory;
    private EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        eventSystem = EventSystem.current;
        gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartClick);
        GameEvents.current.gameOver += ShowDeathScreen;
        GameEvents.current.setCheckpoint += OnSetCheckpoint;
        player = GameObject.FindGameObjectWithTag("Player");
        
        // set checkpoint to player's starting position
        checkpoint = player.transform.position;
    }

    void Update() // listen for controller submit
    {
        if (gameObject.activeSelf && Input.GetButtonDown("Submit"))
        {
            OnRestartClick();
        }
    }

    private void ShowDeathScreen()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        inventoryUI.SetActive(false);
        
        // Set the selected button for controller navigation
        eventSystem.SetSelectedGameObject(restartButton.gameObject);
    }

    private void OnSetCheckpoint(Vector3 position)
    {
        checkpoint = position;
    }
    
    public void OnRestartClick()
    {
        // We want the player and pet to respawn at its last checkpoint
        player.transform.position = checkpoint;
        GameEvents.current.resetPetPos?.Invoke();
        
        // Get rid of all cheese (duplicate method make it cleaner)
        GameObject[] allCheeses = GameObject.FindGameObjectsWithTag("cheese");
        foreach (GameObject cheese in allCheeses)
        {
            Destroy(cheese);
        }
        
        GameObject[] allSpicy = GameObject.FindGameObjectsWithTag("SpicyBiscuit");
        foreach (GameObject biscuit in allSpicy)
        {
            Destroy(biscuit);
        }
        
        // Clear inventory
        inventory.ClearInventory();
        
        // Disable game over screen
        gameObject.SetActive(false);
        inventoryUI.SetActive(true);
        Time.timeScale = 1f;
    }
}
