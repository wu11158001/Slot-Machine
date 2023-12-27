using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGSManager : BaseManager
{
    //遊玩次數
    private const int targetPlayTime = 5;
    private int playTime;

    //獲得目標金幣
    private const long targetCoin = 100000;

    public override void OnInit()
    {
        base.OnInit();
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    /// <summary>
    /// 登入Google
    /// </summary>
    /// <param name="status"></param>
    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            entry.UserInfo.UserId = PlayGamesPlatform.Instance.GetUserId();
            entry.UserInfo.NickName = PlayGamesPlatform.Instance.GetUserDisplayName();
            entry.UserInfo.ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            entry.StartLogin();

            OnLuckAchievement(GPGSIds.achievement_first_login, 100.0f);

            Debug.Log($"用戶登入:{entry.UserInfo.UserId}");
        }
        else
        {
            entry.ShowTip("Google 登入失敗!!!");
        }
    }

    /// <summary>
    /// 判斷是否以驗證
    /// </summary>
    /// <returns></returns>
    public bool IsAuthenticated()
    {
        bool isAuthenticated = PlayGamesPlatform.Instance.IsAuthenticated();

        if (!isAuthenticated)
        {
            entry.ShowTip("Google未登入!!!");
        }

        return isAuthenticated;
    }

    /// <summary>
    /// 顯示成就
    /// </summary>
    public void ShowAchievement()
    {
        if (!IsAuthenticated()) return;

        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    /// <summary>
    /// 解鎖成就
    /// </summary>
    /// <param name="id"></param>
    /// <param name="val"></param>
    public void OnLuckAchievement(string id, float val)
    {
        if (!IsAuthenticated()) return;

        PlayGamesPlatform.Instance.ReportProgress(id, 100.0f, (bool success) => {
            string str = success ? "成就解鎖成功" : "成就解鎖失敗";
            Debug.Log(str);
        });
    }

    /// <summary>
    /// 解鎖遊玩次數
    /// </summary>
    public void OnLuckPlayTime()
    {
        if (!IsAuthenticated()) return;

        playTime++;
        OnLuckAchievement(GPGSIds.achievement_play_five_time, (float)playTime / targetPlayTime);
    }

    /// <summary>
    /// 解鎖獲得金幣
    /// </summary>
    /// <param name="coin"></param>
    public void OnLuckGetCoin(long coin)
    {
        if (!IsAuthenticated()) return;

        if (coin >= targetCoin)
        {
            OnLuckAchievement(GPGSIds.achievement_first_time_get_100000, 100.0f);
        }
    }

    /// <summary>
    /// 顯示排行榜
    /// </summary>
    public void ShowLeaderboard()
    {
        if (!IsAuthenticated()) return;

        Social.ShowLeaderboardUI();
    }

    /// <summary>
    /// 設定排行榜分數
    /// </summary>
    /// <param name="id">GPGSId</param>
    /// <param name="val">更換植</param>
    public void SetLeaderboard(string id, long val) 
    {
        if (!IsAuthenticated()) return;

        Social.ReportScore(val, id, (bool success) =>
        {
            string str = success ? "設定排行成功" : "設定排行失敗";
            Debug.Log(str);
        });
    }
}
