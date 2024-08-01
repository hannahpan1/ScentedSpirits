using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
    public GameObject petSpawnPoint;
    private Transform spawnTransform;
    private Transform checkPointTransform;
    private Vector3 petStartingPos;
    private Vector3 playerStartingPos;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnTransform = petSpawnPoint.transform;
        checkPointTransform = gameObject.transform;
        petStartingPos = GameObject.FindGameObjectWithTag("rat").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with cooking station and if so set checkpoint
        if (other.gameObject.CompareTag("Player"))
        {
            // Set the spawn point of the pet and player to the cooking station
            GameEvents.current.setCheckpoint?.Invoke(new Vector3(checkPointTransform.position.x,
                other.gameObject.transform.position.y,
                checkPointTransform.position.z));
            GameEvents.current.setPetSpawn?.Invoke(new Vector3(spawnTransform.position.x,
                    petStartingPos.y,
                    spawnTransform.position.z));
            GameEvents.current.resetPetPos?.Invoke();
        }
    }
}
