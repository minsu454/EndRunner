using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserInfo : MonoBehaviour
{

    private SpriteRenderer laserSprite;
    private BoxCollider2D laserCol;

    private void OnEnable()
    {
        laserCol.enabled = false;
        transform.DOScaleX(0f, 1.5f).OnComplete(() =>
        {
            InitLaser();
        });
    }

    private void Awake()
    {
        laserSprite = GetComponent<SpriteRenderer>();
        laserCol = GetComponent<BoxCollider2D>();
    }

    public void InitLaser() {
        transform.localScale = new Vector2(transform.localScale.x, 7f);
        laserSprite.color = Color.white;
        laserSprite.sprite = SpriteContainer.Instance.GetSprite(SpriteContainer.Category.Enemy, "Laser");

        laserCol.enabled = true;
        Managers.Sound.PlaySFX(SfxType.Laser);
        laserCol.size = new Vector2(0.47f, 1.5f);
        transform.DOScaleX(3f, 0.1f).OnUpdate(() => {
        }).OnComplete(() => {
            transform.DOScaleX(0f, 1f).OnComplete(() => {
                gameObject.SetActive(false);
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) {
            col.GetComponent<EnemyInfo>().ActiveFalseNoEffect();
        }
        else if (col.CompareTag("Player"))
        {
            GameManager.instance.barrierObj.SetActive(false);
            BackkeyManager.isBlock = true;
            PlayerManager.instance.hatSprite.gameObject.SetActive(false);
            PlayerManager.instance.eyeSprite.gameObject.SetActive(false);
            PlayerManager.instance.tieSprite.gameObject.SetActive(false);
            PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Die);
            Managers.Sound.PlaySFX(SfxType.Die);
            EnemyInfo.enemyList[0].GetComponent<EnemyInfo>().StopEnemy();
            GameManager.instance.Stop();
        }
    }

    public void OnDisable()
    {
        Color redColor = Color.red;
        redColor.a = 0.75f;
        laserSprite.color = redColor;
        transform.localScale = new Vector2(0.2f, 8.5f);
        laserSprite.sprite = SpriteContainer.Instance.GetSprite(SpriteContainer.Category.Enemy, "White");
        ObjectPoolContainer.Instance.Return(gameObject);
    }
}
