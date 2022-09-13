using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingMovement : MonoBehaviour
{
	public Transform target;
	float speed = 10;
	Vector3[] path;
	int targetIndex;
	void Start()
	{
		PathRequestManager.instance.RequestPath(transform.position, target.position, OnPathFound, PathFindingTypeEnum.AStar);
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
				if (targetIndex >= path.Length)
				{
					yield break;
				}
				currentNode = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position, currentNode, speed * Time.deltaTime);
			yield return null;

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
