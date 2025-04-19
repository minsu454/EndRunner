using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [Header("Barrier")]
    public GameObject barrierObj;

    [Header("Mana")]
    public UISlider manaBarSlider;
    private int curManaPoint = 0;
    private int maxManaPoint = 300;

    [Header("Controll")]
    public GameObject controllManager;
    public GameObject controllParent;

    [Header("Ad")]
    private bool isShowAd = false;

    [Header("UI")]
    public GameObject pauseParentObj;
    public GameObject scoreParentObj;
    public GameObject bombButtonObj;
    public UIPanel StageUpLabel;
    public UILabel scoreLabel;
    private float saveTimeScale = 1;
    public bool isNotBombNotItem;
    private int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreLabel.text = Score.ToString();
            Spawner.instance.CheckStage(score);
        }
    }

    
    [Header("Spawner")]
    public GameObject spawner;

    [Header("enemyParent")]
    public GameObject enemyParent;
    public Transform enemyParentTr;
    public Transform laserParentTr_1;
    public Transform laserParentTr_2;

    [Header("ObjectPoolEnemy")]
    public GameObject blackProjectilePrefab;
    public GameObject redProjectilePrefab;
    public GameObject yellowProjectilePrefab;
    public GameObject greenProjectilePrefab;
    public GameObject LaserPrefab;

    [Header("Effect")]
    public GameObject destroyParticlePrefab;
    public GameObject bombParticlePrefab;

    private void Awake()
    {
        instance = this;
        isNotBombNotItem = false;
        UseItem();
        InitObjectPool();
        ReSet();
    }

    private void Start()
    {
        Managers.Sound.SetBGM(1f);
        LoadingManager.instance.CartainOnOff(new Vector2(400, 400), false, () => {
            BackkeyManager.isBlock = false;
        });
    }

    public void UseItem() {
        int itemCount = 0;
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        if (saveData.bombreducedcooldowntime == "TRUE") {
            maxManaPoint = 240;
            itemCount++;
            isNotBombNotItem = true;
            saveData.coin -= 6000;
        }
        if (saveData.barrier == "TRUE")
        {
            barrierObj.SetActive(true);
            itemCount++;
            isNotBombNotItem = true;
            saveData.coin -= 4500;
        }
        if (saveData.hyperrun == "TRUE")
        {
            score += 50000;
            itemCount++;
            isNotBombNotItem = true;
            saveData.coin -= 8500;
        }
        saveData.item_count += itemCount;
        var challengeDataList = Managers.Data.GetDataList<Table.ChallengeTable>().FindAll(x => x.text_id == 6);
        Table.PlayerChallengeTable playerData;

        for (int i = 0; i < challengeDataList.Count; i++) {
            playerData = Managers.Data.GetData<Table.PlayerChallengeTable>(challengeDataList[i].id);
            if (playerData.clear_challenge == "FALSE")
            {
                if (challengeDataList[i].value <= saveData.item_count) {
                    playerData.clear_challenge = "TRUE";
                    Managers.Data.UpdateData(playerData);
                }
                break;
            }
        }
        Managers.Data.UpdateData(saveData);
    }

    public void ItemDataReset() {
        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        saveData.bombreducedcooldowntime = "FALSE";
        saveData.barrier = "FALSE";
        saveData.hyperrun = "FALSE";
        Managers.Data.UpdateData(saveData);
    }

    public void ReSet()
    {
        spawner.SetActive(true);
        pauseParentObj.SetActive(true);
        scoreParentObj.SetActive(true);
        bombButtonObj.SetActive(true);
        ControllOnOff(true);
        StartCoroutine("CoPlusScore");
        StartCoroutine("CoRecoveryMana");
    }

    public void Retry() {
        ItemDataReset();
        Time.timeScale = 1;
        BackkeyManager.isBlock = true;
        LoadingManager.instance.CartainOnOff(new Vector2(1, 1), true, () => {
            SceneManager.LoadScene("Game");
        });
    }

    public void GoTitle() {
        ItemDataReset();
        Time.timeScale = 1;
        BackkeyManager.isBlock = true;
        LoadingManager.instance.CartainOnOff(new Vector2(1, 1), true, () => {
            SceneManager.LoadScene("Title");
        });
    }

    #region UI

    public void Stop() {
        StopCoroutine("CoPlusScore");
        StopCoroutine("CoRecoveryMana");
        spawner.SetActive(false);
        pauseParentObj.SetActive(false);
        scoreParentObj.SetActive(false);
        bombButtonObj.SetActive(false);
        ControllOnOff(false);
    }

    public void GameContinue() {
        if (isShowAd)
        {
            GameOver();
        }
        else {
            PopupContainer.CreatePopup(PopupType.GameContinuePopup).Init();
            isShowAd = true;
        }
    }

    public void GameOver() {
        PopupContainer.CreatePopup(PopupType.GameOverPopup).Init();
    }

    public void SetPause() {
        if (Time.timeScale != 0)
        {
            Managers.Sound.PlaySFX(SfxType.Button);
            PopupContainer.CreatePopup(PopupType.PausePopup).Init();
            ControllOnOff(false);
            saveTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            ControllOnOff(true);
            Time.timeScale = saveTimeScale;
        }
    }

    void ControllOnOff(bool isOn) {
        controllManager.SetActive(isOn);
        controllParent.SetActive(isOn);
    }

    public void BombEnemy() {
        if (curManaPoint == maxManaPoint) {
            ManaReset();
            Bomb();
        }
    }

    public void ManaReset() {
        curManaPoint = 0;
    }

    public void Bomb() {
        isNotBombNotItem = true;
        enemyParent.SetActive(false);
        enemyParent.SetActive(true);
    }

    IEnumerator CoRecoveryMana()
    {
        float checkTime = 0;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            checkTime += Time.fixedDeltaTime;
            if (checkTime >= 0.1f)
            {
                checkTime -= 0.1f;
                if (curManaPoint < maxManaPoint)
                {
                    bombParticlePrefab.SetActive(false);
                    curManaPoint++;
                    SetManaBarUI();
                }
                else {
                    bombParticlePrefab.SetActive(true);
                }
            }
        }
    }

    void SetManaBarUI()
    {
        manaBarSlider.value = (float)curManaPoint / maxManaPoint;
    }

    IEnumerator CoPlusScore() {
        while (true) {
            yield return new WaitForFixedUpdate();
            Score += 9;
        }
    }

    #endregion

    public void InitObjectPool() {
        ObjectPoolContainer.Instance.CreateObjectPool("Black", blackProjectilePrefab, 200, enemyParentTr);
        ObjectPoolContainer.Instance.CreateObjectPool("Red", redProjectilePrefab, 25, enemyParentTr);
        ObjectPoolContainer.Instance.CreateObjectPool("Yellow", yellowProjectilePrefab, 100, enemyParentTr);
        ObjectPoolContainer.Instance.CreateObjectPool("Green", greenProjectilePrefab, 100, enemyParentTr);
        ObjectPoolContainer.Instance.CreateObjectPool("Laser_1", LaserPrefab, 4, laserParentTr_1);
        ObjectPoolContainer.Instance.CreateObjectPool("Laser_2", LaserPrefab, 4, laserParentTr_2);

        ObjectPoolContainer.Instance.CreateObjectPool("DestroyParticle", destroyParticlePrefab, 300);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
