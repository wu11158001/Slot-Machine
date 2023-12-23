using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Threading.Tasks;
using System.Linq;

public class GameClassicPanel : GameBasePanel
{
    [SerializeField]
    private GameClassicRequest classicRequest;

    [SerializeField]
    private Button home_Btn, spin_Btn, rateAndRule_Btn;
    [SerializeField]
    private Text spin_Txt, winScore_Txt;
    [SerializeField]
    private Transform broadParent;
    [SerializeField]
    private GameObject broadSample, rateObj;

    //紀錄贏的編號
    private List<int> winNums = new List<int>();
    //贏得賠率
    private float winRate;

    private const int initDelayTime = 200;
    private int delayTime;
    private bool isSpin;
    private bool isStoping;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        SetBroad(9, broadParent, broadSample);
    }

    public override void OnPause()
    {
        RemoveBroad();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        //返回大廳
        home_Btn.onClick.AddListener(() =>
        {
            uiManager.PushPanel(PanelType.HallPanel);
        });

        //旋轉按鈕
        spin_Btn.onClick.AddListener(() =>
        {
            if (isSpin) StopSpin();
            else StartSpinning();
        });

        //賠率與規則按鈕
        rateAndRule_Btn.onClick.AddListener(()=>
        {
            rateObj.SetActive(true);
        });

        delayTime = initDelayTime;
        rateObj.SetActive(false);
    }

    /// <summary>
    /// 設定轉盤
    /// </summary>
    /// <param name="count"></param>
    /// <param name="parent"></param>
    /// <param name="obj"></param>
    protected override void SetBroad(int count, Transform parent, GameObject obj)
    {
        base.SetBroad(count, parent, obj);
    }

    /// <summary>
    /// 開始旋轉
    /// </summary>
    private void StartSpinning()
    {
        classicRequest.SendRequest();

        isSpin = true;
        spin_Txt.text = "停止";
        spin_Btn.interactable = false;
        winScore_Txt.text = "";
        winNums.Clear();

        for (int i = 0; i < resultDic.Count; i++)
        {
            resultDic[i].StartSpinning();
        }
    }

    /// <summary>
    /// 獲取服務端結果
    /// </summary>
    /// <param name="pack"></param>
    public void GetResult(MainPack pack)
    {
        //結果編號
        int i = 0;
        foreach (var resultNum in pack.ClassicPack.ResultNums)
        {
            resultDic[i].resultNum = resultNum;
            i++;
        }

        //贏的編號
        foreach (var num in pack.ClassicPack.WinNums)
        {
            winNums.Add(num);
        }

        //贏得賠率
        winRate = (float)pack.ClassicPack.WinRate / 100;

        Invoke(nameof(SetSpinBtn), 1);
        Invoke(nameof(DelayStopSpin), 2);
    }

    /// <summary>
    /// 設定旋轉按鈕
    /// </summary>
    private void SetSpinBtn()
    {
        spin_Btn.interactable = true;
    }

    /// <summary>
    /// 延遲停止選轉
    /// </summary>
    private async void DelayStopSpin()
    {
        isStoping = true;
        foreach (var result in resultDic.Values)
        {
            result.StopSpin();
            await Task.Delay(delayTime);
        }

        isSpin = false;
        isStoping = false;
        spin_Txt.text = "開始";
        spin_Btn.interactable = true;        
        delayTime = initDelayTime;

        //顯示效果
        if (winNums.Count > 0)
        {
            foreach (var broadAction in resultDic.Values)
            {
                if(winNums.Contains(broadAction.resultNum))
                {
                    broadAction.ShowEffect();
                }
            }

            WinScoreEffect();
        }
    }

    /// <summary>
    /// 停止旋轉
    /// </summary>
    private void StopSpin()
    {
        if (isStoping)
        {
            delayTime = 0;
        }
        else
        {
            delayTime = 0;
            CancelInvoke();
            DelayStopSpin();
        }       

        isSpin = false;
        spin_Txt.text = "開始";
    }

    /// <summary>
    /// 贏分效果
    /// </summary>
    private void WinScoreEffect()
    {

    }
}
