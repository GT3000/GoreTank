using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> AddFuel;
    public static Action<int> SubtractFuel;
    public static Action<int> SetFuel;
    public static Action<int> ScoreUpdate;
    public static Action<int> ChangeScore;
    public static Action<int> GetBestScore;
    public static Action EnemyKilled;
    public static Action PlayerKilled;
    public static Action PauseMenu;
    public static Action Restart;
    public static Action Quit;
    public static Action<AudioClip> PlaySfx;
    public static Action<float, float> SetOptionsSliders;
    public static Action<float> UpdateMusicVolume;
    public static Action<float> UpdateSfxVolume;
    public static Action SavePlayerPrefs;
    public static Action ClearScores;
    public static Action<float, float, float> CameraShake;
}
