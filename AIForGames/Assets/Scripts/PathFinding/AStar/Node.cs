using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node: IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int costSoFar;
    public int costHeuristic;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPos,int _gridX,int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int estimatedTotalCost
    {
        get { 
            return costSoFar + costHeuristic; 
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set 
        {
            heapIndex = value; 
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = estimatedTotalCost.CompareTo(nodeToCompare.estimatedTotalCost);
        if(compare == 0)
        {
            compare = costHeuristic.CompareTo(nodeToCompare.costHeuristic);
        }
        return -compare;
    }
}
