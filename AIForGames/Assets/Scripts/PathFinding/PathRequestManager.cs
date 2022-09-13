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

public class PathRequestManager: MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest> ();
    PathRequest currentPathRequest;
    public static PathRequestManager instance;
    PathFindingBase pathFinding;
    bool isProcessingPath;
    Grid grid;

    void Awake()
    {
        instance = this;
        grid = GetComponent<Grid>();
    }

    public void RequestPath(Vector3 startNode, Vector3 goalNode, Action<Vector3[], bool> callback, PathFindingTypeEnum pathFindingType)
    {
        PathRequest pathRequest = new PathRequest (startNode, goalNode, callback, pathFindingType);
        instance.pathRequestQueue.Enqueue (pathRequest);
        instance.TryProcessNext(pathFindingType);
    }

    private void TryProcessNext(PathFindingTypeEnum pathFindingType)
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue ();
            isProcessingPath = true;
            StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, pathFindingType);
        }
    }

    public void StartFindPath(Vector3 startPos, Vector3 goalPos, PathFindingTypeEnum pathFindingType)
    {
        if (pathFindingType == PathFindingTypeEnum.AStar)
        {
            pathFinding = new PathFindingAStar();
            StartCoroutine(pathFinding.FindPath(startPos, goalPos, grid));
        }
        else if(pathFindingType == PathFindingTypeEnum.Dijkstra)
        {

        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success, PathFindingTypeEnum pathFindingType)
    {
        currentPathRequest.callback(path,success);
        isProcessingPath = false;
        TryProcessNext(pathFindingType);
    }
}
