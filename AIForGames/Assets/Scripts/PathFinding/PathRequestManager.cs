using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PathFindingTypeEnum
{
    AStar,
    Dijkstra,
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[],bool> callback;
    PathFindingTypeEnum pathFindingType;


    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback, PathFindingTypeEnum _pathFindingType)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
        pathFindingType = _pathFindingType;
    }
}

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest> ();
    PathRequest currentPathRequest;
    public static PathRequestManager instance;
    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
    }

    public void RequestPath(Vector3 startNode, Vector3 GoalNode, Action<Vector3[], bool> callback, PathFindingTypeEnum pathFindingType)
    {
        PathRequest pathRequest = new PathRequest (startNode, GoalNode, callback, pathFindingType);
        instance.pathRequestQueue.Enqueue (pathRequest);
        instance.TryProcessNext(pathFindingType);
    }

    private void TryProcessNext(PathFindingTypeEnum pathFindingType)
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue ();
            isProcessingPath = true;
            if(pathFindingType == PathFindingTypeEnum.AStar)
            {
                
            }
            else if(pathFindingType == PathFindingTypeEnum.Dijkstra)
            {

            }
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success, PathFindingTypeEnum pathFindingType)
    {
        currentPathRequest.callback(path,success);
        isProcessingPath = false;
        TryProcessNext(pathFindingType);
    }
}
