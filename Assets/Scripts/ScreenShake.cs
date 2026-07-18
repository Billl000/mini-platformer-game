using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance { get; private set; }
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Shake(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}
