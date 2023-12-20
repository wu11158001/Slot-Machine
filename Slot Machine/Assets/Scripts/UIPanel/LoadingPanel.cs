using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    [SerializeField]
    private Image process_Img;

    [SerializeField]
    private Text process_Txt;

    public override void OnEnter()
    {
        uiManager.SetLoadingPanel = this;        
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 載入畫面
    /// </summary>
    public void Loading()
    {
        transform.SetSiblingIndex(100);
        gameObject.SetActive(true);
        StartCoroutine(nameof(IProcess));
    }

    /// <summary>
    /// 載入進度
    /// </summary>
    /// <returns></returns>
    private IEnumerator IProcess()
    {
        process_Img.fillAmount = 0;
        float process = 0;

        while(process_Img.fillAmount < 1)
        {
            process_Img.fillAmount += Time.deltaTime;

            process = process_Img.fillAmount == 1 ? 100 : (process_Img.fillAmount % 1) * 100;
            process_Txt.text = $"{process.ToString("F0")}%";
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
