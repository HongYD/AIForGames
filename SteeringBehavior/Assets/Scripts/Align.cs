using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBase
{
    [SerializeField]
    protected Transform target;
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
        Vector3 targetVelocity = Vector3.zero;

        //定义一个面向,就是character位置到鼻子位置的朝向；Todo:把面向整合到人物属性当中
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;


        //定义一个target orientation,就是从character到target向量的方向
        Vector3 targetOrientation = (target.position - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;

        //求解两向量间的夹角
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation,Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if(rotationSize < targetRadius)
        {
            steeringOutput.linear = Vector3.zero;
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

        targetRotation *= (rotation / rotationSize);

        steeringOutput.angular = targetRotation - kinematic.rotation;
        steeringOutput.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(steeringOutput.angular);
        if(angularAcceleration > maxAngularAcceleration)
        {
            steeringOutput.angular /= angularAcceleration;
            steeringOutput.angular *= maxAngularAcceleration;
        }

        steeringOutput.linear = Vector3.zero;
    }
}
