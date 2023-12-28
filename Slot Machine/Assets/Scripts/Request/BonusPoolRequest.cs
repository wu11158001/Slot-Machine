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
            StartCoroutine(IWaitSpinState(pack));
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
    /// 等待轉盤結束
    /// </summary>
    /// <returns></returns>
    private IEnumerator IWaitSpinState(MainPack pack)
    {
        yield return new WaitUntil(() => StateManger.IsGameSpinning == false);
        entry.SetAllBonusInfo(pack);
    }
}
