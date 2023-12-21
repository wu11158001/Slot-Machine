using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class ClassicRateRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private ClassicRate classicRate;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GetClassicRate;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            classicRate.SetRate(pack);
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
