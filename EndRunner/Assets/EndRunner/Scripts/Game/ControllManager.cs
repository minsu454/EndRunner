using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllManager : MonoBehaviour {

    public Transform rotateAllTr;
    private Coroutine coLeft;
    private Coroutine coRight;
    private bool leftOn = false;
    private bool rightOn = false;
    private float speed = 11.5f;
    public enum Direction { Left, Right }

    public void LeftControllerDown() {
        leftOn = true;
        if (coLeft != null)
        {
            StopCoroutine(coLeft);
        }
        if (rightOn) {
            StopCoroutine(coRight);
        }
        coLeft = StartCoroutine("CoRotate", Direction.Left);
    }

    public void LeftControllerUp()
    {
        StopCoroutine(coLeft);
        PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Idle);
    }

    public void RightControllerDown()
    {
        rightOn = true;
        if(coRight != null) {
            StopCoroutine(coRight);
        }
        if (leftOn)
        {
            StopCoroutine(coLeft);
        }
        coRight = StartCoroutine("CoRotate", Direction.Right);
    }

    public void RightControllerUp()
    {
        StopCoroutine(coRight);
        PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Idle);
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        rotateAllTr.Rotate(new Vector3(0, 0, 0.1f * speed));
    //        PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Move);
    //        PlayerManager.instance.SetFlip(false);
    //    }
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        rotateAllTr.Rotate(new Vector3(0, 0, 0.1f * -speed));
    //        PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Move);
    //        PlayerManager.instance.SetFlip(true);
    //    }
    //    else
    //    {
    //        PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Idle);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        GameManager.instance.BombEnemy();
    //    }
    //}

    IEnumerator CoRotate(Direction dir) {
        while (true) {
            switch (dir) {
                case Direction.Left:
                    rotateAllTr.Rotate(new Vector3(0, 0, 0.1f * speed));
                    PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Move);
                    PlayerManager.instance.SetFlip(false);
                    break;
                case Direction.Right:
                    rotateAllTr.Rotate(new Vector3(0, 0, 0.1f * -speed));
                    PlayerManager.instance.SetAnimaton(PlayerManager.AnimType.Move);
                    PlayerManager.instance.SetFlip(true);
                    break;
            }
            yield return null;
        }
    }
}
