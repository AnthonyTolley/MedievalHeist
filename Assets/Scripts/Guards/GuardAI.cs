using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GuardAI : MonoBehaviour
{
    // Start is called before the first frame update
    GuardStats guardStats = new();
    public GuardStats getGuardStats
        { get => guardStats; }
    iEnemyState currentState;
    private int fieldOfView;
    private int detectionMeter;
    private int detectionMeterMax;
    private int movementSpeed;
    private int sprintSpeed;
    private int detectionRange;
    private detectionScript detectionScriptvar;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent getNavMeshAgent
    { get => navMeshAgent;}

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject patrolRoute;

    private List<iEnemyState> enemyStates;
    void Start()
    {
        fieldOfView = guardStats.FieldOfView;
        detectionMeter = guardStats.DetectionMeter;
        detectionMeterMax = guardStats.DetectionMeterMax;
        movementSpeed = guardStats.MovementSpeed;
        sprintSpeed = guardStats.SprintSpeed;
        detectionRange = guardStats.DetectionRange;

        currentState = GetComponent<PatrolState>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        detectionScriptvar = GetComponentInChildren<detectionScript>();

        enemyStates = new List<iEnemyState>(3)
        {
            GetComponent<PatrolState>(),
            GetComponent<SearchState>(),
            GetComponent<ChaseState>()
        };

        navMeshAgent.speed = movementSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        //currentState.Action(player);
        currentState.Movement(this, player);
        currentState.Action(this, player, detectionScriptvar);//, detectionRange, detectionMeter, detectionMeterMax, fieldOfView);
    }

    public void changeState(int stateToSwitchTo)//0 is patrol, 1 is search, 2 is chase
    {
        //switch between searching, patrolling and chasing
        currentState = enemyStates[stateToSwitchTo];
        if (stateToSwitchTo == 0)
        {
            navMeshAgent.speed = movementSpeed;
        }
        else
        {
            navMeshAgent.speed = movementSpeed;
        }
    }

}
