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
    private AdRewardRequest adRewardRequest;
    [SerializeField]
    private CoinFlyEffect coinFlyEffect;

    [SerializeField]
    private UserCoin userCoin;
    [SerializeField]
    private UserLevel userLevel;

    [SerializeField]
    private Text nickName_Txt, coin_Txt, level_Txt;
    [SerializeField]
    private Image avatar_Img, lvProcess_Img;
    [SerializeField]
    private Button gameClassic_Btn, ad_Btn, achievement_Btn, Rank_Btn;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        userInfoRequset.SendRequest(entry.UserInfo.UserId);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    public override void OnRecovery()
    {
        userInfoRequset.SendRequest(entry.UserInfo.UserId);
    }

    private void Start()
    {
        //廣告獎勵按鈕
        ad_Btn.onClick.AddListener(() =>
        {
            entry.PlaySound(SoundType.ButtonClick);
            adRewardRequest.SendRequest();
            entry.ShowAd();
        });

        //成就按鈕
        achievement_Btn.onClick.AddListener(() =>
        {
            entry.PlaySound(SoundType.ButtonClick);
            entry.ShowAchievement();
        });

        //排行按鈕
        Rank_Btn.onClick.AddListener(() =>
        {
            entry.PlaySound(SoundType.ButtonClick);
            entry.ShowLeaderboard();
        });

        //遊戲_經典
        gameClassic_Btn.onClick.AddListener(() =>
        {
            entry.PlaySound(SoundType.IntoGame);
            entry.EnterGame(PanelType.GameClassicPanel);
        });
   
        SetAvatarAndNickName();
    }

    private void Update()
    {
        ad_Btn.interactable = StateManger.isAdComplete;
    }

    /// <summary>
    /// 設定頭像與暱稱
    /// </summary>
    private async void SetAvatarAndNickName()
    {
        Sprite avatar = await Tools.ImageUrlToSprite(entry.UserInfo.ImgUrl);
        if (avatar != null) avatar_Img.sprite = avatar;
        if (!string.IsNullOrEmpty(entry.UserInfo.NickName)) nickName_Txt.text = entry.UserInfo.NickName;
    }

    /// <summary>
    /// 更新用戶訊息
    /// </summary>
    public void UpdateUserInfo()
    {
        userCoin.UpdateValue();
        userLevel.UpdateValue();
    }

    /// <summary>
    /// 廣告獎勵
    /// </summary>
    /// <param name="pack"></param>
    public void AdReward(MainPack pack)
    {
        long rewardCoin = pack.AdRewardPack.RewardCoin;
        coinFlyEffect.CoinEffectStart(rewardCoin);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }
}
