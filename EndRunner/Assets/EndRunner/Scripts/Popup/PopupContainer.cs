using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupContainer : MonoBehaviour
{
    private static List<BasePopup> basePopupList = new List<BasePopup>();
    private static Transform uiRootTr;

    static void OnSceneLoadComplete(Scene scene, LoadSceneMode mode) {
        basePopupList = new List<BasePopup>();
        uiRootTr = null;
        GameObject uiRootObj = GameObject.Find("UI Root");
        if (uiRootObj == null) {
            Debug.LogError("Can not Find UI Root GameObject");
            return;
        }
        uiRootTr = uiRootObj.transform;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        SceneManager.sceneLoaded += OnSceneLoadComplete;
    }

    public static BasePopup CreatePopup(PopupType type) {
        GameObject popupPrefab = Resources.Load<GameObject>(string.Format("Prefabs/Popup/{0}", type.ToString()));
        if (popupPrefab == null)
        {
            Debug.LogError("Can not Find popupPrefab");
            return null;
        }
        GameObject popupObj = Instantiate(popupPrefab, uiRootTr);
        return popupObj.GetComponent<BasePopup>();
    }

    public static int Pop(BasePopup basePopup, bool isOverlay = true) {
        if (isOverlay == false && basePopupList.Count > 0) {
            basePopupList[basePopupList.Count - 1].gameObject.SetActive(false);
        }
        basePopupList.Add(basePopup);
        return basePopupList.Count;
    }

    public static void Close() {
        basePopupList.RemoveAt(basePopupList.Count - 1);
        if (basePopupList.Count > 0) {
            basePopupList[basePopupList.Count - 1].gameObject.SetActive(true);
        }
    }

    public static BasePopup GetActivatedPopup(int popCount = 1) {
        if (basePopupList.Count == 0)
        {
            return null;
        }
        else {
            return basePopupList[basePopupList.Count - popCount];
        }
    }

}

public enum PopupType {

    GameContinuePopup,
    GameOverPopup,
    PausePopup,
    CharacterPopup,
    HatPopup,
    EyePopup,
    TiePopup,
    ItemPopup,
    GameEndPopup,
    ChallengePopup,
    SettingPopup,
    ConfirmationPopup,
    ExpPopup,

}
