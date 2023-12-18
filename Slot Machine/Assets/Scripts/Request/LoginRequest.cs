using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class LoginRequest : BaseRequest
{
    private MainPack pack;

    [SerializeField]
    private StartPanel startPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        base.Awake();
    }

    /// <summary>
    /// 協議接收
    /// </summary>
    /// <param name="pack"></param>
    public override void OnResponse(MainPack pack)
    {
        startPanel.OnLoginResult(pack);
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

        LoginPack loginPack = new LoginPack();
        loginPack.Userid = id;
        pack.LoginPack = loginPack;
        pack.Str = "LoginRequest";

        base.SendRequest(pack);
    }
}
