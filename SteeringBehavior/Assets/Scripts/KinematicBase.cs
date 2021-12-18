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
        transform.position = kinematic.position;
        transform.rotation = Quaternion.Euler(0, kinematicOutput.outputRotation, 0);
    }
    
    protected float GetNewOrientation(Vector3 currentVelocity)
    {     
        if(currentVelocity.magnitude > 0.1f)
        {
            Debug.Log("current velocity" + currentVelocity.magnitude);
            return Mathf.Atan2(currentVelocity.x, currentVelocity.z) * Mathf.Rad2Deg;
        }
        return 0;
    }

    protected void GetKinematicOutput(Transform targetTransform)
    {
        kinematicOutput.outputVelocity = Vector3.zero;
        kinematicOutput.outputRotation = 0;

        Vector3 dir = (targetTransform.position - transform.position).normalized;
        Vector3 dis2d = targetTransform.position - transform.position;
        kinematicOutput.outputVelocity = dir * maxSpeed;
        kinematicOutput.outputRotation = GetNewOrientation(dis2d);

    }
}
