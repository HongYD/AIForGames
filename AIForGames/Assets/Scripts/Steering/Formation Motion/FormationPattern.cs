using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationPattern
{
    public int numberOfSlots;
    protected float characterRadius = 1.0f;
    protected float xOffset = 1.0f;
    protected float zOffset = 1.0f;
    public virtual Kinematic GetDriftOffset(List<SlotAssignment> assignments) { return new Kinematic(); }
    public virtual Kinematic GetSlotLocation(int slotNumber) { return new Kinematic(); }
    public virtual bool supportSlots(int slotCount) { return true; }
    public virtual void calculateNumberOfSlots(List<SlotAssignment> assignments) { }
}
