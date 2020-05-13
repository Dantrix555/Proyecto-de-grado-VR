using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : CHEMSingleton<SoundManager>
{
    protected SoundManager() { }

    [SerializeField] private AudioAsset _audioAsset = default;
    [SerializeField] private AudioSource _gameSfx = default;
    [SerializeField] private AudioSource _gameMusic = default;

    public enum Music
    {
        InGame,
        Menu,
        Pause,
        GameOver,
        Finished
    }

    public enum SFX
    {
        ButtonClic,
        ObjectAbsorption,
        Shoot,
        ShotHitScenario,
        EnemyCorrectHit,
        EnemyFailHit,
        EnemyDestroyed,
        GotComponent,
        GotKey,
        CanOpenDoor,
        CantOpenDoor,
        CanDestroyWall,
        CantDestroyWall,
        CanPurifyWater,
        CantPurifyWater,
        SpawnComponent,
        Teletransportation
    }

    public static void LoadSoundEffect(SFX sfx)
    {
        AudioClip newSfxClip = null;

        switch(sfx)
        {
            case SFX.ButtonClic:
                newSfxClip = Instance._audioAsset.buttonClic;
                break;
            case SFX.ObjectAbsorption:
                newSfxClip = Instance._audioAsset.objectAbsorption;
                break;
            case SFX.Shoot:
                newSfxClip = Instance._audioAsset.shoot;
                break;
            case SFX.ShotHitScenario:
                newSfxClip = Instance._audioAsset.shotHitScenario;
                break;
            case SFX.EnemyCorrectHit:
                newSfxClip = Instance._audioAsset.enemyCorrectHit;
                break;
            case SFX.EnemyFailHit:
                newSfxClip = Instance._audioAsset.enemyFailHit;
                break;
            case SFX.EnemyDestroyed:
                newSfxClip = Instance._audioAsset.enemyDestroyed;
                break;
            case SFX.GotComponent:
                newSfxClip = Instance._audioAsset.gotComponent;
                break;
            case SFX.GotKey:
                newSfxClip = Instance._audioAsset.gotKey;
                break;
            case SFX.CanOpenDoor:
                newSfxClip = Instance._audioAsset.canOpenDoor;
                break;
            case SFX.CantOpenDoor:
                newSfxClip = Instance._audioAsset.cantOpenDoor;
                break;
            case SFX.CanDestroyWall:
                newSfxClip = Instance._audioAsset.canDestroyWall;
                break;
            case SFX.CantDestroyWall:
                newSfxClip = Instance._audioAsset.cantDestroyWall;
                break;
            case SFX.CanPurifyWater:
                newSfxClip = Instance._audioAsset.canPurifyWater;
                break;
            case SFX.CantPurifyWater:
                newSfxClip = Instance._audioAsset.cantPurifyWater;
                break;
            case SFX.SpawnComponent:
                newSfxClip = Instance._audioAsset.spawnComponent;
                break;
            case SFX.Teletransportation:
                newSfxClip = Instance._audioAsset.teletransportation;
                break;
        }

        Instance._gameSfx.clip = newSfxClip;
        Instance._gameSfx.Play();
    }

    //Set the music of the stage
    public static void SetBackgroundMusic(Music music)
    {
        AudioClip newMusicClip = null;

        switch(music)
        {
            case Music.Menu:
                newMusicClip = Instance._audioAsset.menuMusic;
                break;
            case Music.InGame:
                newMusicClip = Instance._audioAsset.inGameMusic;
                break;
            case Music.Pause:
                newMusicClip = Instance._audioAsset.pauseMusic;
                break;
            case Music.GameOver:
                newMusicClip = Instance._audioAsset.gameOver;
                break;
            case Music.Finished:
                newMusicClip = Instance._audioAsset.gameFinished;
                break;
        }

        Instance._gameMusic.clip = newMusicClip;
        Instance._gameMusic.Play();
    }
}
