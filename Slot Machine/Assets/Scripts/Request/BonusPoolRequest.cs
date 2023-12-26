using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class BonusPoolRequest : BaseRequest
{
    private MainPack pack;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.BonusPoolInfo;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            entry.SetAllBonusInfo(pack);
            pack = null;
        }
    }

    /// <summary>
    /// 協議接收
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
