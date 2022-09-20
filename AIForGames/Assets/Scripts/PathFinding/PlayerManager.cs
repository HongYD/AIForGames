using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject target;

    public Vector3 originalRightClickPosition;
    public Vector3 newTempRightClickPosition;
    public Vector3 recalculatedTarget;

    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> movingPlayers = new List<GameObject>();
    public List<Vector3> occupiedNodes = new List<Vector3>();
    public List<Vector3> listOfVectors = new List<Vector3>();

    //Path
    Vector3[] path;
    int targetIndex;
    bool pathFindingSuccessful;
    Vector3 currentWayPoint;
    Vector3 currentTargetPos;
    private float speed = 20.0f;

    public FormationManager formationManager;

    public bool buildMode;
    public bool foundClosestFreeNode;
    public bool isTargetChanged;

    public enum PlayerControlState
    {
        rightClickTargetNode,
        calculateMoveArea,
        createFormation,
        clearList,
    }

    public PlayerControlState playerControlState;


    private void Awake()
    {
        instance = this;
        formationManager = new FormationManager();
    }

    private void Start()
    {
        for(int i =0;i< GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            players.Add(GameObject.FindGameObjectsWithTag("Player")[i]);
        }
        formationManager.InitSlotAssignments(players, FormationPatternEnum.Square);
    }

    private void Update()
    {
        UpdatePlayersControlState();
    }

    private void UpdatePlayersControlState()
    {
        switch (playerControlState)
        {
            case PlayerControlState.rightClickTargetNode:
                RightclickTargetNode();
                break;
            case PlayerControlState.calculateMoveArea:
                CreateWalkableArea(originalRightClickPosition);
                break;
            case PlayerControlState.createFormation:
                FindPathForSelectedPlayers();
                //FollowPathAndKeepFormation();
                break;
            case PlayerControlState.clearList:
                listOfVectors.Clear();
                playerControlState = PlayerControlState.rightClickTargetNode;
                break;
        }
    }

    IEnumerator FollowPathAndKeepFormation()
    {       
        if(path.Length > 0 && pathFindingSuccessful)
        {
            currentWayPoint = path[0];
            while (true)
            {
                if(currentTargetPos == currentWayPoint)
                {
                    targetIndex++;
                }
                if(targetIndex < path.Length)
                {
                    currentWayPoint = path[targetIndex];
                    currentTargetPos = Vector3.MoveTowards(currentTargetPos, currentWayPoint, speed * Time.deltaTime);
                }
                else if((targetIndex >= path.Length) && currentTargetPos != Target.target3D)
                {
                    currentTargetPos = Vector3.MoveTowards(currentTargetPos, Target.target3D, speed * Time.deltaTime);
                }
                else
                {
                    yield break;
                }
                yield return null;
            }
        }
    }

    private void FindPathForSelectedPlayers()
    {
        List<GameObject> selcetedPlayers = SelectionManager.instance.currentlySelectedPlayersListInstance;
        //1.We need to calc 1 target for all selected players
        //2.we need to calc 1 Path for all team, because we should keep formation
        //3.we need to calc position offset for all selected players at each frame,
           //that means all selected players should keep formation at each frame,
           //also means the position of target is the "leader" of all selected players
        //4.if any of the selected player's target is unwalkable at any frame, we need to get a new target

        Vector3 initTarget = originalRightClickPosition;
        //Get StartPos
        Vector3 startPos = new Vector3();
        if (selcetedPlayers.Count > 0)
        {
            for (int i = 0; i < selcetedPlayers.Count; i++)
            {
                startPos += Grid.instance.NodeFromWorldPoint(selcetedPlayers[i].transform.position).worldPosition;
            }
            startPos = Grid.instance.NodeFromWorldPoint(startPos /= selcetedPlayers.Count).worldPosition;
        }
        PathRequestManager.instance.RequestPath(startPos, initTarget, OnPathFound, PathFindingTypeEnum.AStar);

        //then we get Path
    }

    private void CreateWalkableArea(Vector3 startPos)
    {
        Vector3 topLeft = new Vector3();
        Vector3 bottomLeft = new Vector3();
        Vector3 topRight = new Vector3();
        Vector3 bottomRight = new Vector3();
        int cornerIncrementer = 1;
        int sideIncrementer = 1;

        for(int i = 0; i < 4; i++)
        {
            topLeft.x = (startPos.x - cornerIncrementer);
            topLeft.z = (startPos.z - sideIncrementer);
            topLeft.y = 1.0f;
            listOfVectors.Add(topLeft);
            EdgeMaker(sideIncrementer, topLeft, 1, 0);
            EdgeMaker(sideIncrementer, topLeft, 0, -1);

            bottomLeft.x = (startPos.x - cornerIncrementer);
            bottomLeft.z = (startPos.z - cornerIncrementer);
            bottomLeft.y = 1.0f;
            listOfVectors.Add(bottomLeft);

            topRight.x = (startPos.x + cornerIncrementer);
            topRight.z = (startPos.z + cornerIncrementer);
            topRight.y = 1.0f;
            listOfVectors.Add(topRight);

            bottomRight.x =(startPos.x - sideIncrementer);
            bottomRight.z = (startPos.z - sideIncrementer);
            bottomRight.y = 1.0f;
            listOfVectors.Add(bottomRight);
            EdgeMaker(sideIncrementer, bottomRight, -1, 0);
            EdgeMaker(sideIncrementer, bottomRight, 0, 1);

            cornerIncrementer++;
            sideIncrementer += 2;
        }

        playerControlState = PlayerControlState.createFormation;
    }

    private void EdgeMaker(int sideIncrementer, Vector3 corner, int x, int z)
    {
        for(int i = 0; i < sideIncrementer; i++)
        {
            if (i == 0)
            {
                Vector3 temp = corner;
                temp.x += x;
                temp.z += z;
                newTempRightClickPosition = temp;
                listOfVectors.Add(newTempRightClickPosition);
            }
            else
            {
                Vector3 thirdTemp = newTempRightClickPosition;
                thirdTemp.x += x;
                thirdTemp.z += z;
                newTempRightClickPosition = thirdTemp;
                listOfVectors.Add(newTempRightClickPosition);
            }
        }
    }

    private void RightclickTargetNode()
    {
        if (isTargetChanged)
        {
            originalRightClickPosition = Grid.instance.NodeFromWorldPoint(Target.target3D).worldPosition;
            RemoveFromTargetsAndMovingUnits();
            playerControlState = PlayerControlState.calculateMoveArea;
        }
    }

    private void RemoveFromTargetsAndMovingUnits()
    {
        if (SelectionManager.instance.currentlySelectedPlayersListInstance.Count > 0)
        {
            for(int i =0;i<SelectionManager.instance.currentlySelectedPlayersListInstance.Count; i++)
            {
                SelectionManager.instance.currentlySelectedPlayersListInstance[i].GetComponent<PathFindingMovement>().RemoveUnitFromUnitManagerMovingUnitsList();
            }
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            pathFindingSuccessful = true;
            //We have already found the path
            isTargetChanged = false;
        }
    }
}
