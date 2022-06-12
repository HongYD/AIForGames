using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationFlocking : SteeringBase
{
    [SerializeField]
    private float threshold = 1.0f;
    [SerializeField]
    private float decayCoefficient = 2.0f;
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

        Vector3 avoidanceForce = new Vector3();
        int numAvoid = 0;
        foreach (Transform t in neighbours)
        {
            if(Vector3.Magnitude(t.position - flockingAgent.position) < threshold)
            {
                numAvoid++;
                float distance = (flockingAgent.position - t.position).magnitude;
                Vector3 forceDirection3D = (flockingAgent.position - t.position);
                Vector3 forceDirection = new Vector3(forceDirection3D.x, 0, forceDirection3D.z);
                avoidanceForce += Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration) * forceDirection;
            }
        }

        if(numAvoid > 0)
            avoidanceForce /= numAvoid;

        steeringOutput.linear = avoidanceForce;
        steeringOutput.angular = 0f;

        return steeringOutput;
    }
}
