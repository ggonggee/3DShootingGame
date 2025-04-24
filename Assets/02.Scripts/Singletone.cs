using UnityEditorInternal;
using UnityEngine;

public class Singleton<T> :MonoBehaviour where T : Component
{
    protected static T _instance;

    public static T Instance 
    { 
        get 
        {
            _instance = FindAnyObjectByType<T>();
            return _instance; 
        } 
    }


    public void Initialize_DontDesTroyOnLoad()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

       
}
