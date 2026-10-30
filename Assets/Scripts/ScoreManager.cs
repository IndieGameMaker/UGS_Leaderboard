using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private TMP_InputField _scoreIF;
    [SerializeField] private Button _scoreSaveButton;
    [SerializeField] private Button _fetchAllScoreButton;

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

            await GetPlayerScores();
        };
        
        // 익명 로그인 요청
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task GetPlayerScores()
    {
        var response = await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
        _scoreIF.text = response.Score.ToString();
    }

    private async Task GetAllScore()
    {
        var response = await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID);
        Debug.Log($"Json: " + JsonConvert.SerializeObject(response));
        Debug.Log("점수 조회 완료");
        
    }
    
    // 버튼 이벤트 연결 메서드
    private void BindingButtonEvents()
    {
        _scoreSaveButton.onClick.AddListener(async () => await AddScore());
        _fetchAllScoreButton.onClick.AddListener(async () => await GetAllScore());
    }

    private async Task AddScore()
    {
        int score = int.Parse(_scoreIF.text);
        var response = await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
        Debug.Log(JsonConvert.SerializeObject(response));
    }
}
