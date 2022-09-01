using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotAssignment
{
    public GameObject player;
    public int slotNumber;
}
public class FormationManager : MonoBehaviour
{
    public List<SlotAssignment> slotAssignments;
    public Kinematic driftOffset;
    public DefensiveCirclePattern circlePattern;

    public void UpdateSlotAssignments()
    {
        for(int i = 0; i < slotAssignments.Count; i++)
        {
            slotAssignments[i].slotNumber = i;
        }
        driftOffset = circlePattern.GetDriftOffset(slotAssignments);
    }

    public bool AddCharacter(GameObject character)
    {
        int occupiedSlots = slotAssignments.Count;
        if (circlePattern.supportSlots(occupiedSlots + 1))
        {
            SlotAssignment slotAssignment = new SlotAssignment();
            slotAssignment.player = character;
            slotAssignments.Add(slotAssignment);
            UpdateSlotAssignments();
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

    public Vector3 GetAnchorPoiint()
    {
        return new Vector3();
    }
}
