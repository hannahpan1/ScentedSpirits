using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

public class CookingStation : MonoBehaviour
{
    public GameObject testGroup;
    public InventoryManager inventoryManager;
    public Item Cheese;
    public Item spicyCheese;
    public int cheeseCount = 4;
    public int spicyCheeseCount = 4;

    void Start()
    {
        if (testGroup != null)
        {
            testGroup.SetActive(false);
        }
        
    }

    public void StartDelay()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        ClearThrownCheeses();
        GiveBiscuits();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventoryManager != null)
            {
                inventoryManager.ClearInventory();
                StartDelay();
            }
            
            // Reset the velocity and position of pet
            GameEvents.current.resetPetPos?.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (testGroup != null)
            {
                testGroup.SetActive(false);
            }
        }
    }
    
    public void ClearThrownCheeses()
    {
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
    }

    private void GiveBiscuits()
    {
        for (int i = 0; i < cheeseCount; i++)
        {
            inventoryManager.AddItem(Cheese);
        }
        for (int i = 0; i < spicyCheeseCount; i++)
        {
            inventoryManager.AddItem(spicyCheese);
        }
    }
}