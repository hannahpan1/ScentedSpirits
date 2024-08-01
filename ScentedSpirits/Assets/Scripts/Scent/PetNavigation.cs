using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SearchService;

public class PetNavigation : MonoBehaviour
{
    private Vector3 spawnLocation;
    private NavMeshAgent agent;
    private Dictionary<string, int> agentTypes;
    private NavMeshPath path;
    public NavMeshSurface noLinksNavMesh; // NavMeshSurface with no jumping
    private NavMeshQueryFilter navMeshQueryFilter;
    private GameObject player;
    private InventoryManager inventory;
    public float followCoolDown = 0.5f; // how many sec between following
    private float timer;
    private bool isMoving; // Keeps track of whether the pet is moving
    public Animator myAnim;
    private bool inScentCloud = false; // checks if currently following a scent
    public float stoppingDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        GameEvents.current.changeAbility += OnChangeAbility;
        GameEvents.current.followScent += OnFollowScent;
        GameEvents.current.setPetSpawn += OnSetSpawnPoint;
        GameEvents.current.resetPetPos += OnResetPetPos;
        GameEvents.current.itemEaten += OnEaten;
        
        // Get agent types
        agentTypes = new Dictionary<string, int>();
        int count = UnityEngine.AI.NavMesh.GetSettingsCount();
        string[] agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            int id = UnityEngine.AI.NavMesh.GetSettingsByIndex(i).agentTypeID;
            string name = UnityEngine.AI.NavMesh.GetSettingsNameFromID(id);
            agentTypes[name] = id;
        }
        
        // Used to check if pet and player are on same plaform. Queries for
        // a navMeshAgent and surface where jumping is impossible
        path = new NavMeshPath();
        navMeshQueryFilter = new NavMeshQueryFilter();
        navMeshQueryFilter.agentTypeID = noLinksNavMesh.agentTypeID;
        navMeshQueryFilter.areaMask = noLinksNavMesh.layerMask;
        
        // Set starting spawn location
        spawnLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer so that the pet isn't following the player every frame
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = followCoolDown;
            followPlayer();
        }
    }

    private void followPlayer()
    {
        myAnim.SetBool("IsMoving",isMoving);
        // This checks if path (full or partial) is possible between the player and pet
        // without jumping
        bool pathPossible = NavMesh.CalculatePath(transform.position,
            player.transform.position, navMeshQueryFilter, path);
        
        // Check if player and pet are on the same platform
        if (pathPossible & path.status == NavMeshPathStatus.PathComplete)
        {
            // Check pet is not colliding with a particle and player has cheese in inventory
            if (!inScentCloud && inventory.isHoldingCheese)
            {
                agent.stoppingDistance = stoppingDistance;
                agent.destination = player.transform.position;
            }
        }
        
        // Update boolean if moving
        if (agent.velocity != new Vector3(0, 0, 0))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void OnEaten()
    {
        inScentCloud = false;
    }
    
    private void OnChangeAbility(string agentType)
    {
        int agentID;
        if (agentTypes.TryGetValue(agentType, out agentID))
        {
            agent.agentTypeID = agentID;
        }
    }
    
    private void OnFollowScent(Vector3 location)
    {
        agent.stoppingDistance = 0f;
        agent.destination = location;
        inScentCloud = true;
    }

    private void OnSetSpawnPoint(Vector3 position)
    {
        spawnLocation = position;
    }
    private void OnResetPetPos()
    {
        agent.ResetPath();
        gameObject.SetActive(false);
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
    }
}
