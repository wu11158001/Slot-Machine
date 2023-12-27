using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Threading.Tasks;
using System.Linq;
using System;

public class GameClassicPanel : GameBasePanel
{
    [SerializeField]
    private GameClassicRequest gameClassicRequest;

    [SerializeField]
    private Button home_Btn, spin_Btn, rateAndRule_Btn, betMax_Btn, betPlus_Btn, betMinus_Btn;
    [SerializeField]
    private Text spin_Txt, winScore_Txt, bet_Txt;
    [SerializeField]
    private Transform broadParent;
    [SerializeField]
    private GameObject broadSample, rateObj;

    [SerializeField]
    private UserCoin userCoin;
    [SerializeField]
    private UserLevel userLevel;

    //紀錄贏的編號
    private List<int> winNums = new List<int>();
    //贏得金幣
    private long winCoin;

    //贏分效果時間
    private const float coinEffecDur = 1.0f;

    //轉盤參數
    private const int initDelayTime = 200;
    private int delayTime;
    private bool isSpin;
    private bool isStoping;

    //押注參數
    private int betLevel = 1;
    private long betValue;

    private Coroutine usingCoroutine;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        winScore_Txt.text = "";
        SetBroad(9, broadParent, broadSample);

        SetBetVal();
    }

    public override void OnPause()
    {
        RemoveBroad();
        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        StopAllCoroutines();
        CancelInvoke();
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

        //押注+按鈕
        betPlus_Btn.onClick.AddListener(() =>
        {
            betLevel++;
            SetBetVal();
        });

        //押注-按鈕
        betMinus_Btn.onClick.AddListener(() =>
        {
            betLevel--;
            SetBetVal();
        });

        //押注最大按鈕
        betMax_Btn.onClick.AddListener(() =>
        {
            betLevel = 10;
            SetBetVal();
        });

        //轉盤參數
        spin_Txt.text = "開始";
        delayTime = initDelayTime;
        rateObj.SetActive(false);
    }

    /// <summary>
    /// 設定轉盤狀態
    /// </summary>
    /// <param name="isSpin"></param>
    protected override void SetSpinState(bool isSpin)
    {
        this.isSpin = isSpin;
        base.SetSpinState(isSpin);
    }

    /// <summary>
    /// 設定押注值
    /// </summary>
    private void SetBetVal()
    {
        betValue = Tools.JudgeBetValue(ref betLevel, entry.UserInfo.Coin);
        bet_Txt.text = Tools.SetCoinStr(betValue);
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
        if(entry.UserInfo.Coin - betValue < 0)
        {
            uiManager.ShowTip("金幣數不足!!!");
            return;
        }

        userCoin.ChangeCoin(entry.UserInfo.Coin - betValue);
        gameClassicRequest.SendRequest(betValue);

        if (usingCoroutine != null) StopCoroutine(usingCoroutine);
        SetSpinState(true);
        spin_Txt.text = "停止";
        winScore_Txt.text = "";
        winNums.Clear();
        spin_Btn.interactable = false;

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

        //贏得金幣
        winCoin = pack.ClassicPack.WinCoin;

        //更新用戶訊息
        entry.UserInfo.Coin = pack.UserInfoPack.Coin;
        entry.UserInfo.Exp = pack.UserInfoPack.Exp;
        entry.UserInfo.Level = pack.UserInfoPack.Level;

        Invoke(nameof(SetSpinBtn), 0.7f);
        Invoke(nameof(DelayStopSpin), 2.0f);
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

        //單獨延遲停止事件
        bool isEven = UnityEngine.Random.Range(0, 100) < 100;
        int evenNum = UnityEngine.Random.Range(0, resultDic.Count);

        //停止旋轉
        foreach (var result in resultDic)
        {
            //觸發事件
            if (isEven && result.Key == evenNum)
            {
                EvenEffecf(result.Value);
            }
            else
            {
                result.Value.StopSpin();
                await Task.Delay(delayTime);
            }            
        }
        
        isStoping = false;
        spin_Txt.text = "開始"; 
        delayTime = initDelayTime;
        SetSpinBtn();

        if (!isEven) TurnTableOver();
    }

    /// <summary>
    /// 事件效果
    /// </summary>
    /// <param name="evenBroad"></param>
    private async void EvenEffecf(BroadAction evenBroad)
    {
        await Task.Delay(delayTime * 10);
        evenBroad.StopSpin();
        TurnTableOver();
    }

    /// <summary>
    /// 轉盤結束
    /// </summary>
    private void TurnTableOver()
    {
        userCoin.ChangeCoin(entry.UserInfo.Coin);
        userLevel.ExpIncrease();

        //顯示贏分效果
        if (winNums.Count > 0)
        {
            foreach (var broadAction in resultDic.Values)
            {
                if (winNums.Contains(broadAction.resultNum))
                {
                    broadAction.ShowEffect();
                }
            }

            usingCoroutine = StartCoroutine(nameof(IScoreEffect));
        }

        SetSpinState(false);
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

        usingCoroutine = StartCoroutine(nameof(IScoreEffect));
        spin_Txt.text = "開始";
    }

    /// <summary>
    /// 贏分效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator IScoreEffect()
    {
        float startTime = Time.time;
        int num = 0;

        while (Time.time - startTime < coinEffecDur)
        {
            float progress = (Time.time - startTime) / coinEffecDur;
            num = (int)(winCoin * progress);
            winScore_Txt.text = Tools.SetCoinStr(num);
            yield return null;
        }

        winScore_Txt.text = Tools.SetCoinStr(winCoin);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
}
