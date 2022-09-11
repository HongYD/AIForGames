using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAStar : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node goalNode = grid.NodeFromWorldPoint(endPosition);

        List<Node> openNodes = new List<Node>();
        HashSet<Node> closeNodes = new HashSet<Node>();
        openNodes.Add(startNode);

        while(openNodes.Count > 0)
        {
            Node node = openNodes[0];
            //Find the smallest element in the open list
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].estimatedTotalCost <= node.estimatedTotalCost)
                {
                    if(openNodes[i].costHeuristic < node.costHeuristic)
                    {
                        node = openNodes[i];
                    }
                }
            }

            openNodes.Remove(node);
            closeNodes.Add(node);
            if(node == goalNode)
            {
                RetracePath(startNode, goalNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(node))
            {
                if(!neighbour.walkable || closeNodes.Contains(neighbour))
                {
                    continue;
                }
                else
                {
                    int newCostToNeighbour = node.costSoFar + GetCost(node, neighbour);
                    if(newCostToNeighbour < neighbour.costSoFar || !openNodes.Contains(neighbour))
                    {
                        neighbour.costSoFar = newCostToNeighbour;
                        neighbour.costHeuristic = GetCost(node,goalNode);
                        neighbour.parent = node;

                        if (!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    //Heuristic使用Manhattan Distance曼哈顿距离，因为欧式距离要开根号开销大
    private int GetCost(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY)
        {
            return 14 * distY + 10 * distX;
        }
        else
        {
            return 14 * distX + 10 * distY;
        }
    }

    private void RetracePath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        grid.path = path;
    }
}
