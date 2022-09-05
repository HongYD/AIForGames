using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveCirclePattern : FormationPattern
{
    //calculate average position and orienation of slot character(assignments)

    public override Kinematic GetDriftOffset(List<SlotAssignment> assignments)
    {
        Kinematic center = new Kinematic();
        for(int i = 0; i < assignments.Count; i++)
        {
            Kinematic location = GetSlotLocation(assignments[i].slotNumber);
            center.position += location.position;
            center.orientation += location.orientation;
        }
        int numberOfAssignment = assignments.Count;
        if(numberOfAssignment > 0)
        {
            center.position /= numberOfAssignment;
            center.orientation /= numberOfAssignment;
        }
        return center;
    }


    public override Kinematic GetSlotLocation(int slotNumber)
    {
        //ÿһ��slot�����ĽǶ�
        characterRadius = 2.0f;
        Kinematic location = new Kinematic();
        float angleAroundCircle = ((float)slotNumber / (float)numberOfSlots) * Mathf.PI * 2.0f;
        float radius = characterRadius / Mathf.Sin(2.0f * Mathf.PI / numberOfSlots);
        location.position.x = radius * Mathf.Cos(angleAroundCircle);
        location.position.z = radius * Mathf.Sin(angleAroundCircle);
        location.orientation = angleAroundCircle;
        return location;
    }

    public override bool supportSlots(int slotCount)
    {
        return true;
    }

    public override void calculateNumberOfSlots(List<SlotAssignment> assignments)
    {
        //int filledSlots = 0;
        //int maxSlotNumber = 0;
        //for(int i = 0; i < assignments.Count; i++)
        //{
        //    if (assignments[i].slotNumber >= maxSlotNumber)
        //        filledSlots = assignments[i].slotNumber;
        //}
        //numberOfSlots = filledSlots + 1;
        //return numberOfSlots;
        numberOfSlots = assignments.Count;
    }
}
