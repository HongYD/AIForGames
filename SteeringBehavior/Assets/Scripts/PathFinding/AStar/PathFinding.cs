using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    [SerializeField]
    public List<Node> path;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        path = new List<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            //找出openSet中fcost+gcost最小的node
            Node node = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if(openSet[i].hCost < node.hCost)
                    {
                        //node现在是openSet中Cost最小的
                        node = openSet[i];
                    }
                }
            }

            //已经访问过node了，需要把Node从OpenSet种移除，同时放入closedSet里
            openSet.Remove(node);
            closedSet.Add(node);

            //如果当前node和targetNode相等，寻路结束，通过RetracePath获得路径
            if(node == targetNode)
            {
                RetracePath(startNode,targetNode);
                return;
            }

            //更新neighbour的gcost和hcost
            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                //如果该邻居不可访问或者已经访问过，那么跳过这个邻居
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour,targetNode);
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        path.Clear();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
