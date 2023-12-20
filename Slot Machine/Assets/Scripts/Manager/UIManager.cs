using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    private Transform canvasTransform;

    //紀錄已生成的UI
    private Dictionary<PanelType, BasePanel> panelDic = new Dictionary<PanelType, BasePanel>();
    //場景顯示的UI
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    //提示框面板
    private TipPanel tipPanel;
    public TipPanel SetTipPanel { set { tipPanel = value; } }
    //開始面板
    private StartPanel startPanel;
    public StartPanel GetStartPanel { get { return startPanel; } }
    //載入畫面
    private LoadingPanel loadingPanel;
    public LoadingPanel SetLoadingPanel { set { loadingPanel = value; } }

    public override void OnInit()
    {
        base.OnInit();

        canvasTransform = GameObject.Find("Canvas").transform;

        PushPanel(PanelType.LoadingPanel);
        PushPanel(PanelType.TipPanel);
        startPanel = PushPanel(PanelType.StartPanel).GetComponent<StartPanel>();
    }

    /// <summary>
    /// 實例化UI
    /// </summary>
    /// <param name="panelType"></param>
    BasePanel SpawnPanel(PanelType panelType)
    {
        if (!panelDic.ContainsKey(panelType))
        {
            string path = $"Panel/{panelType}";
            Debug.Log($"產生Panel: {path}");
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
            obj.transform.SetParent(canvasTransform, false);
            BasePanel basePanel = obj.GetComponent<BasePanel>();
            basePanel.SetUIManager = this;
            panelDic.Add(panelType, basePanel);
            return basePanel;
        }

        return null;
    }

    /// <summary>
    /// 顯示UI
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(PanelType panelType)
    {
        if (panelDic.ContainsKey(panelType))
        {
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            BasePanel panel = panelDic[panelType];
            panelStack.Push(panel);
            panel.OnEnter();

            if(tipPanel) tipPanel.ChangePanel();

            return panel;
        }
        else
        {
            //實例化UI
            BasePanel newPanel = SpawnPanel(panelType);

            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }

            panelStack.Push(newPanel);
            newPanel.OnEnter();

            if (tipPanel) tipPanel.ChangePanel();

            return newPanel;
        }
    }

    /// <summary>
    /// 退回前個UI面板
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count == 0) return;

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        BasePanel nextPanel = panelStack.Peek();
        nextPanel.OnRecovery();
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    public void ShowTip(string str)
    {
        if (!panelDic.ContainsKey(PanelType.TipPanel))
        {
            tipPanel = PushPanel(PanelType.TipPanel).GetComponent<TipPanel>();
        }
        
        tipPanel.ShowTip(str);
    }

    /// <summary>
    /// 顯示載入畫面
    /// </summary>
    public void ShowLoading()
    {
        loadingPanel.Loading();
    }
}
