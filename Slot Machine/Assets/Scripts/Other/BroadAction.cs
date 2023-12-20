using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using SlotMachineProtobuf;

public class BroadAction : MonoBehaviour
{
    [SerializeField]
    private RectTransform resultParent;
    [SerializeField]
    private GameObject result_Img;

    [SerializeField]
    private Sprite[] resultSprites;

    private bool isSpin;        //旋轉控制
    private float size;         //結果圖片大小
    private DateTime startTime; //開始旋轉時間
    private int resultNum;      //服務端回傳結果編號

    //編號,(轉盤物件,結果圖片)
    [SerializeField]
    private Dictionary<int, (RectTransform, Image)> resultDic = new Dictionary<int, (RectTransform, Image)>();
    //紀錄已使用的結果編號
    private List<int> usingResult = new List<int>();

    private void Start()
    {
        Initializer();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Initializer()
    {
        size = resultParent.rect.height;

        //產生轉盤內容
        for (int i = -1; i <= 1; i++)
        {
            RectTransform obj = Instantiate(result_Img).GetComponent<RectTransform>();
            obj.transform.SetParent(resultParent);
            obj.gameObject.SetActive(true);
            obj.anchoredPosition = new Vector2(0, size * i);

            Image img = obj.GetComponent<Image>();
            img.sprite = resultSprites[GetResultNum()];
            resultDic.Add(i, (obj,img));
        }
        result_Img.SetActive(false);
    }

    /// <summary>
    /// 設定結果
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private int GetResultNum(int? num = null)
    {
        //已有結果編號
        if(num != null)
        {
            int n = (int)num;
            usingResult.Add(n);
            return n;
        }

        //未有結果編號
        bool isFind = false;
        int result = 0;
        while(!isFind)
        {
            result = UnityEngine.Random.Range(0, resultSprites.Length - 1);
            if(!usingResult.Contains(result))
            {
                usingResult.Add(result);
                isFind = true;
            }
        }

        return result;
    }

    /// <summary>
    /// 服務端回傳結果
    /// </summary>
    /// <param name="num"></param>
    public void SetResultNum(int num)
    {
        resultNum = num;
    }

    /// <summary>
    /// 開始旋轉
    /// </summary>
    public void StartSpinning()
    {
        isSpin = true;
        startTime = DateTime.Now ;
        usingResult.Clear();
        resultNum = 1;
        
        Spinning();
    }

    /// <summary>
    /// 選轉
    /// </summary>
    private async void Spinning()
    {
        while (isSpin)
        {
            foreach (var result in resultDic)
            {
                float posY = result.Value.Item1.anchoredPosition.y - 1000 * Time.deltaTime;
                result.Value.Item1.anchoredPosition = new Vector2(0, posY);

                if (result.Value.Item1.anchoredPosition.y <= -size)
                {
                    result.Value.Item1.anchoredPosition = new Vector2(0, size);
                    result.Value.Item2.sprite = resultSprites[UnityEngine.Random.Range(0, resultSprites.Length)];
                }
            }
            await Task.Delay(1);
        }
    }

    /// <summary>
    /// 選轉停止
    /// </summary>
    public async void StopSpin()
    {
        //設定結果
        isSpin = false;
        int index = GetResultNum(resultNum);
        resultDic[0].Item2.sprite = resultSprites[index];       
        
        for (int i = -1; i <= 1; i++)
        {
            resultDic[i].Item1.anchoredPosition = new Vector2(0, size * i);
            if(i != 0)
            {
                resultDic[i].Item2.sprite = resultSprites[GetResultNum()];
            }
        }

        //煞車效果
        while (resultDic[0].Item1.anchoredPosition.y > -30)
        {
            foreach (var result in resultDic.Values)
            {
                float posY = result.Item1.anchoredPosition.y - 200 * Time.deltaTime;
                result.Item1.anchoredPosition = new Vector2(0, posY);
            }
            await Task.Delay(1);
        }

        await Task.Delay(150);

        while (resultDic[0].Item1.anchoredPosition.y < 0)
        {
            foreach (var result in resultDic.Values)
            {
                float posY = result.Item1.anchoredPosition.y + 200 * Time.deltaTime;
                result.Item1.anchoredPosition = new Vector2(0, posY);
            }
            await Task.Delay(1);
        }

        for (int i = -1; i <= 1; i++)
        {
            resultDic[i].Item1.anchoredPosition = new Vector2(0, size * i);
        }
    }
}
