using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment : SteeringBase
{
    [SerializeField]
    private float maxAngularAcceleration = 90.0f;
    [SerializeField]
    private float maxRotation = 5.0f;
    [SerializeField]
    private float targetAngleDiff = 0.1f;
    [SerializeField]
    private float slowDownAngleDiff = 1.5f;
    [SerializeField]
    private float timeToTarget = 0.1f;
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

        Vector3 averageFacing = new Vector3();
        foreach (Transform t in neighbors)
        {
            averageFacing += GetFacing(t);
        }
        averageFacing /= neighbors.Count;
        averageFacing = averageFacing.normalized;

        Vector3 currentFacing = GetFacing(flockingAgent);
        float rotation = Vector3.SignedAngle(currentFacing, averageFacing, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);
        float targetRotation;

        if (rotationSize < targetAngleDiff)
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0;
            return steeringOutput;
        }
        else if (rotationSize > slowDownAngleDiff)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * (rotationSize / slowDownAngleDiff);
        }

        targetRotation *= (rotation / rotationSize);
        steeringOutput.angular = targetRotation - kinematic.rotation;
        steeringOutput.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(steeringOutput.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            steeringOutput.angular /= angularAcceleration;
            steeringOutput.angular *= maxAngularAcceleration;
        }

        steeringOutput.linear = Vector3.zero;
        return steeringOutput;
    }

    protected override Vector3 GetFacing(Transform agent)
    {
        Vector3 childPosition = agent.GetChild(0).position;
        Vector3 faceDir3D = (childPosition - agent.position);
        Vector3 faceDir = new Vector3(faceDir3D.x, 0, faceDir3D.z);
        faceDir = faceDir.normalized;
        return faceDir;
    }
}
