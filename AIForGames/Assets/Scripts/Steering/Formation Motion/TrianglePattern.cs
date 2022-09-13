using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePattern:FormationPattern
{
    public override Kinematic GetDriftOffset(List<SlotAssignment> assignments)
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

    public override Kinematic GetSlotLocation(int slotNumber)
    {
        int rowNum = slotNumber;
        int i = 0;
        float xShift = characterRadius + xOffset;
        while(rowNum > 0)
        {
            rowNum -= i;
            if (rowNum > 0)
            {
                i++;
            }
            else
            {
                break;
            }
        }
        int xLoc = 0;
        int zLoc = 0;
        if (rowNum == 0)
            xLoc = rowNum;
        else
            xLoc = rowNum + i;
        if (xLoc == 0)
            zLoc = i;
        else
            zLoc = i - 1;
        Kinematic location = new Kinematic();
        location.position.x = xLoc + characterRadius * xLoc + xOffset * xLoc - xShift * zLoc;
        location.position.z = zLoc + characterRadius * zLoc + zOffset * zLoc;
        return location;
    }

    public override bool supportSlots(int slotCount)
    {
        return true;
    }

    public override void calculateNumberOfSlots(List<SlotAssignment> assignments)
    {
        numberOfSlots = assignments.Count;
    }
}
