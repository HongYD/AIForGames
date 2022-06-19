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
        steeringOutput.linear += (target.position - transform.position).normalized * maxAcceleration;

        steeringOutput.angular = output.angular + output1.angular + output2.angular;

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
