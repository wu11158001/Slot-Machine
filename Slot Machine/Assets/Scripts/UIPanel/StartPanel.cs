using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;

public class StartPanel : BasePanel
{
    [SerializeField]
    private LoginRequest loginRequest;

    [SerializeField]
    private Text tip_Txt;

    private int tipEffectCount;//提示文字效果數量

    private void Start()
    {
        InvokeRepeating(nameof(TipTxtEffect), 0, 0.5f);
    }

    /// <summary>
    /// 提示文字效果
    /// </summary>
    void TipTxtEffect()
    {
        tipEffectCount++;
        if (tipEffectCount > 3) tipEffectCount = 0;

        tip_Txt.text = $"Login{new string('.', tipEffectCount)}";
    }

    /// <summary>
    /// 登入
    /// </summary>
    public void Login()
    {
        loginRequest.SendRequest(entry.UserInfo.UserId);
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
            Invoke(nameof(IntoHall), 1);
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

    /// <summary>
    /// 進入大廳
    /// </summary>
    void IntoHall()
    {
        uiManager.PushPanel(PanelType.HallPanel);
    }
}
