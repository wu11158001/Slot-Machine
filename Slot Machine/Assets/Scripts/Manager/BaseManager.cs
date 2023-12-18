using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected Entry entry { get { return Entry.Instance; } }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
