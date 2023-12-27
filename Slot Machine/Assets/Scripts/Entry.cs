using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class Entry : MonoBehaviour
{
    private static Entry entry;
    public static Entry Instance
    {
        get
        {
            if (entry == null) entry = GameObject.Find("Entry").GetComponent<Entry>();
            return entry;
        }
    }

    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uiManager;
    private GPGSManager gpgsManager;
    private UnityAdsManager unityAdsManager;
    private BonusPoolManager bonusPoolManager;
    private SoundManager soundManager;

    /// <summary>
    /// 客戶訊息
    /// </summary>
    public class UserInfoData
    {
        public string NickName { get; set; }
        public string UserId { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public string ImgUrl { get; set; }
        public long Coin { get; set; }
        public int LoginDay { get; set; }
    }
    public UserInfoData UserInfo { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        UserInfo = new UserInfoData();

        uiManager = new UIManager();
        clientManager = new ClientManager();
        requestManager = new RequestManager();
        gpgsManager = new GPGSManager();
        unityAdsManager = new UnityAdsManager();
        bonusPoolManager = new BonusPoolManager();
        soundManager = new SoundManager();

        uiManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        gpgsManager.OnInit();
        unityAdsManager.OnInit();
        bonusPoolManager.OnInit();
        soundManager.OnInit();

#if UNITY_EDITOR_WIN
        TestLogin();
#endif
    }

    /// <summary>
    /// 測試登入
    /// </summary>
    void TestLogin()
    {
        UserInfo.UserId = "550101";
        UserInfo.NickName = "Test Account";
        UserInfo.ImgUrl = "Null";
        StartLogin();
    }

    /// <summary>
    /// 發送TCP
    /// </summary>
    /// <param name="pack"></param>
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    /// <summary>
    /// 處理回覆
    /// </summary>
    /// <param name="pack"></param>
    public void HandleResponse(MainPack pack)
    {
        requestManager.HandleResponse(pack);
    }

    /// <summary>
    /// 添加請求
    /// </summary>
    /// <param name="request"></param>
    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    /// <summary>
    /// 移除請求
    /// </summary>
    /// <param name="action"></param>
    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    public void ShowTip(string str)
    {
        uiManager.ShowTip(str);
    }

    /// <summary>
    /// 播放廣告
    /// </summary>
    public void ShowAd()
    {
        unityAdsManager.ShowAd();
    }

    /// <summary>
    /// 開始登入
    /// </summary>
    public void StartLogin()
    {
        uiManager.GetStartPanel.Login();
    }

    /// <summary>
    /// 設定用戶訊息
    /// </summary>
    /// <param name="pack"></param>
    public void SetUserInfo(MainPack pack)
    {
        entry.UserInfo.Level = pack.UserInfoPack.Level;
        entry.UserInfo.Exp = pack.UserInfoPack.Exp;
        entry.UserInfo.Coin = pack.UserInfoPack.Coin;
        entry.UserInfo.LoginDay = pack.UserInfoPack.LoginDay;
    }

    /// <summary>
    /// 進入遊戲
    /// </summary>
    /// <param name="gamePanel"></param>
    public void EnterGame(PanelType gamePanel)
    {
        uiManager.PushPanel(gamePanel);
        uiManager.ShowLoading();
    }

    /// <summary>
    /// 設定所有獎池訊息
    /// </summary>
    /// <param name="pack"></param>
    public void SetAllBonusInfo(MainPack pack)
    {
        bonusPoolManager.SetAllBonusInfo(pack);
    }

    /// <summary>
    /// 添加獎池
    /// </summary>
    /// <param name="baseBonus"></param>
    public void AddBonusPool(GameBonusPool baseBonus)
    {
        bonusPoolManager.AddBonusPool(baseBonus);
    }

    /// <summary>
    /// 設定廣播贏家
    /// </summary>
    /// <param name="pack"></param>
    public void SetBroadcastWinner(MainPack pack)
    {
        uiManager.SetBroadcastWinner(pack);
    }

    /// <summary>
    /// 顯示成就
    /// </summary>
    public void ShowAchievement()
    {
        gpgsManager.ShowAchievement();
    }

    /// <summary>
    /// 解鎖遊玩次數
    /// </summary>
    public void OnLuckPlayTime()
    {
        gpgsManager.OnLuckPlayTime();
    }

    /// <summary>
    /// 解鎖獲得金幣
    /// </summary>
    /// <param name="coin"></param>
    public void OnLuckGetCoin(long coin)
    {
        gpgsManager.OnLuckGetCoin(coin);
    }

    /// <summary>
    /// 顯示排行榜
    /// </summary>
    public void ShowLeaderboard()
    {
        gpgsManager.ShowLeaderboard();
    }

    /// <summary>
    /// 設定排行榜分數
    /// </summary>
    /// <param name="id">GPGSId</param>
    /// <param name="val">更換植</param>
    public void SetLeaderboard(string id, long val)
    {
        gpgsManager.SetLeaderboard(id, val);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundType"></param>
    /// <param name="volume"></param>
    public void PlaySound(SoundType soundType, float volume = 1)
    {
        soundManager.PlaySound(soundType, volume);
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uiManager.OnDestroy();
        gpgsManager.OnDestroy();
        unityAdsManager.OnDestroy();
        bonusPoolManager.OnDestroy();
    }
}
