using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    // Start is called before the first frame update
    public float avoidDistance;
    public int lookAhead;
    public LayerMask layerMask;

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

    private Vector3 characterFacing;
    private RaycastHit hit;

    private void Start()
    {
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;
        hit = new RaycastHit();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, characterFacing, out hit, lookAhead, layerMask))
        {
            target.position = hit.point + hit.normal * avoidDistance;
        }

        GetSteeringOutput(target);
        ApplyMovement();
    }

    // Update is called once per frame
    protected override void GetSteeringOutput(Transform target)
    {
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;
        float targetRotation = 0;
        Vector3 targetVelocity = Vector3.zero;

        steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;

        //Facing
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        characterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        characterFacing = characterFacing.normalized;

        //target orientation
        Vector3 targetOrientation = (target.position - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;

        float rotation = Vector3.SignedAngle(characterFacing, targetOrientation, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < targetRadius)
        {
            steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;
            steeringOutput.angular = 0;
            return;
        }
        else if (rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }
        else
        {
            targetRotation = maxRotation * (rotationSize / slowRadius);
        }


        //·½Ïò
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + characterFacing * lookAhead);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hit.point + hit.point + hit.normal * avoidDistance, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hit.point, hit.point + hit.normal * avoidDistance);
        
    }
}
