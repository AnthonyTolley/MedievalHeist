using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PatrolState : MonoBehaviour, iEnemyState
{
    private List<Transform> patrolList = new List<Transform>();
    private bool atWaitSpot = false;
    private bool waitTimerStarted = false;
    private bool detectTimerStarted = false;
    private int currentSpotIndex = 0;
    [SerializeField] private GameObject patrolListParent;

    /// <summary>
    /// DEBUG STUFF REMOVE AT END
    /// </summary>
    [SerializeField] private Text previousPatrol;
    [SerializeField] private Text currentPatrol;
    [SerializeField] private Text currentDetection;
    //private int currentDetection;

    void Start()
    {
        for (int i = 0; i < patrolListParent.transform.childCount; i++)
        {
            patrolList.Insert(i, patrolListParent.transform.GetChild(i));
            //Debug.Log(patrolListParent.transform.GetChild(i)+ "" +i);
        }
    }
    public void Action(GuardAI guard, GameObject player, detectionScript detectionScriptvar)//, int detectionRange, int detectionMeter, int detectionMeterMax, int fieldOfView)
    {
        //Checking if player is within sight and dealing with detection mechanics.

        /*
        Debug.DrawRay(this.transform.position, Vector3.forward*detectionRange);
        if(Physics.Raycast(this.transform.position, Vector3.forward, detectionRange))
        {
            Debug.Log("PLAYER IN SIGHT");
        }
        */
        //We do this 70ish times for the full fov?
        /*
        for (Vector3 x = new Vector3(-(fieldOfView/2),0,0); x.x < fieldOfView/2; x.x++)
        {
            Debug.DrawRay(this.transform.position, (Vector3.forward+x) * detectionRange);
            if (Physics.Raycast(this.transform.position, (Vector3.forward+x), detectionRange))
            {
                //Debug.Log("PLAYER IN SIGHT");
            }
            Debug.Log(x.ToString());
        }
        */
        //45 degrees left is (-1,0,0) 45 right is (1,0,0)

        //This works but is very unoptimised.
        //This doesn't work with rotation
        //Only using x currently. Need to incorporate z as well for when we're turned left/right
        /*
        for (float i = this.transform.forward.x-1; i < 1; i=i+0.05f)
        {
            if (Physics.Raycast(this.transform.position, (this.transform.forward + new Vector3(i, 0, this.transform.forward.z)), guard.getGuardStats.DetectionRange))
            {
                Debug.Log("PLAYER IN SIGHT");
            }
            Debug.DrawRay(this.transform.position, (this.transform.forward + new Vector3(i, 0, this.transform.forward.z)) * guard.getGuardStats.DetectionRange);
        }
        Debug.Log(this.transform.forward);
        */
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        if (detectionScriptvar.getPlayerDetected && !detectTimerStarted)
        {
            //Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position, Color.white, Vector3.Distance(guard.transform.position, player.transform.position));
            if (!Physics.Raycast(this.transform.position,player.transform.position - this.transform.position, Vector3.Distance(guard.transform.position, player.transform.position), layerMask))
            {
                StartCoroutine(increaseDetection(guard));
                detectTimerStarted = true;
            }
        }
        else if (!detectTimerStarted)
        {
            StartCoroutine(decreaseDetection(guard));
            detectTimerStarted = true;
        }
        currentDetection.text = guard.getGuardStats.DetectionMeter.ToString();
    }
    public void Movement(GuardAI thisEnemy, GameObject player)
    {
        //Patrolling
        //Are we at a wait point?
        //If not, move towards a point
        if (!atWaitSpot)
        {
            Vector3 patrolSpot = patrolList[currentSpotIndex].position;
            patrolSpot.y = thisEnemy.transform.position.y;

            //thisEnemy.transform.LookAt(patrolSpot);
            //thisEnemy.transform.position = Vector3.MoveTowards(thisEnemy.transform.position, patrolSpot, Time.deltaTime * thisEnemy.getGuardStats.MovementSpeed);
            thisEnemy.getNavMeshAgent.SetDestination(patrolSpot);
            
            if (thisEnemy.transform.position == patrolSpot)
            {
                atWaitSpot = true;
            }
        }
        else
        {
            if ((patrolList[currentSpotIndex].tag == "Wait") && (waitTimerStarted == false))
            {
                //Debug.Log("Before courtoinue");

                StartCoroutine(waitAtWaitSpot());
                waitTimerStarted = true;
                //Debug.Log("After courtoinue");
                //atWaitSpot = false;
            }
            else if (waitTimerStarted == false)
            {
                currentSpotIndex++;
                if (currentSpotIndex == patrolList.Count)
                {
                    currentSpotIndex = 0;
                }
                atWaitSpot = false;
            }
        }
    }
    private IEnumerator waitAtWaitSpot()
    {
        yield return new WaitForSeconds(5f);
        //Debug.Log("Finished wait timer");
        atWaitSpot = false;
        //Debug.Log("this should only trigger once");
        previousPatrol.text = patrolList[currentSpotIndex].gameObject.name;

        currentSpotIndex++;
        if (currentSpotIndex == patrolList.Count)
        {
            currentSpotIndex = 0;
        }
        currentPatrol.text = patrolList[currentSpotIndex].gameObject.name;
        waitTimerStarted = false;
    }

    private IEnumerator increaseDetection(GuardAI guard)
    {
        guard.getGuardStats.DetectionMeter += 1;
        yield return new WaitForSeconds(1f);
        
        
        if (guard.getGuardStats.DetectionMeter == guard.getGuardStats.DetectionMeterMax)
        {
            //switch state to chase
            //Debug.Log(guard.getGuardStats.DetectionMeter);
            guard.changeState(2);
        }
        detectTimerStarted = false;
    }

    private IEnumerator decreaseDetection(GuardAI guard)
    {
        yield return new WaitForSeconds(1f);
        if (guard.getGuardStats.DetectionMeter > 0)
        {
            guard.getGuardStats.DetectionMeter -= 1;
        }
        detectTimerStarted = false;
    }
}
