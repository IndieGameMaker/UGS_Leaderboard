using System.Threading.Tasks;
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
    
    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();
        Debug.Log("UGS 초기화 완료");

        await SignIn();
    }

    private async Task SignIn()
    {
        // 로그인 성공시 호출되는 이벤트에 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"로그인 완료: {playerId}");
        };
        
        // 익명 로그인 요청
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
