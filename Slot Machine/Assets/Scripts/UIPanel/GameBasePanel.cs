using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBasePanel : BasePanel
{
    [SerializeField]
    //(結果編號,腳本)
    protected Dictionary<int, BroadAction> resultDic = new Dictionary<int, BroadAction>();

    /// <summary>
    /// 設定轉盤
    /// </summary>
    /// <param name="count">數量</param>
    /// <param name="parent">父物件</param>
    /// <param name="obj">轉盤內容樣板</param>
    protected virtual void SetBroad(int count, Transform parent, GameObject obj)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject broad = Instantiate(obj);
            broad.transform.SetParent(parent);
            broad.SetActive(true);
            resultDic.Add(i, broad.GetComponent<BroadAction>());
        }

        obj.SetActive(false);
    }

    /// <summary>
    /// 移除轉盤
    /// </summary>
    protected virtual void RemoveBroad()
    {
        foreach (var item in resultDic.Values)
        {
            Destroy(item.gameObject);
        }

        resultDic.Clear();
    }
}
