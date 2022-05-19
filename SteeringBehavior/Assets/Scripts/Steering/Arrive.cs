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


    //tor
    [SerializeField]
    private float targetRadiusRot;
    [SerializeField]
    private float slowRadiusRot;
    [SerializeField]
    private float timeToTargetRot;
    [SerializeField]
    private float maxAngularAcceleration;
    [SerializeField]
    private float maxRotation;
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
        float targetRotation = 0;
        float targetSpeed = 0;
        Vector3 targetVelocity = Vector3.zero;


        steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;

        //facing,rot
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;

        Vector3 targetOrientation = (target.position - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;
        //求解两向量间的夹角
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);


        Vector3 direction = (target.position - transform.position).normalized;
        float distance = (target.position - transform.position).magnitude;


        if (rotationSize < targetRadiusRot)
        {
            steeringOutput.linear = Vector3.zero;
            steeringOutput.angular = 0;
            return;
        }
        else if(rotationSize > slowRadiusRot)
        {
            if (distance < targetRadius)
            {
                steeringOutput.linear = Vector3.zero;
                steeringOutput.angular = 0;
                return;
            }
            else if (distance < slowRadius)
            {
                targetSpeed = maxSpeed * (distance / slowRadius);
            }
            else
            {
                targetSpeed = maxSpeed;
            }
            targetRotation = maxRotation;
        }
        else
        {
            if (distance < targetRadius)
            {
                steeringOutput.linear = Vector3.zero;
                steeringOutput.angular = 0;
                return;
            }
            else if (distance < slowRadius)
            {
                targetSpeed = maxSpeed * (distance / slowRadius);
            }
            else
            {
                targetSpeed = maxSpeed;
            }
            targetRotation = maxRotation * (rotationSize / slowRadius);
        }

        targetRotation *= (rotation / rotationSize);
        targetVelocity = direction * targetSpeed;

        steeringOutput.angular = targetRotation - kinematic.rotation;
        steeringOutput.angular /= timeToTargetRot;

        steeringOutput.linear = (targetVelocity - kinematic.velocity) / timeToTarget;
        if(steeringOutput.linear.magnitude > maxAcceleration)
        {
            steeringOutput.linear = steeringOutput.linear.normalized * maxAcceleration;
        }

        float angularAcceleration = Mathf.Abs(steeringOutput.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            steeringOutput.angular /= angularAcceleration;
            steeringOutput.angular *= maxAngularAcceleration;
        }
        //steeringOutput.angular = 0;
    }
}
