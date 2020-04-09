using UnityEngine;

// Based on http://wiki.unity3d.com/index.php/Singleton

/// <summary>
/// Be aware this will not prevent a non singleton constructor such as `T myT = new T();` To prevent that, add `protected T () {}` to your singleton class.
/// </summary>
/// 
public abstract class CHEMSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if(_instance != null)
                    {

                    }
                    // Debug.Log("[Singleton] An instance of " + typeof(T) + // 	" is needed in the scene, so '" + singleton + // 	"' was created with DontDestroyOnLoad.");
                }
                return _instance;
            }
        }
    }
}
