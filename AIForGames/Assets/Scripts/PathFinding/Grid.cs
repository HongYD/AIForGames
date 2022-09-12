using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    private Vector2 gridWorldSize;
    private float nodeDiameter;
    private int gridXNodeNum, gridYNodeNum;
    public float nodeRadius;
    Node[,] grid;

    private void Awake()
    {
        gridWorldSize = new Vector2(this.transform.lossyScale.x * 10, this.transform.lossyScale.z * 10);
        nodeDiameter = nodeRadius * 2;
        gridXNodeNum = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridYNodeNum = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get { return gridXNodeNum * gridYNodeNum; }
    }

    private void CreateGrid()
    {
        grid = new Node[gridXNodeNum, gridYNodeNum];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for(int x = 0; x< gridXNodeNum; x++)
        {
            for(int y = 0; y< gridYNodeNum; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    private void UpdateWalkable()
    {

    }

    public List<Node>GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = - 1; x <= 1; x++)
        {
            for(int y= -1; y <= 1; y++)
            {
                if(x==0  && y == 0)
                {
                    continue;
                }
                else
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if(checkX >=0 && checkX < gridXNodeNum && checkY >=0 && checkY < gridYNodeNum)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridXNodeNum - 1) * percentX);
        int y = Mathf.RoundToInt((gridYNodeNum - 1) * percentY);
        return grid[x, y];
    }
}
