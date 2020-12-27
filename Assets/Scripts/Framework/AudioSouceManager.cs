using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;


class AudioSouceManager
{
    private GameManager gameManager;
    //音效声音
    private AudioSource soundAudioSource;
    //音乐游戏背景音乐
    private AudioSource musicAudioSouce;
    private SimpleMusicPlayer musicPlayer;

    public AudioSouceManager()
    {
        gameManager = GameManager.Instance;
        soundAudioSource = gameManager.GetComponent<AudioSource>();
        musicPlayer = gameManager.GetObj(StringManager.musicPlayer).GetComponent<SimpleMusicPlayer>();
        musicAudioSouce = musicPlayer.GetComponent<AudioSource>();       
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName)
    {
        soundAudioSource.PlayOneShot(gameManager.GetAudioClipRes(soundName));
    }
    /// <summary>
    /// 背景音乐播放
    /// </summary>
    /// <param name="musicName"></param>
    /// <param name="loop"></param>
    public void PlayMusic(string musicName,bool loop)
    {
        musicAudioSouce.clip = gameManager.GetAudioClipRes(musicName);
        musicAudioSouce.loop = loop;
        musicAudioSouce.Play();
    }
    /// <summary>
    /// 停止背景音乐的方法
    /// </summary>
    public void StopMusic()
    {
        musicAudioSouce.Stop();
    }
    /// <summary>
    /// 获取musicPlayer对象
    /// </summary>
    /// <returns></returns>
    public SimpleMusicPlayer GetMusicPlayer()
    {
        return musicPlayer;
    }
    /// <summary>
    /// 获取当前音乐播放到样本点位置
    /// </summary>
    /// <returns></returns>
    public int GetMusicSample()
    {
        return musicAudioSouce.timeSamples;
    }
}

