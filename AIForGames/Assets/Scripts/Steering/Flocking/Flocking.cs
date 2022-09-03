using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flocking : SteeringBase
{
    public Transform target;
    CapsuleCollider capsuleCollider;
    public LayerMask player;
    public CapsuleCollider CapsuleCollider
    {
        get { return capsuleCollider; }
    }
    List<Transform> neighbors = new List<Transform> ();

    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 10f)]
    public float alignmentFactor = 1.5f;
    [Range(0f, 10f)]
    public float cohesionFactor = 1.5f;
    [Range(0f, 10f)]
    public float separationFlockingFactor = 1.5f;
    [Range(0f, 10f)]
    public float seekFactor = 1.5f;
    [Range(0f, 10f)]
    public float alignFactor = 1.5f;

    [SerializeField]
    private float slowAngleDiff;
    [SerializeField]
    private float targetAngleDiff;
    [SerializeField]
    private float timeToTarget;
    [SerializeField]
    private float maxAngularAcceleration = 90.0f;
    [SerializeField]
    private float maxRotation = 5.0f;


    Alignment alignment;
    Cohesion cohesion;
    SeparationFlocking separationFlocking;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        capsuleCollider = GetComponent<CapsuleCollider>();
        alignment = new Alignment();
        cohesion = new Cohesion();
        separationFlocking = new SeparationFlocking();
    }

    // Update is called once per frame
    void Update()
    {
        GetSteeringOutput(target);
        ApplyMovement();
    }

    protected override void GetSteeringOutput(Transform target)
    {
        neighbors = GetNearbyObjects();
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
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < targetAngleDiff)
        {
            targetRotation = 0;
        }
        else if (rotationSize > slowAngleDiff)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * (rotationSize / slowAngleDiff);
        }

        if(rotationSize == 0)
        {
            rotationSize = 0.01f;
        }
        //方向
        targetRotation *= (rotation / rotationSize);

        targetRotation = targetRotation - kinematic.rotation;
        targetRotation /= timeToTarget;

        float angularAcceleration = Mathf.Abs(targetRotation);
        if (angularAcceleration > maxAngularAcceleration)
        {
            targetRotation /= angularAcceleration;
            targetRotation *= maxAngularAcceleration;
        }

        //TODO:refactor
        SteeringOutput output = new SteeringOutput();
        output = alignment.GetSteeringOutput(this.transform, neighbors);
        output.linear *= alignmentFactor;
        output.angular *= alignmentFactor;

        SteeringOutput output1 = new SteeringOutput();
        output1 = cohesion.GetSteeringOutput(this.transform, neighbors);
        output1.linear *= cohesionFactor;
        output1.angular *= cohesionFactor;

        SteeringOutput output2 = new SteeringOutput();
        output2 = separationFlocking.GetSteeringOutput(this.transform, neighbors);
        output2.linear *= separationFlockingFactor;
        output2.angular *= separationFlockingFactor;

        steeringOutput.linear = output.linear + output1.linear + output2.linear;
        steeringOutput.linear += (target.position - transform.position).normalized * maxAcceleration * seekFactor;

        steeringOutput.angular = output.angular + output1.angular + output2.angular;
        steeringOutput.angular += targetRotation * alignFactor;
    }

    private List<Transform> GetNearbyObjects()
    {
        List<Transform> nearbyObjects = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, neighborRadius, player);
        foreach (Collider collider in colliders)
        {
            if(collider.name != capsuleCollider.name)
            {
                nearbyObjects.Add(collider.transform);
            }
        }
        return nearbyObjects;
    }
}
