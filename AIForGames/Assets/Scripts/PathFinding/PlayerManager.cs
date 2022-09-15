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

    public FormationManager formationManager;

    public bool buildMode;
    public bool foundClosestFreeNode;

    public enum PlayerControlState
    {
        rightClickTargetNode,
        calculateMoveArea,
        createFormation,
        cclearList,
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
                break;
            case PlayerControlState.calculateMoveArea:
                break;
            case PlayerControlState.createFormation:
                break;
            case PlayerControlState.cclearList:
                break;
        }
    }
}
