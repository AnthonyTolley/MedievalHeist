using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : MonoBehaviour, iEnemyState
{
    private bool detectTimerStarted = false;
    public void Action(GuardAI guard, GameObject player, detectionScript detectionScriptvar)//, int detectionRange, int detectionMeter, int detectionMeterMax, int fieldOfView)
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        if (detectionScriptvar.getPlayerDetected && !detectTimerStarted)
        {
            if (!Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, Vector3.Distance(guard.transform.position, player.transform.position), layerMask))
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
    }

    public void Movement(GuardAI thisEnemy, GameObject player)
    {
        thisEnemy.getNavMeshAgent.SetDestination(player.transform.position);
        //thisEnemy.transform.position = Vector3.MoveTowards(thisEnemy.transform.position, player.transform.position, Time.deltaTime * thisEnemy.getGuardStats.MovementSpeed);
    }
    private IEnumerator increaseDetection(GuardAI guard)
    {
        guard.getGuardStats.DetectionMeter += 1;
        yield return new WaitForSeconds(1f);


        if (guard.getGuardStats.DetectionMeter == guard.getGuardStats.DetectionMeterMax)
        {
            //switch state to chase
            //Debug.Log(guard.getGuardStats.DetectionMeter);
            //guard.changeState(2);
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
        else
        {
            guard.changeState(0);
        }
        detectTimerStarted = false;
    }
}
