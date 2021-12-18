using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBase : MonoBehaviour
{
    struct Kinematic
    {
        Vector3 position;
        float orientation;//面向
        Vector3 velocity;
        float rotation;//角速度
    }

    struct SteeringOutput
    {
        Vector3 linear;
        float angular;
    }
}
