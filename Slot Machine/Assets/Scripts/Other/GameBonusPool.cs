using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBonusPool : MonoBehaviour
{
    protected Entry entry { get { return Entry.Instance; } }

    protected BonusPoolManager bonusPoolManager;
    public BonusPoolManager SetBonusPoolManager { set { bonusPoolManager = value; } }

    public BonusPoolType bonusType;

    private Text bonus_Txt;
    private long bonusValue;

    private void Start()
    {
        bonus_Txt = GetComponent<Text>();

        entry.AddBonusPool(this);
        SetValue();
    }

    private void OnEnable()
    {
        if (bonusPoolManager != null)
        {
            SetValue();
        }
    }

    /// <summary>
    /// 設定獎池金幣
    /// </summary>
    /// <param name="val"></param>
    public void SetValue()
    {
        bonusValue = bonusPoolManager.GetBonusValue(bonusType);
        bonus_Txt.text = Tools.SetCoinStr(bonusValue);
    }
}
