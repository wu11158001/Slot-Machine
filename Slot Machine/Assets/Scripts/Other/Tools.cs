using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;
using System;

public static class Tools
{
    /// <summary>
    /// 載入url圖片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<Sprite> ImageUrlToSprite(string url)
    {
        Sprite sprite = null;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        var asyncOperation = www.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Delay(100);//等待100毫秒，避免卡住主線程
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            // 載入成功，將下載的紋理轉換為Sprite
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
            Debug.LogError($"載入url圖片失敗:{www.error}");
        }

        return sprite;
    }

    /// <summary>
    /// 設定金幣文字
    /// </summary>
    /// <param name="coin"></param>
    /// <returns></returns>
    public static string SetCoinStr(long coin)
    {
        StringBuilder sb = new StringBuilder();
        string str = coin.ToString();

        int count = 0;
        for (int i = str.Length - 1; i >= 0; i--)
        {

            sb.Insert(0, str[i]);
            count++;

            if (count == 3 && i != 0)
            {
                count = 0;
                sb.Insert(0, ",");
            }            
        }

        return sb.ToString();
    }

    /// <summary>
    /// 判斷押注金額
    /// </summary>
    /// <param name="level">押注等級</param>
    /// <param name="userCoin">用戶金幣</param>
    /// <returns></returns>
    public static long JudgeBetValue(int level, long userCoin)
    {
        //防呆限制
        if (level <= 0) level = 1;
        else if (level >= 10) level = 10;

        long bet = 0;
        switch (level)
        {
            case 1:
                bet = 60000;
                break;
            case 2:
                bet = 120000;
                break;
            case 3:
                bet = 180000;
                break;
            case 4:
                bet = 240000;
                break;
            case 5:
                bet = 360000;
                break;
            case 6:
                bet = 580000;
                break;
            case 7:
                bet = 770000;
                break;
            case 8:
                bet = 1000000;
                break;
            case 9:
                bet = 1200000;
                break;
            case 10:
                bet = 2400000;
                break;
        }

        return bet;
    }
}
