using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private readonly HashSet<int> ignorePauseSfx = new HashSet<int> { 5 }; // Sounds we will ignore to stop playing

    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySfx;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke(nameof(AllowSfx), 1f);
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySfx(int _sfxIndex, Transform _source)
    {
        if (canPlaySfx == false)
            return;

        if (_source && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) >
            sfxMinimumDistance)
            return;


        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSfx(int _index) => sfx[_index].Stop();

    public void StopAllSfx(bool _isPaused)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (ignorePauseSfx.Contains(i))
                continue;

            if (_isPaused)
                sfx[i].Pause();
            else
                sfx[i].UnPause();
        }
    }

    public void StopSfxWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private static IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    private void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    private void StopAllBGM()
    {
        foreach (AudioSource sound in bgm)
        {
            sound.Stop();
        }
    }

    private void AllowSfx() => canPlaySfx = true;
}