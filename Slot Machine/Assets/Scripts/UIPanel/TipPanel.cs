using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipPanel : BasePanel
{
    [SerializeField]
    private GameObject tipSample;

    [SerializeField]
    private float sizeY;

    private List<TipContent> tipList = new List<TipContent>();

    public override void OnEnter()
    {
        base.OnEnter();
        uiManager.SetTipPanel = this;
        tipSample.SetActive(false);
        sizeY = GetComponent<RectTransform>().rect.height;
    }

    /// <summary>
    /// 顯示提示文本
    /// </summary>
    /// <param name="str">文本內容</param>
    public void ShowTip(string str)
    {
        TipContent panel = null;
        if (tipList.Count == 0)
        {
            panel = Instantiate(tipSample).GetComponent<TipContent>();
            panel.transform.SetParent(transform);
            panel.gameObject.SetActive(true);
            panel.Show(str, sizeY);
            tipList.Add(panel);
        }
        else
        {
            for (int i = 0; i < tipList.Count; i++)
            {
                if (!tipList[i].gameObject.activeSelf)
                {
                    panel = tipList[i];
                    break;
                }
            }

            if (panel == null)
            {
                panel = Instantiate(tipSample).GetComponent<TipContent>();
                panel.transform.SetParent(transform);
                tipList.Add(panel);
            }
            panel.gameObject.SetActive(true);
            panel.Show(str, sizeY);
        }
    }
}
