using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackkeyManager : MonoBehaviour
{
    public static bool isBlock = false;

    private static string sceneName = "";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        GameObject obj = new GameObject("Backkey");
        obj.AddComponent<BackkeyManager>();
        DontDestroyOnLoad(obj);

        SceneManager.sceneLoaded += OnSceneLoadComplete;
    }

    static void OnSceneLoadComplete(Scene scene, LoadSceneMode mode) {
        sceneName = scene.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            Debug.Log(isBlock);
            if (isBlock) {
                return;
            }
            BasePopup curPopup = PopupContainer.GetActivatedPopup();
            switch (sceneName) {
                case "Game":
                    if (curPopup != null)
                    {
                        curPopup.Close();
                        curPopup = PopupContainer.GetActivatedPopup();
                        if (curPopup == null) {
                            GameManager.instance.SetPause();
                        }
                    }
                    else
                    {
                        GameManager.instance.SetPause();
                    }
                    break;
                default:    //기본 로직 : 팝업이 있으면 팝업을 끄고, 팝없이 없으면 게임 종료 팝업 켬
                    if (curPopup != null)
                    {
                        curPopup.Close();
                    }
                    else {
                        PopupContainer.CreatePopup(PopupType.GameEndPopup).Init();
                    }
                    break;
            }
        }
    }
}
