using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPoolPanel : BasePanel
{
    [SerializeField]
    private BonusPoolRequest bonusPoolRequest;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
