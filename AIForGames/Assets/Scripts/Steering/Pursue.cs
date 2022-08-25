using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    [SerializeField]
    float maxPrediction;
    Transform farTarget;
    Vector3 preTargetPosition;

    void Start()
    {
        farTarget = base.target;
    }

    // Update is called once per frame
    void Update()
    {
        GetSteeringOutput(base.target);
        ApplyMovement();
    }

    protected override void GetSteeringOutput(Transform target)
    {
        float prediction = 0;
        Vector3 direction3D = (target.position - transform.position);
        float distance = new Vector3(direction3D.x, 0, direction3D.z).magnitude;
        Vector3 direction = direction3D.normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        

        float curSpeed = kinematic.velocity.magnitude;
        if(curSpeed < distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / curSpeed;
        }
        Vector3 targetVelocity = (target.position - preTargetPosition) * Time.deltaTime;
        farTarget.position = target.position + (targetVelocity * prediction).magnitude * direction;
        preTargetPosition = target.position;
        base.GetSteeringOutput(farTarget);
    }
}
