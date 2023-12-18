using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;

public class StartPanel : BasePanel
{
    [SerializeField]
    private LoginRequest loginRequest;

    public void Login()
    {
        loginRequest.SendRequest(entry.UserInfo.userId);
    }

    /// <summary>
    /// 登入結果
    /// </summary>
    /// <param name="pack"></param>
    public void OnLoginResult(MainPack pack)
    {
        if (pack.ReturnCode == ReturnCode.Succeed)
        {
            uiManager.ShowTip("登入成功");
            Debug.Log("登入成功");
        }
        else if (pack.ReturnCode == ReturnCode.DuplicateLogin)
        {
            uiManager.ShowTip("重複登入");
        }
        else
        {
            uiManager.ShowTip("登入失敗");
        }
    }
}
