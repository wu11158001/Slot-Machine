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

    public override void OnInit()
    {
        base.OnInit();

        canvasTransform = GameObject.Find("Canvas").transform;
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
    /// <param name="isSync">是否為異步</param>
    public void ShowTip(string str, bool isSync = false)
    {
        if (!panelDic.ContainsKey(PanelType.TipPanel))
        {
            tipPanel = PushPanel(PanelType.TipPanel).GetComponent<TipPanel>();
        }
        
        tipPanel.ShowTip(str, isSync);
    }
}
