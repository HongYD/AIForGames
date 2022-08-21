using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteSubject : Subject
{
    private bool _isNeedCreate;
    public bool IsNeedCreate
    {
        get { return _isNeedCreate; } 
        set { _isNeedCreate = value; }
    }
}
