using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Subject
{
    public void Attach(GameObject observer) { }
    public void Detach(GameObject observer) { }
    public void Notify() { }
}
