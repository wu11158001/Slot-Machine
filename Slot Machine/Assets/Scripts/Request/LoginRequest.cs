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

    private void Update()
    {
        if (pack != null)
        {
            startPanel.OnLoginResult(pack);
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

        LoginPack loginPack = new LoginPack();
        loginPack.Userid = entry.UserInfo.UserId;
        loginPack.NickName = entry.UserInfo.NickName;
        loginPack.ImgUrl = entry.UserInfo.ImgUrl;
        pack.LoginPack = loginPack;

        base.SendRequest(pack);
    }
}
