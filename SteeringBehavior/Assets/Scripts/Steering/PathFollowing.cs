using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : SteeringBase
{
    // Start is called before the first frame update
    private List<Node> path;
    public int pathOffSet;
    public Vector2 currentPosOnPath;
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

    void Start()
    {
        path = GameObject.Find("AStar").GetComponent<PathFinding>().path;
    }

    // Update is called once per frame
    void Update()
    {
        path = GameObject.Find("AStar").GetComponent<PathFinding>().path;
        GetSteeringOutput(target);
        ApplyMovement();

    }

    protected override void GetSteeringOutput(Transform target)
    {
        steeringOutput.linear = Vector3.zero;
        steeringOutput.angular = 0;
        float targetRotation = 0;
        Vector3 targetVelocity = Vector3.zero;

        int curIndexInPath = 0;
        Node currentNode = PathNodeFromWorldPoint(this.transform.position, out curIndexInPath);
        int newIndexInPath = curIndexInPath + pathOffSet;
        Vector3 newTarget = this.transform.position;
        if(newIndexInPath > path.Count - 1 && newIndexInPath > curIndexInPath && newIndexInPath > 0 && path.Count > 0)
        {
            newIndexInPath = path.Count - 1;
            newTarget = path[newIndexInPath].worldPosition;
        }
        


        steeringOutput.linear = (newTarget - transform.position).normalized * maxAcceleration;
        Vector3 characterFacing3D = transform.GetChild(0).transform.position - transform.position;
        Vector3 charcterFacing = new Vector3(characterFacing3D.x, 0, characterFacing3D.z);
        charcterFacing = charcterFacing.normalized;

        Vector3 targetOrientation = (newTarget - transform.position);
        targetOrientation = new Vector3(targetOrientation.x, 0, targetOrientation.z).normalized;

        //求解两向量间的夹角
        float rotation = Vector3.SignedAngle(charcterFacing, targetOrientation, Vector3.up);
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

    public Node PathNodeFromWorldPoint(Vector3 worldPosition, out int minNodeIndex)
    {
        if(path != null && path.Count > 0)
        {
            float minDist = float.MaxValue;
            minNodeIndex = 0;
            //Todo: use binary search
            for(int i = 0; i < path.Count; i++)
            {
                if(Vector3.Distance(worldPosition,path[i].worldPosition) < minDist)
                {
                    minNodeIndex = i;
                }
            }
            return path[minNodeIndex];
        }
        minNodeIndex = 0;
        return null;
    }
}
