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
        public string Level { get; set; }
        public string ImageUrl { get; set; }
        public int Coin { get; set; }
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
    public async void StartLogin()
    {
        StartPanel startPanel = uIManager.PushPanel(PanelType.StartPanel).GetComponent<StartPanel>();
        await Task.Delay(500);
        startPanel.Login();
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
