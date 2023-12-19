using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Threading.Tasks;

public class HallPanel : BasePanel
{
    [SerializeField]
    private UserInfoRequset userInfoRequset;

    [SerializeField]
    private Text coin_Txt, level_Txt;

    [SerializeField]
    private Image avatar_Img, lvProcess_Img;

    private void Start()
    {
        userInfoRequset.SendRequest(entry.UserInfo.UserId);
        SetAvatar();
    }

    /// <summary>
    /// 設定頭像
    /// </summary>
    private async void SetAvatar()
    {
        Sprite avatar = await Tools.ImageUrlToSprite(entry.UserInfo.ImageUrl);
        if (avatar != null) avatar_Img.sprite = avatar;
    }

    /// <summary>
    /// 設定用戶訊息
    /// </summary>
    public void SetUserInfo()
    {
        coin_Txt.text = $"{entry.UserInfo.Coin}";
        level_Txt.text = $"LV.{entry.UserInfo.Level}";
        lvProcess_Img.fillAmount = entry.UserInfo.Exp / (2 * entry.UserInfo.Level);
    }
}
