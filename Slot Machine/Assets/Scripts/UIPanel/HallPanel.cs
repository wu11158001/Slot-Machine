using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Threading.Tasks;

public class HallPanel : BasePanel
{
    [SerializeField]
    private Text nickName_Txt, coin_Txt, level_Txt;

    [SerializeField]
    private Image avatar_Img, lvProcess_Img;

    [SerializeField]
    private Button gameClassic_Btn;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
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
}
