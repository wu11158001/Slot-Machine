using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Linq;

public class ClassicRate : BasePanel
{
    [SerializeField]
    private ClassicRateRequest classicRateRequest;

    [SerializeField]
    private GameObject[] RatePages;
    [SerializeField]
    private RectTransform appearParent, lineParent;
    [SerializeField]
    private GameObject appearSample, lineSample;
    [SerializeField]
    private Sprite[] resultSprites;
    [SerializeField]
    private Button rateLeft_Btn, rateRight_Btn, back_Btn;

    //經典玩法賠率_出現數量(編號,賠率List)
    private Dictionary<int, List<int>> classicAppearRate = new Dictionary<int, List<int>>();
    //經典玩法賠率_連線(編號,賠率)
    private Dictionary<int, int> classicLineRate = new Dictionary<int, int>();

    private int curPage;
    private GameObject prePage;

    private void Start()
    {
        classicRateRequest.SendRequest();

        rateLeft_Btn.onClick.AddListener(delegate { ChangePage(-1); });
        rateRight_Btn.onClick.AddListener(delegate { ChangePage(1); });
        back_Btn.onClick.AddListener(() =>
        {
            entry.PlaySound(SoundType.ButtonClick);
            gameObject.SetActive(false);
        });


        RatePages[0].SetActive(true);
        prePage = RatePages[0];
        for (int i = 1; i < RatePages.Length; i++)
        {
            RatePages[i].SetActive(false);
        }
    }

    /// <summary>
    /// 更換頁面
    /// </summary>
    /// <param name="dic"></param>
    private void ChangePage(int dic)
    {
        entry.PlaySound(SoundType.ButtonClick);
        dic = dic >= 0 ? 1 : -1;

        curPage += dic;
        if (curPage >= RatePages.Length) curPage = 0;
        else if (curPage < 0) curPage = RatePages.Length - 1;

        RatePages[curPage].SetActive(true);
        prePage.SetActive(false);
        prePage = RatePages[curPage];
    }

    /// <summary>
    /// 設定賠率
    /// </summary>
    /// <param name="pack"></param>
    public void SetRate(MainPack pack)
    {
        //連線
        foreach (var keyValue in pack.ClassicRatePack.Line)
        {
            classicLineRate[keyValue.Key] = keyValue.Value;

            GameObject lineObj = Instantiate(lineSample);
            lineObj.transform.SetParent(lineParent);
            Image img = lineObj.transform.Find($"Icon_Img").GetComponent<Image>();
            img.sprite = resultSprites[keyValue.Key];
            Text txt = lineObj.transform.Find($"Rate_Txt").GetComponent<Text>();
            txt.text = $"<color=#FFFFFF>3</color><color=#FFEC01>   {keyValue.Value}</color>";
        }
        lineSample.SetActive(false);

        //出現數量
        foreach (var dic in pack.ClassicRatePack.Appear)
        {
            int key = dic.Key;
            List<int> values = dic.Value.Values.ToList();
            classicAppearRate[key] = values;

            GameObject appearObj = Instantiate(appearSample);
            appearObj.transform.SetParent(appearParent);
            Image img = appearObj.transform.Find($"Icon_Img").GetComponent<Image>();
            img.sprite = resultSprites[key];
            Text[] txts = new Text[7];
            for (int i = 0; i < txts.Length; i++)
            {
                txts[i] = appearObj.transform.Find($"Txt/{i + 3}_Txt").GetComponent<Text>();
                txts[i].text = classicAppearRate[key][i].ToString();
            }
        }
        appearSample.SetActive(false);
    }
}
