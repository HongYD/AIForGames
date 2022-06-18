using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flocking : SteeringBase
{
    CapsuleCollider capsuleCollider;
    public CapsuleCollider CapsuleCollider
    {
        get { return capsuleCollider; }
    }
    List<Transform> neighbors = new List<Transform> ();

    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;


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

    }

    protected override void GetSteeringOutput(Transform target)
    {
        base.GetSteeringOutput(target);
    }
}
