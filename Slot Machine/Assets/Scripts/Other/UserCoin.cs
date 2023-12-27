using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCoin : BasePanel
{
    [SerializeField]
    private Text coin_Txt;

    private const float duration = 0.5f;
    private long tempVal;
    private long targetVal;

    private void OnEnable()
    {
        UpdateValue();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void UpdateValue()
    {
        tempVal = entry.UserInfo.Coin;
        coin_Txt.text = Tools.SetCoinStr(entry.UserInfo.Coin);
    }

    /// <summary>
    /// 無效果變化
    /// </summary>
    /// <param name="coin"></param>
    public void NoEffect(long coin)
    {
        coin_Txt.text = Tools.SetCoinStr(coin);
    }

    /// <summary>
    /// 變更金幣
    /// </summary>
    public void ChangeCoin(long target)
    {
        targetVal = target;
        StartCoroutine(nameof(IEffect));
    }

    /// <summary>
    /// 更新效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEffect()
    {
        float startTime = Time.time;
        long num = 0;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            num = (long)Mathf.Round(Mathf.Lerp(tempVal, targetVal, progress));
            coin_Txt.text = Tools.SetCoinStr(num);
            yield return null;
        }

        coin_Txt.text = Tools.SetCoinStr(targetVal);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
