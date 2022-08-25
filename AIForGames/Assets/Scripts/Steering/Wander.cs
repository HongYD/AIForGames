using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wander = Align + Seek
public class Wander : SteeringBase
{
    float _changeDirTime = 8.0f;
    float changeDirTime;

    [Range(5, 10)]
    public int rd;

    [Range(10, 20)]
    public int forwardVec;

    Vector3 direction;
    Vector3 center;

    [SerializeField]
    Transform target;
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private float slowRadius;
    [SerializeField]
    private float timeToTarget;
    [SerializeField]
    private float maxAngularAcceleration;
    [SerializeField]
    private float maxRotation;

    private void Start()
    {
        base.Start();
        direction = Random.insideUnitSphere.normalized;
        changeDirTime = _changeDirTime;
    }

    private void Update()
    {
        changeDirTime -= Time.deltaTime;
        center = transform.position + (transform.up * forwardVec);

        if(changeDirTime <= 0)
        {
            direction = Random.insideUnitSphere.normalized;
            changeDirTime = _changeDirTime;
        }
        Vector3 newPosOfTarget = center + (direction * rd);
        target.position = new Vector3(newPosOfTarget.x, 0, newPosOfTarget.z);

        GetSteeringOutput(target);
        ApplyMovement();
    }

    protected override void GetSteeringOutput(Transform target)
    {
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;
        float targetRotation = 0;
        Vector3 targetVelocity = Vector3.zero;

        steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;

        Vector3 targetOrientation = (target.position - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;

        //求解两向量间的夹角
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if(rotationSize < targetRadius)
        {
            steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;
            steeringOutput.angular = 0;
            return;
        }
        else if(rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * (rotationSize / slowRadius);
        }

        //方向
        targetRotation *= (rotation / rotationSize);

        steeringOutput.angular = targetRotation - kinematic.rotation;
        steeringOutput.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(steeringOutput.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            steeringOutput.angular /= angularAcceleration;
            steeringOutput.angular *= maxAngularAcceleration;
        }
    }
}
