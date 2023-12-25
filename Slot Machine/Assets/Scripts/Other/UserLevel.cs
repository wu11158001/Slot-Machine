using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserLevel : BasePanel
{
    [SerializeField]
    private Text level_Txt;
    [SerializeField]
    private Image lvProcess_Img;
    [SerializeField]
    private GameObject lvUpEffect;

    private const float duration = 0.5f;
    private int tempLv;
    private int tempExp;
    private int targetLv;
    private int targetExp;

    private void Start()
    {
        lvUpEffect.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateValue();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void UpdateValue()
    {
        tempLv = entry.UserInfo.Level;
        tempExp = entry.UserInfo.Exp;

        level_Txt.text = $"LV.{entry.UserInfo.Level}";

        float p = entry.UserInfo.Level - 1 == 0 ? 0 : Mathf.Pow(2, entry.UserInfo.Level - 1);
        float curExp = entry.UserInfo.Exp - p;
        float needExp = Mathf.Pow(2, entry.UserInfo.Level) - p;
        lvProcess_Img.fillAmount = curExp / needExp;
    }

    /// <summary>
    /// 經驗值增加
    /// </summary>
    public void ExpIncrease()
    {
        targetLv = entry.UserInfo.Level;
        targetExp = entry.UserInfo.Exp;
        lvUpEffect.SetActive(false);
        StartCoroutine(nameof(IEffect));
    }

    /// <summary>
    /// 更新效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator IEffect()
    {
        float startTime = Time.time;
        float increaseP = tempLv - 1 == 0 ? 0 : Mathf.Pow(2, tempLv - 1);

        //等級不同
        float lvAmount;
        float curProgess = (tempExp - increaseP) / (Mathf.Pow(2, tempLv) - increaseP);
        while (tempLv != targetLv)
        {
            float progress = (Time.time - startTime) / duration;
            lvAmount = Mathf.Lerp(curProgess, 1, progress);
            lvProcess_Img.fillAmount = lvAmount;

            yield return null;

            if (lvProcess_Img.fillAmount == 1)
            {
                lvUpEffect.SetActive(true);

                tempLv = targetLv;                
                tempExp = (int)(Mathf.Pow(2, targetLv - 1));
                increaseP = targetLv - 1 == 0 ? 0 : Mathf.Pow(2, targetLv - 1);
                curProgess = (tempExp - increaseP) / (Mathf.Pow(2, tempLv) - increaseP);
                startTime = Time.time;

                level_Txt.text = $"LV.{targetLv}";
                lvProcess_Img.fillAmount = 0;
            }
        }

        //增加經驗值
        float expAmount;
        float expNeedProgess = (targetExp - increaseP) / (Mathf.Pow(2, targetLv) - increaseP);
        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            expAmount = Mathf.Lerp(curProgess, expNeedProgess, progress);
            lvProcess_Img.fillAmount = expAmount;

            yield return null;
        }

        tempLv = targetLv;
        tempExp = targetExp;

        level_Txt.text = $"LV.{targetLv}";

        float p = targetLv - 1 == 0 ? 0 : Mathf.Pow(2, targetLv - 1);
        float curExp = targetExp - p;
        float needExp = Mathf.Pow(2, targetLv) - p;
        lvProcess_Img.fillAmount = curExp / needExp;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
