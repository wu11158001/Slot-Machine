using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;

public class BonusPoolManager : BaseManager
{
    //紀錄獎池訊息(獎池,獎池金幣)
    private Dictionary<string, long> bonusInfoDic = new Dictionary<string, long>();
    //紀錄各獎池(獎池類型,List)
    private Dictionary<BonusPoolType, List<GameBonusPool>> poolDic = new Dictionary<BonusPoolType, List<GameBonusPool>>();

    public override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 設定所有獎池訊息
    /// </summary>
    /// <param name="pack"></param>
    public void SetAllBonusInfo(MainPack pack)
    {
        //所有獎池
        foreach (var pool in pack.BonusPoolPack.AllPool)
        {
            if (bonusInfoDic.ContainsKey(pool.Key))
            {
                bonusInfoDic[pool.Key] = pool.Value;
            }
            else
            {
                bonusInfoDic.Add(pool.Key, pool.Value);
            }            
        }

        //各別獎池
        if (!string.IsNullOrEmpty(pack.BonusPoolPack.GameName))
        {
            //獎池更新
            if (bonusInfoDic.ContainsKey(pack.BonusPoolPack.GameName))
            {
                bonusInfoDic[pack.BonusPoolPack.GameName] = pack.BonusPoolPack.BonusValue;
            }

            //UI更新
            foreach (var poolType in poolDic)
            {
                if (poolType.Key.ToString() == pack.BonusPoolPack.GameName)
                {
                    foreach (var pool in poolType.Value)
                    {
                        pool.SetValue();
                    }
                }
            }
        }

        //設定廣播贏家
        if (!string.IsNullOrEmpty(pack.BonusPoolPack.WinNickName))
        {
            entry.SetBroadcastWinner(pack);
        }
    }

    /// <summary>
    /// 添加獎池
    /// </summary>
    /// <param name="baseBonus"></param>
    public void AddBonusPool(GameBonusPool baseBonus)
    {
        baseBonus.SetBonusPoolManager = this;

        if (poolDic.ContainsKey(baseBonus.bonusType))
        {
            poolDic[baseBonus.bonusType].Add(baseBonus);
        }
        else
        {
            poolDic.Add(baseBonus.bonusType, new List<GameBonusPool>() { baseBonus });
        }
    }

    /// <summary>
    /// 獲取獎池訊息
    /// </summary>
    /// <param name="poolType"></param>
    public long GetBonusValue(BonusPoolType poolType)
    {
        long value = 0;
        if (bonusInfoDic.TryGetValue(poolType.ToString(), out long val))
        {
            value = val;
        }
        return value;
    }
}
