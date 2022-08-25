using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationFlocking : SteeringBase
{
    [SerializeField]
    private float threshold = 2.5f;
    [SerializeField]
    private float decayCoefficient = 5.0f;
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

        Vector3 avoidanceForce = new Vector3();
        int numAvoid = 0;
        foreach (Transform t in neighbors)
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
