using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBase : MonoBehaviour
{
    public struct Kinematic
    {
        public Vector3 position;
        public float orientation;//面向
        public Vector3 velocity;
        public float rotation;//角速度
    }

    public struct SteeringOutput
    {
        public Vector3 linear;
        public float angular;
    }

    protected Kinematic kinematic;
    protected SteeringOutput steeringOutput;
    [SerializeField]
    protected float maxSpeed;
    [SerializeField]
    protected float maxAcceleration;

    protected void Start()
    {
        kinematic.position = transform.position;
        kinematic.orientation = 0;
        kinematic.velocity = Vector3.zero;
        kinematic.rotation = 0;
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;
    }

    protected virtual Vector3 GetFacing(Transform agent)
    {
        return new Vector3();
    }

    protected virtual void GetSteeringOutput(Transform target)
    {
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;

        steeringOutput.linear = (target.position - transform.position).normalized * maxAcceleration;
        steeringOutput.angular = 0;
    }

    protected virtual void GetSteeringOutput()
    {

    }

    public virtual SteeringOutput GetSteeringOutput(Transform angent, List<Transform> neighbors)
    {
        return new SteeringOutput();
    }
    protected virtual void ApplyMovement()
    {
        kinematic.position += kinematic.velocity * Time.deltaTime;
        kinematic.orientation += kinematic.rotation * Time.deltaTime;

        kinematic.velocity += steeringOutput.linear * Time.deltaTime;
        kinematic.rotation += steeringOutput.angular * Time.deltaTime;

        transform.position = new Vector3(kinematic.position.x, 2.42f, kinematic.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, kinematic.orientation, 0));

        if (kinematic.velocity.magnitude > maxSpeed)
        {
            kinematic.velocity = kinematic.velocity.normalized * maxSpeed;
        }
    }
}
