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
            if (entry == null) entry = GameObject.FindObjectOfType<Entry>();
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
        public string nickName { get; set; }
        public string userId { get; set; }
        public string level { get; set; }
        public string imageUrl { get; set; }
        public int coin { get; set; }
    }
    public UserInfoData UserInfo { get; set; }

    private void Awake()
    {
        UserInfo = new UserInfoData();

        uIManager = new UIManager(this);
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        gpgsManager = new GPGSManager(this);
        unityAdsManager = new UnityAdsManager(this);

        uIManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        gpgsManager.OnInit();
        unityAdsManager.OnInit();
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
    /// <param name="isSync">是否為異步</param>
    public void ShowTip(string str, bool isSync = false)
    {
        uIManager.ShowTip(str, isSync);
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
        StartPanel startPanel = uIManager.PushPanel(PanelType.StartPanel).GetComponent<StartPanel>();
        startPanel.Login();
    }
}
