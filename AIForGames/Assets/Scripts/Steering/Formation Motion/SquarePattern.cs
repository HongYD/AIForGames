using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePattern : IFormationPattern
{
    private int numberOfSlots;
    public float characterRadius;
    float xOffset = 0.5f;
    float zOffset = 0.5f;
    public Kinematic GetDriftOffset(List<SlotAssignment> assignments)
    {
        Kinematic center = new Kinematic();
        for (int i = 0; i < assignments.Count; i++)
        {
            Kinematic location = GetSlotLocation(assignments[i].slotNumber);
            center.position += location.position;
            center.orientation += location.orientation;
        }
        int numberOfAssignment = assignments.Count;
        if (numberOfAssignment > 0)
        {
            center.position /= numberOfAssignment;
            center.orientation /= numberOfAssignment;
        }
        return center;
    }

    public Kinematic GetSlotLocation(int slotNumber)
    {
        Kinematic location = new Kinematic();
        int rows = Mathf.FloorToInt(Mathf.Sqrt(numberOfSlots));
        int xLoc = slotNumber % rows;
        int zLoc = Mathf.FloorToInt(slotNumber / rows);
        location.position.x = xLoc + characterRadius * xLoc + xOffset * xLoc;
        location.position.z = zLoc + characterRadius * zLoc + zOffset * zLoc;
        return location;

    }

    public bool supportSlots(int slotCount)
    {
        return true;
    }

    public void calculateNumberOfSlots(List<SlotAssignment> assignments)
    {
        numberOfSlots = assignments.Count;
    }
}
