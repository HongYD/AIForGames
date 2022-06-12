using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : SteeringBase
{
    [SerializeField]
    private float targetRadius = 0.1f;
    /// <param name="flockingAgent"> current agent</param>
    /// <param name="neighbours"> neighbours</param>
    protected override SteeringOutput GetSteeringOutput(Transform flockingAgent, List<Transform> neighbours)
    {
        SteeringOutput steeringOutput = new SteeringOutput();
        if (neighbours.Count == 0 || neighbours == null) 
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0f;
            return steeringOutput;
        }

        Vector3 massCenter = new Vector3();
        foreach (Transform t in neighbours)
        {
            massCenter += t.position;
        }
        massCenter /= neighbours.Count;

        if((massCenter- flockingAgent.position).magnitude < targetRadius)
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0f;
            return steeringOutput;
        }

        steeringOutput.linear = (massCenter - flockingAgent.position).normalized * maxAcceleration;
        steeringOutput.angular = 0f;
        return steeringOutput;
    }
}
