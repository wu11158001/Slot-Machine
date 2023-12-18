using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGSManager : BaseManager
{
    public GPGSManager(Entry entry) : base(entry) { }

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
            entry.UserInfo.userId = PlayGamesPlatform.Instance.GetUserId();
            entry.UserInfo.nickName = PlayGamesPlatform.Instance.GetUserDisplayName();
            entry.UserInfo.imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            entry.StartLogin();

            Debug.Log($"用戶登入:{entry.UserInfo.userId}");
        }
        else
        {
            entry.UserInfo.userId = "123456";
            entry.StartLogin();
            entry.ShowTip("Google 登入失敗!!!");
        }
    }

    /// <summary>
    /// 顯示成就
    /// </summary>
    public void ShowAchievement()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    /// <summary>
    /// 解鎖成就
    /// </summary>
    /// <param name="id"></param>
    public void OnLuckAchievement(string id)
    {
        PlayGamesPlatform.Instance.ReportProgress(id, 100.0f, (bool success) => {
            string str = success ? "成就解鎖成功" : "成就解鎖失敗";
            Debug.Log(str);
        });
    }

    /// <summary>
    /// 顯示排行榜
    /// </summary>
    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    /// <summary>
    /// 設定排行榜分數
    /// </summary>
    /// <param name="id"></param>
    public void SetLeaderboard(string id)
    {
        Social.ReportScore(11, id, (bool success) =>
        {
            string str = success ? "設定排行成功" : "設定排行失敗";
            Debug.Log(str);
        });
    }
}
