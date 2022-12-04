using UnityEngine;

public static class AndroidUtil
{
    private static AndroidJavaObject activity;
    private static AndroidJavaObject analyticsManager;

    public static AndroidJavaObject Activity
    {
        get
        {
            if (activity == null)
            {
                activity = GetActivity();
            }

            return activity;
        }
    }

    private static AndroidJavaObject GetActivity()
    {
        using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return actClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}