using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFormationPattern
{
    public Kinematic GetDriftOffset(List<SlotAssignment> assignments);
    public Kinematic GetSlotLocation(int slotNumber);
    public bool supportSlots(int slotCount);
    public void calculateNumberOfSlots(List<SlotAssignment> assignments);
}
