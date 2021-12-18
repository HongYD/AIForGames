using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBase : MonoBehaviour
{
    struct Kinematic
    {
        Vector3 position;
        float orientation;//����
        Vector3 velocity;
        float rotation;//���ٶ�
    }

    struct SteeringOutput
    {
        Vector3 linear;
        float angular;
    }
}
