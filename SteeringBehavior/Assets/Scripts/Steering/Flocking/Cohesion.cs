using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : SteeringBase
{
    [SerializeField]
    private float targetRadius = 0.1f;
    private float maxAcceleration = 5.0f;
    /// <param name="flockingAgent"> current agent</param>
    /// <param name="neighbours"> neighbours</param>
    public override SteeringOutput GetSteeringOutput(Transform flockingAgent, List<Transform> neighbors)
    {
        SteeringOutput steeringOutput = new SteeringOutput();
        if (neighbors.Count == 0 || neighbors == null) 
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0f;
            return steeringOutput;
        }

        Vector3 massCenter = new Vector3();
        foreach (Transform t in neighbors)
        {
            massCenter += t.position;
        }
        massCenter /= neighbors.Count;

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
