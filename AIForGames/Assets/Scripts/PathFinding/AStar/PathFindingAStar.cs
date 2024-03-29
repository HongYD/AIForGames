using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAStar : PathFindingBase
{
    public override IEnumerator FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3[] pathNodes = new Vector3[0];
        bool pathFindingSuccess = false;

        Node startNode = Grid.instance.NodeFromWorldPoint(startPosition);
        Node goalNode = Grid.instance.NodeFromWorldPoint(endPosition);

        if (startNode.walkable && goalNode.walkable)
        {
            MinHeap<Node> openNodes = new MinHeap<Node>(Grid.instance.MaxSize);
            HashSet<Node> closeNodes = new HashSet<Node>();
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                Node node = openNodes.RemoveFirst();
                closeNodes.Add(node);

                if (node == goalNode)
                {
                    pathFindingSuccess = true;
                    break;
                }

                foreach (Node neighbour in Grid.instance.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closeNodes.Contains(neighbour))
                    {
                        continue;
                    }
                    else
                    {
                        int newCostToNeighbour = node.costSoFar + GetHeuristicCost(node, neighbour);
                        if (newCostToNeighbour < neighbour.costSoFar || !openNodes.Contains(neighbour))
                        {
                            neighbour.costSoFar = newCostToNeighbour;
                            neighbour.costHeuristic = GetHeuristicCost(node, goalNode);
                            neighbour.parent = node;

                            if (!openNodes.Contains(neighbour))
                            {
                                openNodes.Add(neighbour);
                            }
                            else
                            {
                                openNodes.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathFindingSuccess)
        {
            pathNodes = RetracePath(startNode, goalNode);
        }
        PathRequestManager.instance.FinishedProcessingPath(pathNodes, pathFindingSuccess, PathFindingTypeEnum.AStar);      
    }

    //Heuristic使用Manhattan Distance曼哈顿距离，因为欧式距离要开根号开销大
    private int GetHeuristicCost(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    private Vector3[] RetracePath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //path.Add(startNode);
        Vector3[] pathNodes = SimplifyPath(path,startNode,goalNode);
        Array.Reverse(pathNodes);
        return pathNodes;
    }

    private Vector3[] SimplifyPath(List<Node> path, Node startNode, Node goalNode)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition + new Vector3(0,1.0f,0));
            }
            directionOld = directionNew;
        }
        if (waypoints.Count <= 0)
        {
            if (!waypoints.Contains(startNode.worldPosition + new Vector3(0, 1.0f, 0)))
            {
                waypoints.Add(startNode.worldPosition + new Vector3(0, 1.0f, 0));
            }
            if (!waypoints.Contains(goalNode.worldPosition + new Vector3(0, 1.0f, 0)))
            {
                waypoints.Add(goalNode.worldPosition + new Vector3(0, 1.0f, 0));
            }
        }
        return waypoints.ToArray();
    }
}
