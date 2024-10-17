using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundData
{
    public SoundType SoundType;
    public AudioClip Clip;
    public AudioMixerGroup Mixer;
}

public enum SoundType
{
    ButtonClick,
    CharacterAttack,
    CharacterUseSkill,
    EnemyUseSkill,
    BackGroundMusic,
    CardSelect,
    CardPop,
    RollDice,
    ThrowDice,
    MapScorll,
    UpgradeButton
}
