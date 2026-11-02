using System;
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

    [SerializeField] private float body_scale;
    [SerializeField] private Transform _mummy;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
    
    private async void Start()
    {
        // 로그인 이벤트 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"로그인 성공: PlayerID: {AuthenticationService.Instance.PlayerId}");
        };        
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await GetRemoteConfigData();
    }

    private void OnEnable()
    {
        // Remote Config 에서 데이터 패치가 완료되었을 때 호출되는 이벤트 연결
        RemoteConfigService.Instance.FetchCompleted += OnFetchRemoteConfig;
    }

    private void OnDisable()
    {
        RemoteConfigService.Instance.FetchCompleted -= OnFetchRemoteConfig;
    }

    private void OnFetchRemoteConfig(ConfigResponse obj)
    {
        Debug.Log("Remote Config 데이터 패치 완료");
        move_speed = RemoteConfigService.Instance.appConfig.GetFloat("move_speed");
        rotate_speed = RemoteConfigService.Instance.appConfig.GetFloat("rotate_speed");
        gold = RemoteConfigService.Instance.appConfig.GetInt("gold");
        body_scale = RemoteConfigService.Instance.appConfig.GetFloat("body_scale");
        
        _mummy.localScale = Vector3.one * body_scale;
    }

    // Filter 역할
    public struct userAttr {};
    public struct appAttr {};

    private async Task GetRemoteConfigData()
    {
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttr(), new appAttr());
    }
    
}
