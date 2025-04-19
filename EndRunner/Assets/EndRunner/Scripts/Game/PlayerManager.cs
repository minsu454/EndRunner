using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Animator animator;

    [Header("Paticle")]
    public GameObject restartPaticle;
    public GameObject DiePaticle;

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer hatSprite;
    public SpriteRenderer eyeSprite;
    public SpriteRenderer tieSprite;

    public enum AnimType {
        Idle, Move, Die
    }

    private void Awake()
    {
        instance = this;
        SetAnimaton(PlayerManager.AnimType.Idle);
    }

    public void SetAnimaton(AnimType type) {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName(type.ToString())) {
            animator.Play(type.ToString(), -1, 0);
        }
    }

    public void OnCompleteAnimDie()
    {
        DiePaticle.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetFlip(bool isFlip) {
        spriteRenderer.flipX = isFlip;
        hatSprite.flipX = isFlip;
        eyeSprite.flipX = isFlip;
        tieSprite.flipX = isFlip;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
