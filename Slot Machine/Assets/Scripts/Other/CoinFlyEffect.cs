using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlyEffect : BasePanel
{
    [SerializeField]
    private Transform startObj;
    [SerializeField]
    private Transform targetObj;
    [SerializeField]
    private GameObject createCoinObj;

    [SerializeField]
    private UserCoin userCoin;

    //產生1金幣需求
    private const long CreateCoinNeed = 100000;
    //飛行時間
    private const float flyTime = 3f;

    //產生數量
    private int createCount;
    //金幣增加量
    private long increaseCoin;
    //計數器
    private int count;

    /// <summary>
    /// 金幣飛行效果開始
    /// </summary>
    /// <param name="coinVal"></param>
    public void CoinEffectStart(long coinVal)
    {
        count = 0;
        increaseCoin = coinVal;
        createCount = (int)(coinVal / CreateCoinNeed);
        StartCoroutine(ICreateCoin());
        StartCoroutine(ISetUserInfo());
    }

    /// <summary>
    /// 產生金幣
    /// </summary>
    /// <returns></returns>
    private IEnumerator ICreateCoin()
    {
        yield return new WaitUntil(() => StateManger.isAdActing == false);

        for (int i = 0; i < createCount; i++)
        {
            Transform obj = Instantiate(createCoinObj).GetComponent<Transform>();
            obj.SetParent(startObj);
            obj.position = startObj.position;
            StartCoroutine(IDelayFly(obj));

            float nextTime = Random.Range(0.1f, 0.2f);
            yield return new WaitForSeconds(nextTime);
        }        
    }

    /// <summary>
    /// 延遲飛行
    /// </summary>
    /// <param name="obj"></param>
    private IEnumerator IDelayFly(Transform obj)
    {
        yield return new WaitForSeconds(0.5f);

        float startTime = Time.time;

        while (Time.time - startTime < flyTime &&
            (obj.position.x > targetObj.position.x + 1 || obj.position.x < targetObj.position.x - 1) &&
            (obj.position.y > targetObj.position.y + 1 || obj.position.y < targetObj.position.y - 1))
        {
            float progress = (Time.time - startTime) / flyTime;
            float posX = Mathf.Lerp(obj.position.x, targetObj.position.x, progress);
            float posY = Mathf.Lerp(obj.position.y, targetObj.position.y, progress);
            obj.position = new Vector2(posX, posY);

            yield return null;
        }

        count++;
        long effectVal = entry.UserInfo.Coin + (increaseCoin / createCount) * count;
        userCoin.NoEffect(effectVal);
        entry.PlaySound(SoundType.Coin);
        Destroy(obj.gameObject);
    }

    /// <summary>
    /// 設定用戶訊息
    /// </summary>
    /// <returns></returns>
    private IEnumerator ISetUserInfo()
    {
        yield return new WaitUntil(() => count == createCount);

        entry.UserInfo.Coin += increaseCoin;
        userCoin.UpdateValue();
    }
}
