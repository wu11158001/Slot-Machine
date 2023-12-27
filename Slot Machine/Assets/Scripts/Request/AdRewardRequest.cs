using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class AdRewardRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private HallPanel hallPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.AdReward;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            hallPanel.AdReward(pack);
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

    /// <summary>
    /// 發送請求
    /// </summary>
    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequest(pack);
    }
}
