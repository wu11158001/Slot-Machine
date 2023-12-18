using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Image bg_Img;
    public Text tipText;
    string tip = null;

    public override void OnEnter()
    {
        base.OnEnter();
        tipText.CrossFadeAlpha(0, 0.1f, false);
        uiManager.SetTipPanel = this;
    }

    private void Update()
    {
        if (tip != null)
        {
            ShowText(tip);
            tip = null;
        }
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    /// <param name="isSync">是否為異步</param>
    public void ShowTip(string str, bool isSync = false)
    {
        //異步顯示
        if (isSync) tip = str;
        else ShowText(str);
    }

    /// <summary>
    /// 顯示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    void ShowText(string str)
    {
        tipText.text = str;
        bg_Img.enabled = true;
        tipText.CrossFadeAlpha(1, 0.05f, false);
        transform.SetSiblingIndex(100);

        CancelInvoke(nameof(HodeTip));
        Invoke(nameof(HodeTip), 2.5f);
    }

    /// <summary>
    /// 隱藏提示
    /// </summary>
    void HodeTip()
    {
        tipText.CrossFadeAlpha(0, 0.05f, false);
        bg_Img.enabled = false;
    }
}
