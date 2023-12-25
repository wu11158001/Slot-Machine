using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class ClassicBonusPoolRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private GameClassicPanel gameClassicPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.BonusInfo;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameClassicPanel.SetBonusPool(pack);
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

        BonusPack bonusPack = new BonusPack();
        bonusPack.GameName = "classic";

        pack.BonusPack = bonusPack;
        base.SendRequest(pack);
    }
}
