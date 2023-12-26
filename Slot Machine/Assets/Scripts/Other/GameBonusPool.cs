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

    private const float duration = 1f;
    private Text bonus_Txt;
    private long bonusValue;

    [SerializeField]
    private bool isActive;
    private long preVal;

    private void Start()
    {
        bonus_Txt = GetComponent<Text>();

        entry.AddBonusPool(this);
        SetValue();
    }

    private void OnEnable()
    {
        isActive = true;

        if (bonusPoolManager != null)
        {
            SetValue();
        }
    }

    private void OnDisable()
    {
        isActive = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// 設定獎池金幣
    /// </summary>
    /// <param name="val"></param>
    public void SetValue()
    {
        bonusValue = bonusPoolManager.GetBonusValue(bonusType);
        if(isActive) StartCoroutine(nameof(IEffect));
    }

    /// <summary>
    /// 效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEffect()
    {
        float startTime = Time.time;
        long val = 0;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time -startTime) / duration;
            val = (long)Mathf.Round(Mathf.Lerp(preVal, bonusValue, progress));
            bonus_Txt.text = Tools.SetCoinStr(val);

            yield return null;
        }

        bonus_Txt.text = Tools.SetCoinStr(bonusValue);
        preVal = bonusValue;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
