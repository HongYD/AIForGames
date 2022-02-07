using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : SteeringBase
{
    private List<Transform> playerList;
    public float threshold = 1.0f;
    public float decayCoefficient = 2.0f;

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

    // Start is called before the first frame update
    void Start()
    {
        playerList = new List<Transform>();
        GameObject PlayerList = GameObject.Find("PlayerList");
        int childCount = PlayerList.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            playerList.Add(PlayerList.transform.GetChild(i));
        }

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
        Vector3 direction = Vector3.zero;

        for (int i =0; i < playerList.Count; i++)
        {
            direction = this.transform.position - playerList[i].position;
            if(direction.magnitude <= 0)
            {
                continue;
            }
            Vector3 direction2D = new Vector3(direction.x, 0, direction.z);
            float distance = direction2D.magnitude;
            float strength = 0;
            if (distance < threshold)
            {
                strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
            }
            direction2D += direction2D.normalized;
            steeringOutput.linear += strength * direction2D;
        }
        steeringOutput.linear += (target.position - transform.position).normalized * maxAcceleration;

        //rotation
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;

        Vector3 targetOrientation = (target.position - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;

        //求解两向量间的夹角
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < targetRadius)
        {
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
