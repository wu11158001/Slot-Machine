using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;
using System.Threading.Tasks;

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
    private UIManager uIManager;
    private GPGSManager gpgsManager;
    private UnityAdsManager unityAdsManager;

    /// <summary>
    /// 客戶訊息
    /// </summary>
    public class UserInfoData
    {
        public string NickName { get; set; }
        public string UserId { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public string ImageUrl { get; set; }
        public int Coin { get; set; }
        public int LoginDay { get; set; }
    }
    public UserInfoData UserInfo { get; set; }

    private void Awake()
    {
        UserInfo = new UserInfoData();

        uIManager = new UIManager();
        clientManager = new ClientManager();
        requestManager = new RequestManager();
        gpgsManager = new GPGSManager();
        unityAdsManager = new UnityAdsManager();

        uIManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        gpgsManager.OnInit();
        unityAdsManager.OnInit();

#if UNITY_EDITOR_WIN
        TestLogin();
#endif
    }

    /// <summary>
    /// 測試登入
    /// </summary>
    void TestLogin()
    {
        UserInfo.UserId = "123456";
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
        uIManager.ShowTip(str);
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
    public async void StartLogin()
    {
        await Task.Delay(500);
        uIManager.GetStartPanel.Login();
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

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
        gpgsManager.OnDestroy();
        unityAdsManager.OnDestroy();
    }
}
