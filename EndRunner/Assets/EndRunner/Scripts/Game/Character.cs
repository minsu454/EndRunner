using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public static Character instance;

    private SpriteRenderer hatSprite;
    private SpriteRenderer eyeSprite;
    private SpriteRenderer tieSprite;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterType characterType;
    private HatType hatType;
    private EyeType eyeType;
    private TieType tieType;

    private CharacterAccessories characterHat;
    private CharacterAccessories characterEye;
    private CharacterAccessories characterTie;

    private void Awake()
    {
        instance = this;
        OnCharacterSet();
    }

    public void OnCharacterSet() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hatSprite = PlayerManager.instance.hatSprite;
        eyeSprite = PlayerManager.instance.eyeSprite;
        tieSprite = PlayerManager.instance.tieSprite;

        var saveData = Managers.Data.GetData<Table.SaveTable>(0);
        CharacterType characterType = (CharacterType)Enum.Parse(typeof(CharacterType), saveData.pick_character);
        HatType hatType = (HatType)Enum.Parse(typeof(HatType), saveData.pick_hat);
        EyeType eyeType = (EyeType)Enum.Parse(typeof(EyeType), saveData.pick_eye);
        TieType tieType = (TieType)Enum.Parse(typeof(TieType), saveData.pick_tie);
        this.characterType = characterType;
        this.hatType = hatType;
        this.eyeType = eyeType;
        this.tieType = tieType;
        SetCharacter();
        SetHat();
        SetEye();
        SetTie();
    }

    public void SetCharacter()
    {
        var characterColor = CharacterContainer.instance.GetCharacter(characterType);
        spriteRenderer.sprite = characterColor.sprite;
        animator.runtimeAnimatorController = characterColor.runtimeAnimatorController;
    }

    public void SetHat() {
        var characterHat = CharacterContainer.instance.GetAccessory(AccessoryType.HatType, hatType);
        this.characterHat = characterHat;
    }

    public void SetEye() {
        var characterEye = CharacterContainer.instance.GetAccessory(AccessoryType.EyeType, eyeType);
        this.characterEye = characterEye;
    }

    public void SetTie() {
        var characterTie = CharacterContainer.instance.GetAccessory(AccessoryType.TieType, tieType);
        this.characterTie = characterTie;
    }

    public void DownMove() {
        hatSprite.sprite = characterHat.downMoveSprite;
        eyeSprite.sprite = characterEye.downMoveSprite;
        tieSprite.sprite = characterTie.downMoveSprite;
    }

    public void UpMove() {
        hatSprite.sprite = characterHat.upMoveSprite;
        eyeSprite.sprite = characterEye.upMoveSprite;
        tieSprite.sprite = characterTie.upMoveSprite;
    }

    public void idle() {
        hatSprite.sprite = characterHat.idleSprite;
        eyeSprite.sprite = characterEye.idleSprite;
        tieSprite.sprite = characterTie.idleSprite;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
