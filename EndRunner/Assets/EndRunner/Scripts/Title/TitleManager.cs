using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Common.Yield;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    [Header("StartAnimation")]
    public SpriteRenderer background;
    public Sprite changeSprite;
    public Transform playerTr;
    public SpriteRenderer playerSprite;
    public Transform startMeteorTr;
    public Transform meteor_00Tr;
    public Transform meteor_01Tr;
    public GameObject ui_Root;
    public UIPanel light;
    public GameObject titleLabel;

    [Header("ItemPopupUp")]
    public UIPanel itemupPanel;

    private static bool isfirst = true;
    private bool isFlip = true;
    
    private string leaderboardID_easy = "CgkIpN7RoOwREAIQAw";
    private string leaderboardID_hard = "CgkIpN7RoOwREAIQBA";

    public UILabel testLabel;

    private void Awake()
    {
        instance = this;
        BackkeyManager.isBlock = true;
    }

    public void ShowTestLabel (string txt)
    {
        testLabel.text = testLabel.text + " : " + txt;
    }

    private void Start()
    {
        if (isfirst)
        {
            BackkeyManager.isBlock = true;
            ui_Root.SetActive(false);
            isfirst = false;
            StartCoroutine("CoPlayerFlipx");
            startMeteorTr.DOScale(new Vector2(2.25f, 2.25f), 0.8f);
            startMeteorTr.DOMove(new Vector2(0.13f, 2.5f), 1.3f).OnComplete(() =>
            {
                startMeteorTr.gameObject.SetActive(false);
            });
            playerTr.DOScale(new Vector2(1.5f, 1.5f), 1f).OnComplete(() =>
            {
                DOTween.To(() => 0f, x => light.alpha = x, 1f, 0.7f).OnComplete(() =>
                {
                    background.sprite = changeSprite;
                    DOTween.To(() => 1f, x => light.alpha = x, 0f, 1.5f).OnComplete(() => {
                        ui_Root.SetActive(true);
                        BackkeyManager.isBlock = false;
                        Managers.Sound.PlayBGM(BgmType.Title);
                        if (!PlayerPrefs.HasKey("Exp"))
                        {
                            StartCoroutine("CoExpCheck");
                        }
                    });
                });
                StopCoroutine("CoPlayerFlipx");
                if (isFlip)
                {
                    isFlip = false;
                }
                meteor_00Tr.DOMove(new Vector2(-6.5f, 2f), 0.3f);
                meteor_01Tr.DOMove(new Vector2(5f, 5.3f), 0.5f);
            });
        }
        else {
            BackkeyManager.isBlock = false;
            background.sprite = changeSprite;
            playerTr.localScale = new Vector2(1.5f, 1.5f);
            meteor_00Tr.position = new Vector2(-6.5f, 2f);
            meteor_01Tr.position = new Vector2(5f, 5.3f);
            Managers.Sound.PlayBGM(BgmType.Title);
        }
    }

    IEnumerator CoExpCheck() {
        yield return YieldCache.WaitForSeconds(2.5f);
        PopupContainer.CreatePopup(PopupType.ExpPopup).Init();
        PlayerPrefs.SetString("Exp", "true");
    }

    IEnumerator CoPlayerFlipx() {
        while (true) {
            yield return YieldCache.WaitForSeconds(0.1f);
            playerSprite.flipX = isFlip;
            isFlip = !isFlip;
        }
    }

    public void CharacterPopup() {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.CreatePopup(PopupType.CharacterPopup).Init();
    }

    public void AccessoriesPopup()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.CreatePopup(PopupType.HatPopup).Init();
    }

    public void ChallengePopup()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.CreatePopup(PopupType.ChallengePopup).Init();
    }

    public void RankingPopup() {
        Managers.Sound.PlaySFX(SfxType.Button);
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        Social.ReportScore(saveData.high_score_easy, leaderboardID_easy, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });

        Social.ReportScore(saveData.high_score_hard, leaderboardID_hard, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });

        Social.ShowLeaderboardUI();
    }

    public void SettingPopup()
    {
        Managers.Sound.PlaySFX(SfxType.Button);
        PopupContainer.CreatePopup(PopupType.SettingPopup).Init();
    }

    public void GameStart() {
        Managers.Sound.PlaySFX(SfxType.Button);
        BasePopup curPopup = PopupContainer.GetActivatedPopup();
        if (curPopup != null)
        {
            BackkeyManager.isBlock = true;
            LoadingManager.instance.CartainOnOff(new Vector2(1, 1), true, () => {
                SceneManager.LoadScene("Game");
            });
        }
        else {
            itemupPanel.depth = 6;
            titleLabel.SetActive(false);
            PopupContainer.CreatePopup(PopupType.ItemPopup).Init();
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
