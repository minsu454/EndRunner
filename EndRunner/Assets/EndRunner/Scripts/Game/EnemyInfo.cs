using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyInfo : MonoBehaviour {

    public static List<GameObject> enemyList = new List<GameObject>();

    private bool isAfterHiding;
    private bool isBeforeHiding;
    private bool isNoEffect;
    private SpriteRenderer spriteRenderer;

    private Sequence enemySeq;

    private void OnEnable()
    {
        enemyList.Add(gameObject);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitEnemyInfo(Enemy enemy) {
        isAfterHiding = enemy.isAfterHiding;
        isBeforeHiding = enemy.isBeforeHiding;
        gameObject.SetActive(true);

        if (isAfterHiding) {
            enemySeq = DOTween.Sequence();

            enemySeq.Append(transform.DOMove(enemy.arrivePos, enemy.arriveMovingTime))
                .InsertCallback(enemy.arriveMovingTime / 2f, () =>
                {
                    spriteRenderer.DOFade(0, 0.75f).OnComplete(() => {
                        gameObject.SetActive(false);
                    });
                });
        }
        else if (isBeforeHiding) {
            spriteRenderer.color = new Color(255f, 255f, 255f, 0f);
            spriteRenderer.DOFade(1, 0.75f).OnComplete(() => {
                transform.DOMove(enemy.arrivePos, enemy.arriveMovingTime);
            });
            
        }
        else {
            transform.DOMove(enemy.arrivePos, enemy.arriveMovingTime);
        }
    }

    public void StopEnemy() {
        for (int i = 0; i < enemyList.Count; i++)
        {
            EnemyInfo info = enemyList[i].GetComponent<EnemyInfo>();
            info.enemySeq.Kill();
            spriteRenderer = enemyList[i].GetComponent<SpriteRenderer>();
            spriteRenderer.DOKill();
            enemyList[i].transform.DOKill();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) {
            if (GameManager.instance.barrierObj.activeSelf) {
                GameManager.instance.barrierObj.SetActive(false);
                GameManager.instance.Bomb();
                return;
            }
            Managers.Sound.SetBGM(0.2f);
            BackkeyManager.isBlock = true;
            PlayerManager.instance.hatSprite.gameObject.SetActive(false);
            PlayerManager.instance.eyeSprite.gameObject.SetActive(false);
            PlayerManager.instance.tieSprite.gameObject.SetActive(false);
            PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Die);
            Managers.Sound.PlaySFX(SfxType.Die);
            StopEnemy();
            GameManager.instance.Stop();
        }
        else if (col.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }

    public void ActiveFalseNoEffect() {
        isNoEffect = true;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        enemyList.Remove(gameObject);
        gameObject.SetActive(false);

        if (!isAfterHiding && isNoEffect == false)
        {
            GameObject effectObj = ObjectPoolContainer.Instance.Pop("DestroyParticle");
            if (effectObj != null)
            {
                effectObj.transform.position = transform.position;
                effectObj.SetActive(true);
            }
        }
        isNoEffect = false;
        spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
        ObjectPoolContainer.Instance.Return(gameObject);
    }

}
