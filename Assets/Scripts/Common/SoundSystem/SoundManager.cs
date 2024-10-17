using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instant;
    [SerializeField] private List<SoundData> soundDatas;
    [SerializeField] private AudioPool audioPool;
    private AudioSource bgMusicSource;
    private bool isPlayBGMusic = false;
    private void Awake()
    {
        if (Instant == null)
        {
            Instant = this;
            audioPool.SetUp();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void PlayAudioOneShot(SoundType type, Transform playLocation)
    {
        var audio = soundDatas.FirstOrDefault((r) => r.SoundType == type);
        if (audio == null)
        {
            return;
        }
        var instant = audioPool.GetObject();
        var source = instant.GetComponent<AudioSource>();
        source.clip = audio.Clip;
        source.outputAudioMixerGroup = audio.Mixer;
        source.loop = false;
        instant.transform.position = playLocation.position;
        source.Play();
        await Task.Delay((int)(audio.Clip.length * 1000));
        audioPool.ReturnPool(instant);
    }

    public void PlayBackGroundMusic(SoundType type, Transform playLocation)
    {
        StopPlayBackGroundMusic();
        var audio = soundDatas.FirstOrDefault((r) => r.SoundType == type);
        if (audio == null)
        {
            return;
        }
        var instant = audioPool.GetObject();
        var source = instant.GetComponent<AudioSource>();
        source.clip = audio.Clip;
        source.outputAudioMixerGroup = audio.Mixer;
        source.loop = true;
        instant.transform.position = playLocation.position;
        source.Play();
        bgMusicSource = source;
        isPlayBGMusic = true;
    }

    public void StopPlayBackGroundMusic()
    {
        if (isPlayBGMusic)
        {
            bgMusicSource.Stop();
            audioPool.ReturnPool(bgMusicSource.gameObject);
            isPlayBGMusic = false;
        }
    }
}
