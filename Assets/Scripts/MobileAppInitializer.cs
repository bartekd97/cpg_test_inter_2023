#if UNITY_ANDROID || UNITY_IOS
using UnityEngine;

public static class MobileAppInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Application.targetFrameRate = 60;
    }
}
#endif