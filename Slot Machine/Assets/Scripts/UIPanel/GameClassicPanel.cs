using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClassicPanel : BasePanel
{
    [SerializeField]
    private Button home_Btn;

    public override void OnEnter()
    {
        gameObject.SetActive(true);
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
    }
}
