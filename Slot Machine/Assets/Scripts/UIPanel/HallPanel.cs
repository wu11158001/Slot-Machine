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
    private Text nickName_Txt, coin_Txt, level_Txt;

    [SerializeField]
    private Image avatar_Img, lvProcess_Img;

    [SerializeField]
    private Button gameClassic_Btn;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        userInfoRequset.SendRequest(entry.UserInfo.UserId);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        //遊戲_經典
        gameClassic_Btn.onClick.AddListener(() =>
        {
            entry.EnterGame(PanelType.GameClassicPanel);
        });
   
        SetAvatarAndNickName();
    }

    /// <summary>
    /// 設定頭像與暱稱
    /// </summary>
    private async void SetAvatarAndNickName()
    {
        Sprite avatar = await Tools.ImageUrlToSprite(entry.UserInfo.ImageUrl);
        if (avatar != null) avatar_Img.sprite = avatar;
        if (!string.IsNullOrEmpty(entry.UserInfo.NickName)) nickName_Txt.text = entry.UserInfo.NickName;
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
