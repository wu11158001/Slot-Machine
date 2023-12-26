using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotMachineProtobuf;
using System.Threading.Tasks;
using UnityEngine.UI;

public class BonusPoolPanel : BasePanel
{
    [SerializeField]
    private BonusPoolRequest bonusPoolRequest;

    private RectTransform thisRt;

    [SerializeField]
    private RectTransform broadcastObj, bigWinObj;
    [SerializeField]
    private Button broadcast_Btn, bigWinConfirm_Btn;
    [SerializeField]
    private Text nickName_Txt, gameName_Txt, bonusVal_Txt, bigWinCoin_Txt;
    [SerializeField]
    private Image avatar_Img;

    private float broadcastSize;
    
    private const float broadcastMoveTime = 0.7f;
    private const float bigWinTime = 3;

    /// <summary>
    /// 贏家資料
    /// </summary>
    public class WinnerDate
    {
        public string NickName { get; set; }
        public string ImgUrl { get; set; }
        public string GameName { get; set; }
        public long WinCoinVal { get; set; }
    }
    private List<WinnerDate> winnerList = new List<WinnerDate>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        thisRt = GetComponent<RectTransform>();

        broadcastSize = broadcastObj.rect.width;
        broadcastObj.gameObject.SetActive(false);
        bigWinObj.gameObject.SetActive(false);
    }

    /// <summary>
    /// 設定廣播贏家
    /// </summary>
    /// <param name="pack"></param>
    public void SetBroadcastWinner(MainPack pack)
    {
        thisRt.SetSiblingIndex(100);

        //用戶中獎
        if (pack.BonusPoolPack.WinId == entry.UserInfo.UserId)
        {
            BigWinEffect(pack.BonusPoolPack.WinValue);
            return;
        }

        WinnerDate winnerDate = new WinnerDate()
        {
            NickName = pack.BonusPoolPack.WinNickName,
            ImgUrl = pack.BonusPoolPack.WinImgUrl,
            GameName = pack.BonusPoolPack.GameName,
            WinCoinVal = pack.BonusPoolPack.WinValue
        };

        winnerList.Add(winnerDate);

        if (winnerList.Count == 1) BroadcastWinner();
    }

    /// <summary>
    /// 中獎效果
    /// </summary>
    /// <param name="winCoin"></param>
    private async void BigWinEffect(long winCoin)
    {
        try
        {
            bigWinObj.gameObject.SetActive(true);
            bigWinConfirm_Btn.onClick.AddListener(() =>
            {
                bigWinObj.gameObject.SetActive(false);
            });

            //移動效果
            float startTime = Time.time;
            long val = 0;
            while (Time.time - startTime < bigWinTime)
            {
                float progress = (Time.time - startTime) / bigWinTime;
                val = (long)Mathf.Round(Mathf.Lerp(0, winCoin, progress));
                bigWinCoin_Txt.text = $"{Tools.SetCoinStr(val)}";
                await Task.Delay(1);
            }

            bigWinCoin_Txt.text = $"{winCoin}";
        }
        catch (System.Exception)
        {

        }        
    }

    /// <summary>
    /// 廣播贏家
    /// </summary>
    private async void BroadcastWinner()
    {
        try
        {
            while (winnerList.Count > 0)
            {
                broadcast_Btn.onClick.AddListener(() =>
                {
                    PanelType panelType = PanelType.GameClassicPanel;
                    switch (winnerList[0].GameName)
                    {
                        case "classic":
                            panelType = PanelType.GameClassicPanel;
                            break;
                        default:
                            Debug.LogError("廣播贏家,沒有找到面板!!!");
                            break;
                    }

                    entry.EnterGame(panelType);
                });
                nickName_Txt.text = winnerList[0].NickName;
                gameName_Txt.text = winnerList[0].GameName;
                bonusVal_Txt.text = $"{Tools.SetCoinStr(winnerList[0].WinCoinVal)}";
                avatar_Img.sprite = await Tools.ImageUrlToSprite(winnerList[0].ImgUrl);
                broadcastObj.gameObject.SetActive(true);

                //移動效果
                float startTime = Time.time;
                float posX = 0;
                while (Time.time - startTime < broadcastMoveTime)
                {
                    float progress = (Time.time - startTime) / broadcastMoveTime;
                    posX = (long)Mathf.Round(Mathf.Lerp(broadcastSize, 0, progress));
                    broadcastObj.anchoredPosition = new Vector2(posX, broadcastObj.anchoredPosition.y);
                    await Task.Delay(1);
                }
                
                await Task.Delay(3000);

                winnerList.RemoveAt(0);
                broadcastObj.gameObject.SetActive(false);
            }
        }
        catch (System.Exception)
        {
            throw;
        }        
    }
}
