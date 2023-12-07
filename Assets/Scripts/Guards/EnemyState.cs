using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iEnemyState
{
    //Used for detection mechanics
    public abstract void Action(GuardAI guard,GameObject player, detectionScript detectionScriptvar);//, int detectionRange, int detectionMeter, int detectionMeterMax, int fieldOfView);
    //Used for movement
    public abstract void Movement(GuardAI thisEnemy, GameObject player);

}
