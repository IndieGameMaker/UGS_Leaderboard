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
    }
}
