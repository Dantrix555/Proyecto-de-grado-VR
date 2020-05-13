using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Audio Asset", menuName ="Audio Asset")]
public class AudioAsset : ScriptableObject
{
    [Header("Game Music")]
    //Music of the game
    public AudioClip inGameMusic;
    public AudioClip menuMusic;
    public AudioClip pauseMusic;
    public AudioClip gameOver;
    public AudioClip gameFinished;

    [Space(5)]
    [Header("Game SFX")]
    //Sound effects
    public AudioClip buttonClic;
    public AudioClip objectAbsorption;
    public AudioClip shoot;
    public AudioClip shotHitScenario;
    public AudioClip enemyCorrectHit;
    public AudioClip enemyFailHit;
    public AudioClip enemyDestroyed;
    public AudioClip gotComponent;
    public AudioClip gotKey;
    public AudioClip canOpenDoor;
    public AudioClip cantOpenDoor;
    public AudioClip canDestroyWall;
    public AudioClip cantDestroyWall;
    public AudioClip canPurifyWater;
    public AudioClip cantPurifyWater;
    public AudioClip spawnComponent;
    public AudioClip teletransportation;
}
