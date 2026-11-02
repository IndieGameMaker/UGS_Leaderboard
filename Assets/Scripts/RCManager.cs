using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;

public class RCManager : MonoBehaviour
{
    [SerializeField] private float move_speed;
    [SerializeField] private float rotate_speed;
    [SerializeField] private int gold;
    
    private async Task Awake()
    {
        await UnityServices.InitializeAsync();

        // 로그인 이벤트 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"로그인 성공: PlayerID: {AuthenticationService.Instance.PlayerId}");
        };
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Remote Config 에서 데이터 패치가 완료되었을 때 호출되는 이벤트 연결
        RemoteConfigService.Instance.FetchCompleted += OnFetchRemoteConfig;
        
        await GetRemoteConfigData();
    }

    private void OnFetchRemoteConfig(ConfigResponse obj)
    {
        
    }

    // Filter 역할
    public struct userAttr {};
    public struct appAttr {};

    private async Task GetRemoteConfigData()
    {
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttr(), new appAttr());
    }
    
}
