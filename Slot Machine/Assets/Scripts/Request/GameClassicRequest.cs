using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class GameClassicRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private GameClassicPanel classicPanel;

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
            classicPanel.GetResult(pack);
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
    /// <param name="id"></param>
    public void SendRequest(string id)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequest(pack);
    }
}
