using UnityEngine;
using Common.SceneEx;

public sealed class Managers : MonoBehaviour
{
    private static Managers instance;

    public static SoundManager Sound { get { return instance.soundManager; } }
    public static DataService Data { get { return instance.dataService; } }
    public static AdManager Ad { get { return instance.adManager; } }

    private SoundManager soundManager;
    private AdManager adManager;
    private DataService dataService = new DataService();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject go = new GameObject("Managers");
        instance = go.AddComponent<Managers>();

        DontDestroyOnLoad(go);

        SceneJobLoader.Init();

        instance.soundManager = CreateManager<SoundManager>(go.transform);
        instance.adManager = CreateManager<AdManager>(go.transform);
        instance.dataService.InitDataService();
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// 매니저 생성 함수
    /// </summary>
    private static T CreateManager<T>(Transform parent) where T : Component, IInit
    {
        GameObject go = new GameObject(typeof(T).Name);
        T t = go.AddComponent<T>();
        go.transform.parent = parent;

        t.Init();

        return t;
    }

    
}