using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBase
{
    [SerializeField]
    Transform target;
    // Update is called once per frame
    void Update()
    {
        GetSteeringOutput(target);
        ApplyMovement();
    }
}
