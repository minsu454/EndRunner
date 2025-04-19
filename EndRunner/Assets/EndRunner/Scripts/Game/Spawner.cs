using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public Transform centerTr;

    private List<Table.EnemySpawnTable> spawnDataList;
    private int spawnIndex;
    private int conditionScore = 0;
    private int goalScore = 0;
    private int listnum = 0;
    private int nextStageScore = 0;

    private Coroutine[] coroutineArray = new Coroutine[5];

    private Dictionary<string, bool> SpawnDic = new Dictionary<string, bool>();
    private List<Enemy> enemyList = new List<Enemy>();
    private List<bool> stageList = new List<bool>();
    
    private List<int> RandomCheck = new List<int>();
    private enum enemyType {
        Black,
        Yellow,
        Green,
        Red,
        Laser
    }

    private void OnEnable()
    {
        if (SpawnDic[enemyType.Black.ToString()])
        {
            StartSpawnCoroutine(enemyType.Black);
        }
        if (SpawnDic[enemyType.Yellow.ToString()])
        {
            StartSpawnCoroutine(enemyType.Yellow);
        }
        if (SpawnDic[enemyType.Green.ToString()])
        {
            StartSpawnCoroutine(enemyType.Green);
        }
        if (SpawnDic[enemyType.Red.ToString()])
        {
            StartSpawnCoroutine(enemyType.Red);
        }
        if (SpawnDic[enemyType.Laser.ToString()])
        {
            StartSpawnCoroutine(enemyType.Laser);
        }
    }

    private void Awake()
    {
        instance = this;
        spawnDataList = Managers.Data.GetDataList<Table.EnemySpawnTable>();
        nextStageScore = 25000;
        AddEnemy();
        AddSpawn();
        AddStage();
    }

    private void AddEnemy() {
        enemyList.Add(new Enemy("Black", 0, Vector2.zero, 3.7f, true, 0, 0.1f, false, false, false));
        enemyList.Add(new Enemy("Yellow", 0, Vector2.zero, 2.7f, false, 0, 0.2f, false, false, false));
        enemyList.Add(new Enemy("Green", 0, Vector2.zero, 3f, false, 0f, 0.05f, true, false, false));
        enemyList.Add(new Enemy("Red", 20, Vector2.zero, 3f, false, 0.3f, 4f, false, true, false));
        enemyList.Add(new Enemy("Laser", 0, Vector2.zero, 0f, false, 0f, 3f, false, false, true));
        if (PlayerPrefs.GetString("Dif") == "easy") {
            for (int i = 0; i < 5; i++) {
                enemyList[i].spawnSettingTerm *= 2f;
            }
            enemyList[3].enemySpawnNum = 10;
        }

    }

    private void AddSpawn() {
        SpawnDic.Add("Black", false);
        SpawnDic.Add("Yellow", false);
        SpawnDic.Add("Green", false);
        SpawnDic.Add("Red", false);
        SpawnDic.Add("Laser", false);
    }

    private void AddStage() {
        for (int i = 0; i < spawnDataList.Count; i++) {
            stageList.Add(false);
        }
    }

    public void isEasy() {
        if (PlayerPrefs.GetString("Dif") == "easy")
        {
            enemyList[(int)enemyType.Black].spawnSettingTerm *= 2f;
        }
    }

    public void CheckStage (int score)
    {
        if (listnum == stageList.Count) {
            return;
        }

        if (score >= goalScore && !stageList[listnum]) {
            switch (listnum) {
                case 0:
                    break;
                case 1:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.095f;
                    isEasy();
                    break;
                case 2:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.085f;
                    isEasy();
                    nextStageScore += 25000;
                    break;
                case 3:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.0875f;
                    isEasy();
                    break;
                case 4:
                    break;
                case 5:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.0925f;
                    isEasy();
                    nextStageScore += 25000;
                    break;
                case 6: 
                case 7:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.105f;
                    isEasy();
                    break;
                case 8:
                    nextStageScore += 50000;
                    break;
                case 9:
                    enemyList[(int)enemyType.Black].spawnSettingTerm = 0.115f;
                    isEasy();
                    break;
            }
            if (listnum != 0) {
                DOTween.To(() => 0f, x => GameManager.instance.StageUpLabel.alpha = x, 1f, 0.5f).OnComplete(() =>
                {
                    DOTween.To(() => 1f, x => GameManager.instance.StageUpLabel.alpha = x, 0f, 0.5f);
                });
                if (PlayerPrefs.GetString("Dif") == "hard") {
                    var data = Managers.Data.GetData<Table.PlayerChallengeTable>(listnum - 1);
                    if (data.clear_challenge == "FALSE") {
                        data.clear_challenge = "TRUE";
                    }
                    Managers.Data.UpdateData(data);
                }
            }
            Stage(listnum);
            goalScore += nextStageScore;
            listnum++;
        }

    }

    void Stage(int stageNum) {
        if (spawnDataList[stageNum].Black == "TRUE" && !SpawnDic[enemyType.Black.ToString()])
        {
            StartSpawnCoroutine(enemyType.Black);
        }
        else if (spawnDataList[stageNum].Black == "FALSE" && SpawnDic[enemyType.Black.ToString()])
        {
            StopSpawnCoroutine(enemyType.Black);
        }

        if (spawnDataList[stageNum].Yellow == "TRUE" && !SpawnDic[enemyType.Yellow.ToString()])
        {
            StartSpawnCoroutine(enemyType.Yellow);
        }
        else if (spawnDataList[stageNum].Yellow == "FALSE" && SpawnDic[enemyType.Yellow.ToString()])
        {
            StopSpawnCoroutine(enemyType.Yellow);
        }

        if (spawnDataList[stageNum].Green == "TRUE" && !SpawnDic[enemyType.Green.ToString()])
        {
            StartSpawnCoroutine(enemyType.Green);
        }
        else if (spawnDataList[stageNum].Green == "FALSE" && SpawnDic[enemyType.Green.ToString()])
        {
            StopSpawnCoroutine(enemyType.Green);
        }

        if (spawnDataList[stageNum].Red == "TRUE" && !SpawnDic[enemyType.Red.ToString()])
        {
            StartSpawnCoroutine(enemyType.Red);
        }
        else if (spawnDataList[stageNum].Red == "FALSE" && SpawnDic[enemyType.Red.ToString()])
        {
            StopSpawnCoroutine(enemyType.Red);
        }

        if (spawnDataList[stageNum].Laser == "TRUE" && !SpawnDic[enemyType.Laser.ToString()])
        {
            StartSpawnCoroutine(enemyType.Laser);
        }
        else if (spawnDataList[stageNum].Laser == "FALSE" && SpawnDic[enemyType.Laser.ToString()])
        {
            StopSpawnCoroutine(enemyType.Laser);
        }

        stageList[stageNum] = true;
    }

    void StartSpawnCoroutine(enemyType type) {
        coroutineArray[(int)type] = StartCoroutine(nameof(CoSpawn), enemyList[(int)type]);
        SpawnDic[type.ToString()] = true;
    }

    void StopSpawnCoroutine(enemyType type)
    {
        StopCoroutine(coroutineArray[(int)type]);
        SpawnDic[type.ToString()] = false;
    }

    IEnumerator CoSpawn(Enemy enemy)
    {
        float timeCheck = 0f;
        while (true) {
            timeCheck += Time.deltaTime;
            if (timeCheck >= enemy.spawnSettingTerm && enemy.enemySpawnNum == 0) {
                timeCheck -= enemy.spawnSettingTerm;
                Spawn(enemy);
            }
            else if(enemy.enemySpawnNum != 0)
            {
                for (int i = 0; i < enemy.enemySpawnNum;)
                {
                    timeCheck += Time.deltaTime;
                    if (timeCheck >= enemy.enemySpawnTerm)
                    {
                        timeCheck -= enemy.enemySpawnTerm;
                        Spawn(enemy);
                        i++;
                    }
                    yield return null;
                }
                while (timeCheck >= enemy.spawnSettingTerm) {
                    timeCheck += Time.deltaTime;
                    yield return null;
                }
                timeCheck -= enemy.spawnSettingTerm;
            }
            yield return null;
        }
    }

    void Spawn(Enemy enemy) {
        Vector2 pos;
        if (!enemy.isLaser)
        {
            if (enemy.isBeforeHiding)
            {
                pos = GetLimitPos(centerTr.position, 8f, enemy.isRandomCheck);
            }
            else
            {
                pos = GetLimitPos(centerTr.position, 15, enemy.isRandomCheck);
            }
            GameObject enemyObj = ObjectPoolContainer.Instance.Pop(enemy.name);
            enemyObj.transform.position = pos;
            EnemyInfo info = enemyObj.GetComponent<EnemyInfo>();
            info.InitEnemyInfo(enemy);
        }
        else
        {
            float angle_1 = GetAngle();
            float angle_2 = GetAngle();
            GameObject enemyObj_1 = ObjectPoolContainer.Instance.Pop("Laser_1");
            enemyObj_1.transform.parent.eulerAngles = new Vector3(0f, 0f, angle_1);
            enemyObj_1.SetActive(true);
            if (PlayerPrefs.GetString("Dif") == "hard")
            {
                GameObject enemyObj_2 = ObjectPoolContainer.Instance.Pop("Laser_2");
                enemyObj_2.transform.parent.eulerAngles = new Vector3(0f, 0f, angle_2);
                enemyObj_2.SetActive(true);
            }
        }
    }

    private float GetAngle() {
        float angle = Random.value * 360;
        return angle;
    }

    private Vector2 GetLimitPos(Vector2 startPos, float distance, bool isRandomCheck, float getAngle = 0) {
        Vector2 returnVec = Vector2.zero;
        float angle;
        if (getAngle == 0)
        {
            angle = GetAngle();

            if (isRandomCheck)
            {
                int num = Mathf.RoundToInt(angle);
                if (RandomCheck.Count >= 30)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        RandomCheck.RemoveAt(0);
                    }
                }
                while (RandomCheck.Contains(num))
                {
                    angle = Random.value * 360;
                    num = Mathf.RoundToInt(angle);
                }
                for (int i = num - 1; i < num + 1; i++)
                {
                    RandomCheck.Add(i);
                }
            }
        }
        else {
            angle = getAngle;
        }
        
        returnVec.x = distance * Mathf.Cos(angle) + startPos.x;
        returnVec.y = distance * Mathf.Sin(angle) + startPos.y;

        return returnVec;
    }

    private void OnDestroy()
    {
        instance = null;
    }

}
