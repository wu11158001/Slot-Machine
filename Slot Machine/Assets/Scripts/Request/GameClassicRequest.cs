using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class GameClassicRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private GameClassicPanel gameClassicPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ClassicResult;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            gameClassicPanel.GetResult(pack);
            entry.SetLeaderboard(GPGSIds.leaderboard_wealth_accumulation, pack.UserInfoPack.Coin);
            entry.OnLuckGetCoin(pack.ClassicPack.WinCoin);
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
    /// <param name="betVal">押注金額</param>
    public void SendRequest(long betVal)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        ClassicPack classicPack = new ClassicPack();
        classicPack.BetValue = betVal;
        pack.ClassicPack = classicPack;

        base.SendRequest(pack);
    }
}
