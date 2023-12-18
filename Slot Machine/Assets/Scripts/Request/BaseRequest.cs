using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;

    protected ActionCode actionCode;
    public ActionCode GetActionCode { get { return actionCode; } }

    protected Entry entry;

    public virtual void Awake()
    {
        entry = Entry.Instance;
    }

    public virtual void Start()
    {
        entry.AddRequest(this);
        Debug.Log("添加:" + actionCode.ToString());
    }

    /// <summary>
    /// 接收協議
    /// </summary>
    /// <param name="pack"></param>
    public virtual void OnResponse(MainPack pack)
    {

    }

    /// <summary>
    /// 發送請求
    /// </summary>
    /// <param name="pack"></param>
    public virtual void SendRequest(MainPack pack)
    {
        entry.Send(pack);
    }

    public virtual void OnDestroy()
    {
        entry.RemoveRequest(actionCode);
    }
}
