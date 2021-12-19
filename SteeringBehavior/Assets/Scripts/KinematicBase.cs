using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBase : MonoBehaviour
{

    public struct Kinematic
    {
        public Vector3 position;
        public float orientation;
        public Vector3 velocity;
        public float rotation;
    }

    public struct KinematicOutput
    {
        public Vector3 outputVelocity;
        public float outputRotation;
    }

    protected Kinematic kinematic;
    protected KinematicOutput kinematicOutput;
    [SerializeField]
    protected float maxSpeed;
    protected float timeToTarget = 0.25f;
    protected float radius = 1.0f;

    protected void Start()
    {
        kinematic.position = Vector3.zero;
        kinematic.orientation = 0;
        kinematic.velocity = Vector3.zero;
        kinematic.rotation = 0;
        kinematicOutput.outputVelocity = Vector3.zero;
        kinematicOutput.outputRotation = 0;
    }

    protected void ApplyMovement()
    {
        kinematic.position += kinematicOutput.outputVelocity * Time.deltaTime;
        transform.position = new Vector3(kinematic.position.x, 1.0f, kinematic.position.z);
        transform.rotation = Quaternion.Euler(0, kinematicOutput.outputRotation, 0);
    }
    
    protected float GetNewOrientation(Vector3 currentVelocity)
    {
        return Mathf.Atan2(currentVelocity.x, currentVelocity.z) * Mathf.Rad2Deg;
    }

    protected virtual void GetKinematicOutput(Transform targetTransform)
    {
        kinematicOutput.outputVelocity = Vector3.zero;
        kinematicOutput.outputRotation = 0;

        Vector3 dir = (targetTransform.position - transform.position).normalized;
        Vector3 dis2d = targetTransform.position - transform.position;
        if (dis2d.magnitude < radius)
        {
            kinematicOutput.outputVelocity = Vector3.zero;
            kinematicOutput.outputRotation = 0;
        }
        else
        {
            kinematicOutput.outputVelocity =  Vector3.ClampMagnitude(((dir * maxSpeed) / timeToTarget),maxSpeed);
            kinematicOutput.outputRotation =  GetNewOrientation(kinematicOutput.outputVelocity);
            Debug.Log("curent speed: " + kinematicOutput.outputVelocity.magnitude);
        }
    }
}
