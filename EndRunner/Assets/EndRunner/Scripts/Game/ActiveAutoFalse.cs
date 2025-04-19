using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAutoFalse : MonoBehaviour
{
    public bool isObjectPool = true;
    public bool isDie = false;
    public float lifeTime = 1;

    private void OnEnable()
    {
        StartCoroutine(CoLifeTime());
    }

    IEnumerator CoLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (isObjectPool)
        {
            ObjectPoolContainer.Instance.Return(gameObject);
        }
        if (isDie) {
            GameManager.instance.GameContinue();
        }
    }
}
