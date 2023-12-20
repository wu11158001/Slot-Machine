using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotMachineProtobuf;
using System.Threading.Tasks;

public class GameClassicPanel : GameBasePanel
{
    [SerializeField]
    private GameClassicRequest classicRequest;

    [SerializeField]
    private Button home_Btn, spin_Btn;
    [SerializeField]
    private Transform broadParent;
    [SerializeField]
    private GameObject broadSample;

    private bool isSpin;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        SetBroad(9, broadParent, broadSample);
    }

    public override void OnPause()
    {
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
            if (isSpin)
            {

            }
            else
            {
                //isSpin = true;
                StartSpinning();
            }
        });
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
        classicRequest.SendRequest(entry.UserInfo.UserId);

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
        int i = 0;
        foreach (var resultNum in pack.ClassicPack.ResultNums)
        {
            resultDic[i].SetResultNum(resultNum);
            i++;
        }

        Debug.Log($"經典遊戲獲得金幣{pack.ClassicPack.WinCoin}");

        Invoke(nameof(DelayStopSpin), 2);
    }

    /// <summary>
    /// 延遲停止選轉
    /// </summary>
    private async void DelayStopSpin()
    {
        foreach (var result in resultDic.Values)
        {
            result.StopSpin();
            await Task.Delay(200);
        }
    }
}
