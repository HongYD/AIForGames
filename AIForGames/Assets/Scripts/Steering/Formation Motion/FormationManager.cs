using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SlotAssignment
{
    public GameObject player;
    public int slotNumber;
}
public class FormationManager : SteeringBase
{
    public List<SlotAssignment> slotAssignments;
    public Kinematic driftOffset;
    //public DefensiveCirclePattern circlePattern;
    public SquarePattern squarePattern;
    private List<GameObject> selectedPlayers;

    private void Start()
    {
        selectedPlayers = GameObject.Find("GameManager").GetComponent<SelectionManager>().currentlySelectedPlayersListInstance;
        GameObject[] playerList = new GameObject[selectedPlayers.Count];
        if (selectedPlayers != null && selectedPlayers.Count > 0)
        {
            playerList = selectedPlayers.ToArray();
        }
        else
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
        }
        slotAssignments = new List<SlotAssignment>();
        squarePattern = new SquarePattern();
        squarePattern.characterRadius = 1.0f;
        //circlePattern = new DefensiveCirclePattern();
        //circlePattern.characterRadius = 2.0f;
        for (int i = 0; i < playerList.Length; i++)
        {
            AddCharacter(playerList[i]);
            squarePattern.calculateNumberOfSlots(slotAssignments);
            //circlePattern.calculateNumberOfSlots(slotAssignments);
        }      
        UpdateSlotAssignments();
        for(int i = 0; i < slotAssignments.Count; i++)
        {
            slotAssignments[i].player.GetComponent<Arrive>().GetSteeringOutput(slotAssignments[i].player.transform.position);
        }
    }

    private void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlotAssignments()
    {
        for(int i = 0; i < slotAssignments.Count; i++)
        {
            slotAssignments[i].slotNumber = i;
        }
        //driftOffset = circlePattern.GetDriftOffset(slotAssignments);
        driftOffset = squarePattern.GetDriftOffset(slotAssignments);
    }

    public bool AddCharacter(GameObject character)
    {
        int occupiedSlots = slotAssignments.Count;
        //if (circlePattern.supportSlots(occupiedSlots + 1))
        //{
        //    SlotAssignment slotAssignment = new SlotAssignment();
        //    slotAssignment.player = character;
        //    slotAssignments.Add(slotAssignment);
        //    circlePattern.calculateNumberOfSlots(slotAssignments);
        //    return true;
        //}
        if (squarePattern.supportSlots(occupiedSlots + 1))
        {
            SlotAssignment slotAssignment = new SlotAssignment();
            slotAssignment.player = character;
            slotAssignments.Add(slotAssignment);
            squarePattern.calculateNumberOfSlots(slotAssignments);
            return true;
        }
        return false;
    }

    public bool RemoveCharacter(GameObject character)
    {
        int slot = FindCharacterSlot(character);
        if (slot > 0)
        {
            slotAssignments.RemoveAt(slot);
            UpdateSlotAssignments();
            return true;
        }
        return false;
    }

    public void UpdateSlots()
    {
        Vector3 anchorPos = GetAnchorPoint();
        for(int i = 0; i < slotAssignments.Count; i++)
        {
            //Kinematic relativeLoc = circlePattern.GetSlotLocation(slotAssignments[i].slotNumber);
            Kinematic relativeLoc = squarePattern.GetSlotLocation(slotAssignments[i].slotNumber);
            Kinematic location = new Kinematic();
            location.position = relativeLoc.position + anchorPos;
            location.position -= driftOffset.position;
            slotAssignments[i].player.GetComponent<Arrive>().GetSteeringOutput(location.position);
        }
    }

    public int FindCharacterSlot(GameObject character)
    {
        for(int i = 0; i < slotAssignments.Count; i++)
        {
            if (slotAssignments[i].player == character)
                return i;
        }
        return -1;
    }

    public Vector3 GetAnchorPoint()
    {
        Vector3 anchorPoint = GameObject.Find("Target").gameObject.transform.position;
        return anchorPoint;
    }

    protected override Vector3 GetFacing(Transform agent)
    {
        Vector3 childPosition = agent.GetChild(0).position;
        Vector3 faceDir3D = (childPosition - agent.position);
        Vector3 faceDir = new Vector3(faceDir3D.x, 0, faceDir3D.z);
        faceDir = faceDir.normalized;
        return faceDir;
    }
}
