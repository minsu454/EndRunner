using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    private GameObject curtainObj;
    private GameObject maskObj;

    public void OnCompleteSceneLoad(Scene scene, LoadSceneMode mode)
    {
        curtainObj = GameObject.Find("UI Root").transform.Find("Curtain").gameObject;
        LoadingManager.instance.maskObj = curtainObj.transform.Find("CurtainSprite").gameObject;
    }

    public void CartainOnOff(Vector2 scale, bool isOn, System.Action callback = null) {
        maskObj.SetActive(true);
        curtainObj.SetActive(true);
        Transform maskTr = maskObj.GetComponent<Transform>();

        maskTr.DOScale(scale, 0.75f).OnComplete(() =>
        {
            maskObj.SetActive(false);
            if (callback != null)
            {
                if (!isOn) {
                    curtainObj.SetActive(false);
                }
                callback();
            }
            else {
                curtainObj.SetActive(false);
            }
        });
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitLoadingManager() {
        GameObject loadingManagerObj = new GameObject("LoadingManager");
        LoadingManager.instance = loadingManagerObj.AddComponent<LoadingManager>();
        DontDestroyOnLoad(loadingManagerObj);

        SceneManager.sceneLoaded += LoadingManager.instance.OnCompleteSceneLoad;
    }

}
