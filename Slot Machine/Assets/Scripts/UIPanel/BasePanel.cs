using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected UIManager uiManager;
    public UIManager SetUIManager { set { uiManager = value; } }

    protected Entry entry { get { return Entry.Instance; } }

    /// <summary>
    /// UI面板開始
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// UI面板暫停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// UI面板繼續
    /// </summary>
    public virtual void OnRecovery()
    {

    }

    /// <summary>
    /// UI面板退出
    /// </summary>
    public virtual void OnExit()
    {

    }
}
