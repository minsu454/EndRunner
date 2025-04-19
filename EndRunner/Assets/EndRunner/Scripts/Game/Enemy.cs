using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string name;
    public int enemySpawnNum;
    public Vector2 arrivePos;
    public float arriveMovingTime;
    public bool isRandomCheck;
    public float enemySpawnTerm;
    public float spawnSettingTerm;
    public bool isAfterHiding;
    public bool isBeforeHiding;
    public bool isLaser;

    public Enemy(string name, int enemySpawnNum, Vector2 arrivePos, float arriveMovingTime, bool isRandomCheck, float enemySpawnTerm, float spawnSettingTerm, bool isAfterHiding, bool isBeforeHiding, bool isLaser) {
        this.name = name;
        this.enemySpawnNum = enemySpawnNum;
        this.arrivePos = arrivePos;
        this.arriveMovingTime = arriveMovingTime;
        this.isRandomCheck = isRandomCheck;
        this.enemySpawnTerm = enemySpawnTerm;
        this.spawnSettingTerm = spawnSettingTerm;
        this.isAfterHiding = isAfterHiding;
        this.isBeforeHiding = isBeforeHiding;
        this.isLaser = isLaser;
    }
}
