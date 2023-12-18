using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected Entry entry;

    public BaseManager(Entry entry)
    {
        this.entry = entry;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
