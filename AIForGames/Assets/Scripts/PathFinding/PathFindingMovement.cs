using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingMovement : MonoBehaviour
{
	public Transform target;
	public float speed = 10;
	//public bool isTargetChanged = false;
	Vector3[] path;
	int targetIndex;

	void Start()
	{
		
	}

    private void Update()
    {
  //      if(target != null && isTargetChanged)
  //      {
		//	PathRequestManager.instance.RequestPath(transform.position, target.position, OnPathFound, PathFindingTypeEnum.AStar);
		//	isTargetChanged = false;
		//}
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentNode = path[0];
		while (true)
		{
			if (transform.position == currentNode)
			{
				targetIndex++;		
			}
			if (targetIndex < path.Length)
			{
				currentNode = path[targetIndex];
				transform.position = Vector3.MoveTowards(transform.position, currentNode, speed * Time.deltaTime);
				UpdateFacing(currentNode);				
			}
			else if ((targetIndex >= path.Length) && transform.position != target.position)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
				UpdateFacing(target.position);				
			}
            else
            {
				yield break;
			}
			yield return null;
		}
	}

	void UpdateFacing(Vector3 traget)
    {
		Vector2 targetDir = (target.position - transform.position).normalized.ToVector2();
		if ((target.position - transform.position).magnitude > 0.1f)
		{
			this.transform.rotation = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.y));
		}
	}

	public void RemoveUnitFromUnitManagerMovingUnitsList()
	{
		if (PlayerManager.instance.movingPlayers.Count > 0)
		{
			for (int i = 0; i < PlayerManager.instance.movingPlayers.Count; i++)
			{
				if (this.gameObject == PlayerManager.instance.movingPlayers[i])
				{
					PlayerManager.instance.movingPlayers.Remove(PlayerManager.instance.movingPlayers[i]);
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}
