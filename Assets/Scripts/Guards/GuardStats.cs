using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStats
{
    private int fieldOfView = 90; //unused
    private int detectionRange = 10; //unused
    private int detectionMeter = 0;
    private int detectionMeterMax = 10; //this is seconds the player spends being detected before theyre found
    private int movementSpeed = 5;
    private int sprintSpeed = 8;

    private iEnemyState currentState;

    private List<Component> stateList = new List<Component>();
    public int FieldOfView { get => fieldOfView;}
    public int DetectionMeter
    {
        get => detectionMeter; 
        set
        {
            if (value >= detectionMeterMax)
            {
                detectionMeter = detectionMeterMax;
            }
            else
            {
                detectionMeter = value;
            }
        }
    }
    public int DetectionMeterMax { get => detectionMeterMax;}
    public int MovementSpeed { get => movementSpeed;}
    public int SprintSpeed { get => sprintSpeed;}
    public int DetectionRange { get => detectionRange;}
}
