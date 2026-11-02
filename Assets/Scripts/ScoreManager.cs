using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private TMP_InputField _scoreIF;
    [SerializeField] private Button _scoreSaveButton;
    [SerializeField] private Button _fetchAllScoreButton;
    [SerializeField] private Button _deleteUserButton;

    [SerializeField] private Button _pageScoreButton;
    [SerializeField] private TMP_InputField _pageIF;

    private const string LEADERBOARD_ID = "Ranking";
    
    private async void Start()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("UGS 초기화 완료");

        await SignIn();
        BindingButtonEvents();
    }
    
    private async Task SignIn()
    {
        // 로그인 성공시 호출되는 이벤트에 연결
        AuthenticationService.Instance.SignedIn += async () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"로그인 완료: {playerId}");

            //await GetPlayerScores();
        };
        
        // 익명 로그인 요청
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task GetPlayerScores()
    {
        var response = await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
        _scoreIF.text = response.Score.ToString();
    }

    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    
    private async Task GetAllScore()
    {
        var response = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID);
        Debug.Log($"Json: " + JsonConvert.SerializeObject(response));
        Debug.Log("점수 조회 완료");

        entries = response.Results;

        string rank = "";
        foreach (var entry in entries)
        {
            rank += $"[{entry.Rank + 1}] {entry.PlayerId} : {entry.Score}\n";
        }
        Debug.Log(rank);        
    }
    
    // 버튼 이벤트 연결 메서드
    private void BindingButtonEvents()
    {
        _scoreSaveButton.onClick.AddListener(async () => await AddScore());
        _fetchAllScoreButton.onClick.AddListener(async () => await GetAllScore());
        _deleteUserButton.onClick.AddListener(async () =>
        {
            await AuthenticationService.Instance.DeleteAccountAsync();
            Debug.Log("로그인 유저 삭제 완료");
        });
        
        _pageScoreButton.onClick.AddListener(async () =>
        {
            int pageIdx = int.Parse(_pageIF.text) * 5;
            // Offset : 건너뛰는 갯수 / Limit : 추출 갯수
            // Offset = Limit * pageIdx
            var scoreOptions = new GetScoresOptions { Offset = pageIdx, Limit = 5};
            var response = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID,scoreOptions);
            Debug.Log(JsonConvert.SerializeObject(response));
        });
    }

    private async Task AddScore()
    {
        int score = int.Parse(_scoreIF.text);
        var response = await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
        Debug.Log(JsonConvert.SerializeObject(response));
    }
    // 1, 2, 3, 4, 5, 6
}


// 1,2,3,4,5