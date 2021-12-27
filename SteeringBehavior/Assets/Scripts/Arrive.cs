using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBase
{
    [SerializeField]
    Transform target;
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private float slowRadius;
    [SerializeField]
    private float timeToTarget;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        GetSteeringOutput(target);
        ApplyMovement();
    }

    protected override void GetSteeringOutput(Transform target)
    {
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;
        float targetSpeed = 0;
        Vector3 targetVelocity = Vector3.zero;

        Vector3 direction = (target.position - transform.position).normalized;
        float distance = (target.position - transform.position).magnitude;


        if(distance < targetRadius)
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0;
        }
        else if(distance < slowRadius)
        {
            targetSpeed = maxSpeed * (distance / slowRadius);
        }
        else
        {
            targetSpeed = maxSpeed;
        }
        targetVelocity = direction * targetSpeed;

        steeringOutput.linear = (targetVelocity - kinematic.velocity) / timeToTarget;
        if(steeringOutput.linear.magnitude > maxAcceleration)
        {
            steeringOutput.linear = steeringOutput.linear.normalized * maxAcceleration;
        }
        steeringOutput.angular = 0;
    }
}
