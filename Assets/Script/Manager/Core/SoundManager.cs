using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// ���� �Ŵ��� v1.05.0 made by JJSmith (Curookie)
/// Fix��, ��巹����� �ҷ������ �ϰ� �̱����� ���� Manager ���� ���� �ؾ���.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    [SerializeField]
    [TextArea(5, 10)]
    string _���� =
@"1) Playlist�� ����� �����/ȿ�������� �ִ´�. (Resources, URL ����)
2) � ��ũ��Ʈ������ ����Ϸ��� AudioManager.Inst.PlayBGM(""Ŭ���̸�""); AudioManager.Inst.PlaySFX(""Ŭ���̸�""); (�ݺ����, OneShot �� ����)
3) ������� ���̵� ����(Swift), ���̵� �ƿ�/��(LinearFade), ũ�ν� ���̵�(CrossFade) 3���� ���̵� ������ �ִ�. (playback ������ Swift�� �ؾ���)
4) PlayerPrefabs���� ������ �����ϸ� ��� �Ӽ� �ִ�. ex)IsMusicOn �����, IsSoundOn ȿ����";


    [Header("����� ����")]

    [Tooltip("����� On/Off")]
    [SerializeField] bool _musicOn = true;

    [Tooltip("����� ����")]
    [Range(0, 1)]
    [SerializeField] float _musicVolume = 1f;

    [Tooltip("���� �� ����� ��뿩��")]
    [SerializeField] bool _useMusicVolOnStart = false;

    [Tooltip("Target Group ����� ��ȣ�� ���� ����, ��� �� �Ұ�� ��������� ��.")]
    [SerializeField] AudioMixerGroup _musicMixerGroup = null;

    [Tooltip("����� �����ͼ� ��")]
    [SerializeField] string _volumeOfMusicMixer = string.Empty;

    [Space(3)]

    [Header("ȿ���� ����")]

    [Tooltip("ȿ���� On/Off")]
    [SerializeField] bool _soundFxOn = true;

    [Tooltip("ȿ���� ����")]
    [Range(0, 1)]
    [SerializeField] float _soundFxVolume = 1f;

    [Tooltip("���� �� ȿ���� ��뿩��")]
    [SerializeField] bool _useSfxVolOnStart = false;

    [Tooltip("Target Group ȿ���� ��ȣ�� ���� ����, ��� �� �Ұ�� ��������� ��.")]
    [SerializeField] AudioMixerGroup _soundFxMixerGroup = null;

    [Tooltip("ȿ���� �����ͼ� ��")]
    [SerializeField] string _volumeOfSFXMixer = string.Empty;

    [Space(3)]

    [Tooltip("��� ����� Ŭ���� ���⿡ ������ ��.")]
    [SerializeField] List<AudioClip> _playlist = new List<AudioClip>();

    // ȿ���� Ǯ���� ���� ����Ʈ
    List<SoundEffect> sfxPool = new List<SoundEffect>();
    // ����� �Ŵ��� �����
    static BackgroundMusic backgroundMusic;
    // ���� ������ҽ��� ���̵带 ���� ���� ������ҽ�
    static AudioSource musicSource = null, crossfadeSource = null;
    // ���� ������� ���� ��ġ�� ����
    static float currentMusicVol = 0, currentSfxVol = 0, musicVolCap = 0, savedPitch = 1f;
    // On/Off ����
    static bool musicOn = false, sfxOn = false;
    // ��ȯ�ð� ����
    static float transitionTime;

    // PlayerPrefabs ������ ���� Ű
    static readonly string BgMusicVolKey = "BGMVol";
    static readonly string SoundFxVolKey = "SFXVol";
    static readonly string BgMusicMuteKey = "BGMMute";
    static readonly string SoundFxMuteKey = "SFXMute";

    // �� �������� ���ο�
    private static bool alive = true;

    /// <summary>
    /// �Ӽ� �̱��� �������� ����
    /// </summary>

    void OnDestroy()
    {
        StopAllCoroutines();
        SaveAllPreferences();
    }

    void OnApplicationExit()
    {
        alive = false;
    }

    private void Awake()
    {
        Initialise().Forget();
    }


    /// <summary>
    /// ������Ŵ��� �ʱ�ȭ �Լ�
    /// </summary>

    private const string SoundDataScriptableName = "SoundData";
    async UniTaskVoid Initialise()
    {
        //gameObject.name = "AudioManager";

        bool isRegister = false;
        Managers.Resource.Load<ScriptableObject>(SoundDataScriptableName, (success) =>
        {
            SoundData data = (SoundData)success;
            _playlist = data.Playlist;
            isRegister = true;
        });
        await UniTask.WaitUntil(() => { return isRegister == true; });

        // PlayerPrefs���� �� ��������
        _musicOn = LoadBGMMuteStatus();
        _musicVolume = _useMusicVolOnStart ? _musicVolume : LoadBGMVolume();
        _soundFxOn = LoadSFXMuteStatus();
        _soundFxVolume = _useSfxVolOnStart ? _soundFxVolume : LoadSFXVolume();

        // ���� ������ҽ� ������Ʈ ����
        musicSource = gameObject.GetOrAddComponent<AudioSource>();
        musicSource = ConfigureAudioSource(musicSource);

        StartCoroutine(OnUpdate());

    }



    /// <summary>
    /// ���� ������ ����ؼ� 2D�� ������ҽ� �����ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource with 2D features</returns>
    AudioSource ConfigureAudioSource(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = _musicMixerGroup;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;   //2D
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.loop = true;
        // PlayerPrefs���� �� ��������
        audioSource.volume = LoadBGMVolume();
        audioSource.mute = !_musicOn;


        return audioSource;
    }

    /// <summary>
    /// ȿ���� Ǯ�� �ִ� ȿ������ �����ϴ� �Լ�  
    /// OnUpdate�Լ����� �ҷ��´�.
    /// </summary>
    private void ManageSoundEffects()
    {
        for (int i = sfxPool.Count - 1; i >= 0; i--)
        {
            SoundEffect sfx = sfxPool[i];
            // ��� ��
            if (sfx.Source.isPlaying && !float.IsPositiveInfinity(sfx.Time))
            {
                sfx.Time -= Time.deltaTime;
                sfxPool[i] = sfx;
            }

            // ������ ��
            if (sfxPool[i].Time <= 0.0001f || HasPossiblyFinished(sfxPool[i]))
            {
                sfxPool[i].Source.Stop();
                // �ݹ��Լ� ����
                if (sfxPool[i].Callback != null)
                {
                    sfxPool[i].Callback.Invoke();
                }

                // Ŭ�� ���� ��
                Destroy(sfxPool[i].gameObject);

                // Ǯ���� �׸񻩱�
                sfxPool.RemoveAt(i);
                break;
            }
        }
    }

    // ������ ������ �� üũ�� �Լ�
    bool HasPossiblyFinished(SoundEffect soundEffect)
    {
        return !soundEffect.Source.isPlaying && FloatEquals(soundEffect.PlaybackPosition, 0) && soundEffect.Time <= 0.09f;
    }

    bool FloatEquals(float num1, float num2, float threshold = .0001f)
    {
        return Math.Abs(num1 - num2) < threshold;
    }

    /// <summary>
    /// ����� ���� ���°� ���ߴ��� üũ�ϴ� �Լ�
    /// </summary>
    private bool IsMusicAltered()
    {
        bool flag = musicOn != _musicOn || musicOn != !musicSource.mute || !FloatEquals(currentMusicVol, _musicVolume);

        // �ͼ� �׷��� ����� ���
        if (_musicMixerGroup != null && !string.IsNullOrEmpty(_volumeOfMusicMixer.Trim()))
        {
            float vol;
            _musicMixerGroup.audioMixer.GetFloat(_volumeOfMusicMixer, out vol);
            vol = NormaliseVolume(vol);

            return flag || !FloatEquals(currentMusicVol, vol);
        }

        return flag;
    }

    /// <summary>
    /// ȿ���� ���� ���°� ���ߴ��� üũ�ϴ� �Լ�
    /// </summary>
    private bool IsSoundFxAltered()
    {
        bool flag = _soundFxOn != sfxOn || !FloatEquals(currentSfxVol, _soundFxVolume);

        // �ͼ� �׷��� ����� ���
        if (_soundFxMixerGroup != null && !string.IsNullOrEmpty(_volumeOfSFXMixer.Trim()))
        {
            float vol;
            _soundFxMixerGroup.audioMixer.GetFloat(_volumeOfSFXMixer, out vol);
            vol = NormaliseVolume(vol);

            return flag || !FloatEquals(currentSfxVol, vol);
        }

        return flag;
    }

    /// <summary>
    /// ũ�ν� ���̵� �� �ƿ� �Լ�
    /// </summary>
    private void CrossFadeBackgroundMusic()
    {
        if (backgroundMusic.MusicTransition == MusicTransition.CrossFade)
        {
            // ��ȯ�� �������� ���
            if (musicSource.clip.name != backgroundMusic.NextClip.name)
            {
                transitionTime -= Time.deltaTime;

                musicSource.volume = Mathf.Lerp(0, musicVolCap, transitionTime / backgroundMusic.TransitionDuration);

                crossfadeSource.volume = Mathf.Clamp01(musicVolCap - musicSource.volume);
                crossfadeSource.mute = musicSource.mute;

                if (musicSource.volume <= 0.00f)
                {
                    SetBGMVolume(musicVolCap);
                    PlayBackgroundMusic(backgroundMusic.NextClip, crossfadeSource.time, crossfadeSource.pitch);
                }
            }
        }
    }

    /// <summary>
    /// ���̵� ��/�ƿ� �Լ�
    /// </summary>
    private void FadeOutFadeInBackgroundMusic()
    {
        if (backgroundMusic.MusicTransition == MusicTransition.LinearFade)
        {
            // ���̵� ��
            if (musicSource.clip.name == backgroundMusic.NextClip.name)
            {
                transitionTime += Time.deltaTime;

                musicSource.volume = Mathf.Lerp(0, musicVolCap, transitionTime / backgroundMusic.TransitionDuration);

                if (musicSource.volume >= musicVolCap)
                {
                    SetBGMVolume(musicVolCap);
                    PlayBackgroundMusic(backgroundMusic.NextClip, musicSource.time, savedPitch);
                }
            }
            // ���̵� �ƿ�
            else
            {
                transitionTime -= Time.deltaTime;

                musicSource.volume = Mathf.Lerp(0, musicVolCap, transitionTime / backgroundMusic.TransitionDuration);

                // ���̵� �ƿ� ������ ���� ���̵� �� ����
                if (musicSource.volume <= 0.00f)
                {
                    musicSource.volume = transitionTime = 0;
                    PlayMusicFromSource(ref musicSource, backgroundMusic.NextClip, 0, musicSource.pitch);
                }
            }
        }
    }

    /// <summary>
    /// ������Ʈ �Լ� �� Enumerator
    /// </summary>
    IEnumerator OnUpdate()
    {
        while (alive)
        {
            ManageSoundEffects();

            // ����� ���� �ٲ���� üũ
            if (IsMusicAltered())
            {
                ToggleBGMMute(!musicOn);

                if (!FloatEquals(currentMusicVol, _musicVolume))
                {
                    currentMusicVol = _musicVolume;
                }

                if (_musicMixerGroup != null && !string.IsNullOrEmpty(_volumeOfMusicMixer))
                {
                    float vol;
                    _musicMixerGroup.audioMixer.GetFloat(_volumeOfMusicMixer, out vol);
                    vol = NormaliseVolume(vol);
                    currentMusicVol = vol;
                }

                SetBGMVolume(currentMusicVol);
            }

            // ȿ���� ���� �ٲ���� üũ
            if (IsSoundFxAltered())
            {
                ToggleSFXMute(!sfxOn);

                if (!FloatEquals(currentSfxVol, _soundFxVolume))
                {
                    currentSfxVol = _soundFxVolume;
                }

                if (_soundFxMixerGroup != null && !string.IsNullOrEmpty(_volumeOfSFXMixer))
                {
                    float vol;
                    _soundFxMixerGroup.audioMixer.GetFloat(_volumeOfSFXMixer, out vol);
                    vol = NormaliseVolume(vol);
                    currentSfxVol = vol;
                }

                SetSFXVolume(currentSfxVol);
            }

            // ũ�ν� ���̵��� ���
            if (crossfadeSource != null)
            {
                CrossFadeBackgroundMusic();

                yield return null;
            }
            else
            {
                // ���̵� ��/ �ƿ��� ���
                if (backgroundMusic.NextClip != null)
                {
                    FadeOutFadeInBackgroundMusic();

                    yield return null;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    #region ��Ʈ�� ó��

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ��� </param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="playback_position">���۽���</param>
    public void PlayBGM(string clip, MusicTransition transition, float transition_duration, float volume, float pitch, float playback_position = 0)
    {
        PlayBGM(GetClipFromPlaylist(clip), transition, transition_duration, volume, pitch, playback_position);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    public void PlayBGM(string clip, MusicTransition transition, float transition_duration, float volume)
    {
        PlayBGM(GetClipFromPlaylist(clip), transition, transition_duration, volume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    public void PlayBGM(string clip, MusicTransition transition, float transition_duration)
    {
        PlayBGM(GetClipFromPlaylist(clip), transition, transition_duration, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    public void PlayBGM(string clip, MusicTransition transition)
    {
        PlayBGM(GetClipFromPlaylist(clip), transition, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� �ٷ� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    public void PlayBGM(string clip)
    {
        PlayBGM(GetClipFromPlaylist(clip), MusicTransition.Swift, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="duration">����ð�</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(string clip, Vector2 location, float duration, float volume, bool singleton = false, float pitch = 1f, Action callback = null)
    {
        try
        {
            return PlaySFX(GetClipFromPlaylist(clip), location, duration, volume, singleton, pitch, callback);
        }
        catch
        {

            return null;
        }

    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="duration">����ð�</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(string clip, Vector2 location, float duration, bool singleton = false, Action callback = null)
    {
        return PlaySFX(GetClipFromPlaylist(clip), location, duration, _soundFxVolume, singleton, 1.0f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="duration">����ð�</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(string clip, float duration, bool singleton = false, Action callback = null)
    {
        return PlaySFX(GetClipFromPlaylist(clip), Vector2.zero, duration, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(string clip, bool singleton = false, Action callback = null)
    {
        var aClip = GetClipFromPlaylist(clip);
        return PlaySFX(aClip, Vector2.zero, aClip.length, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(string clip, Vector2 location, int repeat, float volume, bool singleton = false, float pitch = 1f, Action callback = null)
    {
        return RepeatSFX(GetClipFromPlaylist(clip), location, repeat, volume, singleton, pitch, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(string clip, Vector2 location, int repeat, bool singleton = false, Action callback = null)
    {
        return RepeatSFX(GetClipFromPlaylist(clip), location, repeat, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(string clip, int repeat, bool singleton = false, Action callback = null)
    {
        return RepeatSFX(GetClipFromPlaylist(clip), Vector2.zero, repeat, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(string clip, Vector2 location, float volume, float pitch = 1f, Action callback = null)
    {
        return PlayOneShot(GetClipFromPlaylist(clip), location, volume, pitch, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(string clip, Vector2 location, Action callback = null)
    {
        return PlayOneShot(GetClipFromPlaylist(clip), location, _soundFxVolume, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(string clip, Action callback = null)
    {
        return PlayOneShot(GetClipFromPlaylist(clip), Vector2.zero, _soundFxVolume, 1f, callback);
    }

    /// <summary>
    /// Ư����ġ ���� �����̽�(2D)���� ȿ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayClipAtPoint(string clip, Vector3 location, Action callback = null)
    {
        return PlayClipAtPoint(GetClipFromPlaylist(clip), location, _soundFxVolume, 1f, callback);
    }

    # endregion


    /// <summary>
    /// Ư���� ������ҽ����� Ŭ���� ����ϴ� �Լ�   
    /// </summary>
    /// <param name="audio_source">�����ϴ� ������ҽ�/ ä��</param>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="playback_position">���۽���</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    private void PlayMusicFromSource(ref AudioSource audio_source, AudioClip clip, float playback_position, float pitch)
    {
        try
        {
            audio_source.clip = clip;
            audio_source.time = playback_position;
            audio_source.pitch = pitch = Mathf.Clamp(pitch, -3f, 3f);
            audio_source.Play();
        }
        catch (NullReferenceException nre)
        {
            Debug.LogError(nre.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// ���� ������ҽ����� ����� Ŭ���� ����ϴ� �Լ� (�����Լ�)
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="playback_position">���۽���</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    private void PlayBackgroundMusic(AudioClip clip, float playback_position, float pitch)
    {
        PlayMusicFromSource(ref musicSource, clip, playback_position, pitch);
        // ���� Ŭ�������� �ִ� Ŭ�� ����
        backgroundMusic.NextClip = null;
        // ���� Ŭ�������� �־�α�
        backgroundMusic.CurrentClip = clip;
        // ũ�ν����̵忡 �ִ� Ŭ���� ����
        if (crossfadeSource != null)
        {
            Destroy(crossfadeSource);
            crossfadeSource = null;
        }
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ��� </param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="playback_position">���۽���</param>
    public void PlayBGM(AudioClip clip, MusicTransition transition, float transition_duration, float volume, float pitch, float playback_position = 0)
    {
        // �䱸Ŭ���� ���ų� �Ȱ��� Ŭ���̸� ������� ����.
        if (clip == null || backgroundMusic.CurrentClip == clip)
        {
            return;
        }

        // ù ��°�� �÷����� �����̰ų� ��ȯ�ð��� 0�̸� - ��ȯȿ�� ���� ���̽�
        if (backgroundMusic.CurrentClip == null || transition_duration <= 0)
        {
            transition = MusicTransition.Swift;
        }

        // ��ȯȿ�� ���� ���̽� ����
        if (transition == MusicTransition.Swift)
        {
            PlayBackgroundMusic(clip, playback_position, pitch);
            SetBGMVolume(volume);
        }
        else
        {
            // ��ȯȿ�� �������� �� ����
            if (backgroundMusic.NextClip != null)
            {
                Debug.LogWarning("Trying to perform a transition on the background music while one is still active");
                return;
            }

            // ��ȯȿ�� ������ ��ȯ������ ����, �� �� �����鵵..
            backgroundMusic.MusicTransition = transition;
            transitionTime = backgroundMusic.TransitionDuration = transition_duration;
            musicVolCap = _musicVolume;
            backgroundMusic.NextClip = clip;

            // ũ�ν����̵� ó��
            if (backgroundMusic.MusicTransition == MusicTransition.CrossFade)
            {
                // ��ȯȿ�� �������� �� ����
                if (crossfadeSource != null)
                {
                    Debug.LogWarning("Trying to perform a transition on the background music while one is still active");
                    return;
                }

                // ũ�ν����̵� ����� �ʱ�ȭ
                crossfadeSource = ConfigureAudioSource(gameObject.AddComponent<AudioSource>());

                crossfadeSource.volume = Mathf.Clamp01(musicVolCap - currentMusicVol);
                crossfadeSource.priority = 0;

                PlayMusicFromSource(ref crossfadeSource, backgroundMusic.NextClip, 0, pitch);
            }
        }
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    public void PlayBGM(AudioClip clip, MusicTransition transition, float transition_duration, float volume)
    {
        PlayBGM(clip, transition, transition_duration, volume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    public void PlayBGM(AudioClip clip, MusicTransition transition, float transition_duration)
    {
        PlayBGM(clip, transition, transition_duration, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="transition">��ȯ���</param>
    public void PlayBGM(AudioClip clip, MusicTransition transition)
    {
        PlayBGM(clip, transition, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� �ٷ� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    public void PlayBGM(AudioClip clip)
    {
        PlayBGM(clip, MusicTransition.Swift, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip_path">Resources ������ �ִ� Ŭ�� ���</param>
    /// <param name="transition">��ȯ��� </param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="playback_position">���۽���</param>
    public void PlayBGMFromPath(string clip_path, MusicTransition transition, float transition_duration, float volume, float pitch, float playback_position = 0)
    {
        PlayBGM(LoadClip(clip_path), transition, transition_duration, volume, pitch, playback_position);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip_path">Resources ������ �ִ� Ŭ�� ���</param>
    /// <param name="transition">��ȯ��� </param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    /// <param name="volume">���� ũ��</param>
    public void PlayBGMFromPath(string clip_path, MusicTransition transition, float transition_duration, float volume)
    {
        PlayBGM(LoadClip(clip_path), transition, transition_duration, volume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip_path">Resources ������ �ִ� Ŭ�� ���</param>
    /// <param name="transition">��ȯ��� </param>
    /// <param name="transition_duration">��ȯ�ð�</param>
    public void PlayBGMFromPath(string clip_path, MusicTransition transition, float transition_duration)
    {
        PlayBGM(LoadClip(clip_path), transition, transition_duration, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip_path">Resources ������ �ִ� Ŭ�� ���</param>
    /// <param name="transition">��ȯ��� </param>
    public void PlayBGMFromPath(string clip_path, MusicTransition transition)
    {
        PlayBGM(LoadClip(clip_path), transition, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� �ٷ� ���
    /// ������� �� ���� �� ���� ���.
    /// </summary>
    /// <param name="clip_path">Resources ������ �ִ� Ŭ�� ���</param>
    public void PlayBGMFromPath(string clip_path)
    {
        PlayBGM(LoadClip(clip_path), MusicTransition.Swift, 1f, _musicVolume, 1f);
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    public void StopBGM()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    /// <summary>
    /// ����� �Ͻ�����
    /// </summary>
    public void PauseBGM()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    /// <summary>
    /// ����� �ٽ����
    /// </summary>
    public void ResumeBGM()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    /// <summary>
    /// ��� ȿ�������� ���Ǵ� ���� �⺻�Լ�
    /// ȿ������ ���� Ư�� �׸��� �ʱ�ȭ��.
    /// </summary>
    /// <param name="audio_clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <returns>Newly created gameobject with sound effect and audio source attached</returns>
    private GameObject CreateSoundFx(AudioClip audio_clip, Vector2 location)
    {
        // �ӽ� ������Ʈ
        GameObject host = new GameObject("TempAudio");
        host.transform.position = location;
        host.transform.SetParent(transform);
        host.AddComponent<SoundEffect>();

        // ������ҽ� �߰�
        AudioSource audioSource = host.AddComponent<AudioSource>() as AudioSource;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // �ͼ� �׷��� ����� ���
        audioSource.outputAudioMixerGroup = _soundFxMixerGroup;

        audioSource.clip = audio_clip;
        audioSource.mute = !_soundFxOn;

        return host;
    }

    /// <summary>
    /// ��� ȿ�������� ���Ǵ� ���� �⺻�Լ� (Vector3.zero ��ġ ����)
    /// ȿ������ ���� Ư�� �׸��� �ʱ�ȭ��.
    /// </summary>
    /// <param name="audio_clip">����� Ŭ��</param>
    /// <returns>Newly created gameobject with sound effect and audio source attached</returns>
    private GameObject CreateSoundFx(AudioClip audio_clip, Vector3 location)
    {
        // �ӽ� ������Ʈ
        GameObject host = new GameObject("TempAudio");
        host.transform.position = location;
        host.transform.SetParent(transform);
        host.AddComponent<SoundEffect>();

        // ������ҽ� �߰�
        AudioSource audioSource = host.AddComponent<AudioSource>() as AudioSource;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 50;

        // �ͼ� �׷��� ����� ���
        audioSource.outputAudioMixerGroup = _soundFxMixerGroup;

        audioSource.clip = audio_clip;
        audioSource.mute = !_soundFxOn;

        return host;
    }

    /// <summary>
    /// ȿ������ ȿ���� Ǯ�� �����ϸ� �ε��� �˷��ִ� �Լ�
    /// </summary>
    /// <param name="name">ȿ���� �̸�</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <returns>Index of sound effect or -1 is none exists</returns>
    public int IndexOfSoundFxPool(string name, bool singleton = false)
    {
        int index = 0;
        while (index < sfxPool.Count)
        {
            if (sfxPool[index].Name == name && singleton == sfxPool[index].Singleton)
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="duration">����ð�</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(AudioClip clip, Vector2 location, float duration, float volume, bool singleton = false, float pitch = 1f, Action callback = null)
    {
        if (duration <= 0 || clip == null)
        {
            return null;
        }

        int index = IndexOfSoundFxPool(clip.name, true);

        if (index >= 0)
        {
            // ȿ���� Ǯ�� �����ϸ� ����ð� �缳���ؼ� ������
            SoundEffect singletonSFx = sfxPool[index];
            singletonSFx.Duration = singletonSFx.Time = duration;
            sfxPool[index] = singletonSFx;

            return sfxPool[index].Source;
        }

        GameObject host = null;
        AudioSource source = null;

        host = CreateSoundFx(clip, location);
        source = host.GetComponent<AudioSource>();
        source.loop = duration > clip.length;
        source.volume = _soundFxVolume * volume;
        source.pitch = pitch;

        // ���� ������ ���� ����
        SoundEffect sfx = host.GetComponent<SoundEffect>();
        sfx.Singleton = singleton;
        sfx.Source = source;
        sfx.OriginalVolume = volume;
        sfx.Duration = sfx.Time = duration;
        sfx.Callback = callback;

        // Ǯ�� �ִ´�.
        sfxPool.Add(sfx);

        source.Play();

        return source;
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="duration">����ð�</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(AudioClip clip, Vector2 location, float duration, bool singleton = false, Action callback = null)
    {
        return PlaySFX(clip, location, duration, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ �ð���ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="duration">����ð�</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlaySFX(AudioClip clip, float duration, bool singleton = false, Action callback = null)
    {
        return PlaySFX(clip, Vector2.zero, duration, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(AudioClip clip, Vector2 location, int repeat, float volume, bool singleton = false, float pitch = 1f, Action callback = null)
    {
        if (clip == null)
        {
            return null;
        }

        if (repeat != 0)
        {
            int index = IndexOfSoundFxPool(clip.name, true);

            if (index >= 0)
            {
                // ȿ���� Ǯ�� �����ϸ� ����ð� �缳���ؼ� ������
                SoundEffect singletonSFx = sfxPool[index];
                singletonSFx.Duration = singletonSFx.Time = repeat > 0 ? clip.length * repeat : float.PositiveInfinity;
                sfxPool[index] = singletonSFx;

                return sfxPool[index].Source;
            }

            GameObject host = CreateSoundFx(clip, location);
            AudioSource source = host.GetComponent<AudioSource>();
            source.loop = repeat != 0;
            source.volume = _soundFxVolume * volume;
            source.pitch = pitch;

            // ���� ������ ���� ����
            SoundEffect sfx = host.GetComponent<SoundEffect>();
            sfx.Singleton = singleton;
            sfx.Source = source;
            sfx.OriginalVolume = volume;
            sfx.Duration = sfx.Time = repeat > 0 ? clip.length * repeat : float.PositiveInfinity;
            sfx.Callback = callback;

            // Ǯ�� �ִ´�.
            sfxPool.Add(sfx);

            source.Play();

            return source;
        }

        // repeat ���̰� 1���� �۰ų� ������ ���
        return PlayOneShot(clip, location, volume, pitch, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(AudioClip clip, Vector2 location, int repeat, bool singleton = false, Action callback = null)
    {
        return RepeatSFX(clip, location, repeat, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ������ Ƚ����ŭ ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="repeat">Ŭ���� �󸶳� �ݺ����� ���Ѵ�. ������ ������ �Է��ϸ� ��.</param>
    /// <param name="singleton">ȿ������ �̱������� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource RepeatSFX(AudioClip clip, int repeat, bool singleton = false, Action callback = null)
    {
        return RepeatSFX(clip, Vector2.zero, repeat, _soundFxVolume, singleton, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(AudioClip clip, Vector2 location, float volume, float pitch = 1f, Action callback = null)
    {
        if (clip == null)
        {
            return null;
        }

        GameObject host = CreateSoundFx(clip, location);
        AudioSource source = host.GetComponent<AudioSource>();
        source.loop = false;
        source.volume = _soundFxVolume * volume;
        source.pitch = pitch;

        // ���� ������ ���� ����
        SoundEffect sfx = host.GetComponent<SoundEffect>();
        sfx.Singleton = false;
        sfx.Source = source;
        sfx.OriginalVolume = volume;
        sfx.Duration = sfx.Time = clip.length;
        sfx.Callback = callback;

        // Ǯ�� �ִ´�.
        sfxPool.Add(sfx);

        source.Play();

        return source;
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ���� ��ġ (2D)</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(AudioClip clip, Vector2 location, Action callback = null)
    {
        return PlayOneShot(clip, location, _soundFxVolume, 1f, callback);
    }

    /// <summary>
    /// ���� �����̽�(2D)���� ȿ������ ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayOneShot(AudioClip clip, Action callback = null)
    {
        return PlayOneShot(clip, Vector2.zero, _soundFxVolume, 1f, callback);
    }

    /// <summary>
    /// Ư�� ��ġ ���� �����̽�(3D)���� ȿ������ ����ϰ� ������ ������ �ݹ� �Լ��� ȣ���ϴ� �Լ� (3D Sound Settings Ȱ��)
    /// </summary>
    /// <returns>An AudioSource</returns>
    /// <param name="clip">����� Ŭ��</param>
    /// <param name="location">Ŭ���� ��� ��ġ (3D)</param>
    /// <param name="volume">���� ũ��</param>
    /// <param name="pitch">Ŭ���� ��ġ ���� ����</param>
    /// <param name="callback">����� ������ �ݹ��� �׼�</param>
    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 location, float volume, float pitch = 1f, Action callback = null)
    {
        if (clip == null)
        {
            return null;
        }
        //AudioSource.PlayClipAtPoint(clip, location);

        GameObject host = CreateSoundFx(clip, location);
        AudioSource source = host.GetComponent<AudioSource>();
        source.loop = false;
        source.volume = _soundFxVolume * volume;
        source.pitch = pitch;

        // ���� ������ ���� ����
        SoundEffect sfx = host.GetComponent<SoundEffect>();
        sfx.Singleton = false;
        sfx.Source = source;
        sfx.OriginalVolume = volume;
        sfx.Duration = sfx.Time = clip.length;
        sfx.Callback = callback;

        // Ǯ�� �ִ´�.
        sfxPool.Add(sfx);

        source.Play();
        //AudioSource source = null;

        return source;
    }

    /// <summary>
    /// ��� ȿ������ �Ͻ�����
    /// </summary>
    public void PauseAllSFX()
    {
        // SoundEffect �� ����
        foreach (SoundEffect sfx in FindObjectsOfType<SoundEffect>())
        {
            if (sfx.Source.isPlaying) sfx.Source.Pause();
        }
    }

    /// <summary>
    /// ��� ȿ������ �ٽ����
    /// </summary>
    public void ResumeAllSFX()
    {
        foreach (SoundEffect sfx in FindObjectsOfType<SoundEffect>())
        {
            if (!sfx.Source.isPlaying) sfx.Source.UnPause();
        }
    }

    /// <summary>
    /// ��� ȿ������ ����
    /// </summary>
    public void StopAllSFX()
    {
        foreach (SoundEffect sfx in FindObjectsOfType<SoundEffect>())
        {
            if (sfx.Source)
            {
                sfx.Source.Stop();
                Destroy(sfx.gameObject);
            }
        }

        sfxPool.Clear();
    }

    /// <summary>
    /// Resources �������� ����� Ŭ���� �������� �Լ�
    /// </summary>
    /// <param name="path">Resources ������ Ŭ�� ���</param>
    /// <param name="add_to_playlist">�ε��� Ŭ���� ���߿� ������ ���ؼ� �÷��� ����Ʈ�� �߰��ϴ� �ɼ�</param>
    /// <returns>The Audioclip from the resource folder</returns>
    public AudioClip LoadClip(string path, bool add_to_playlist = false)
    {
        AudioClip clip = Resources.Load(path) as AudioClip;
        if (clip == null)
        {
            Debug.LogError(string.Format("AudioClip '{0}' not found at location {1}", path, System.IO.Path.Combine(Application.dataPath, "/Resources/" + path)));
            return null;
        }

        if (add_to_playlist)
        {
            AddToPlaylist(clip);
        }

        return clip;
    }

    /// <summary>
    /// URL ��η� ����� Ŭ���� �������� �Լ�
    /// </summary>
    /// <param name="path">����� Ŭ�� �ٿ�ε� URL. ��: 'http://www.my-server.com/audio.ogg'</param>
    /// <param name="audio_type">�ٿ�ε带 ���� ����� ���ڵ� Ÿ��. AudioType ����</param>
    /// <param name="add_to_playlist">�ε��� Ŭ���� ���߿� ������ ���ؼ� �÷��� ����Ʈ�� �߰��ϴ� �ɼ�</param>
    /// <param name="callback">�ε尡 �Ϸ�Ǹ� �ݹ��� �׼�.</param>
    public void LoadClip(string path, AudioType audio_type, bool add_to_playlist, Action<AudioClip> callback)
    {
        StartCoroutine(LoadAudioClipFromUrl(path, audio_type, (downloadedContent) => {
            if (downloadedContent != null && add_to_playlist)
            {
                AddToPlaylist(downloadedContent);
            }

            callback.Invoke(downloadedContent);
        }));
    }

    /// <summary>
    /// URL ��η� ����� Ŭ�� �������� ���� �Լ�
    /// </summary>
    /// <returns>The audio clip from URL.</returns>
    /// <param name="audio_url">����� URL</param>
    /// <param name="audio_type">����� Ÿ��</param>
    /// <param name="callback">�ݹ� �׼�</param>
    IEnumerator LoadAudioClipFromUrl(string audio_url, AudioType audio_type, Action<AudioClip> callback)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(audio_url, audio_type))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(string.Format("Error downloading audio clip at {0} : {1}", audio_url, www.error));
            }

            callback.Invoke(UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www));
        }
    }

    /// <summary>
    /// �����, ȿ���� On/Off ��� �Լ�
    /// </summary>
    /// <param name="flag">On - true, Off - false</param>
    private void ToggleMute(bool flag)
    {
        ToggleBGMMute(flag);
        ToggleSFXMute(flag);
    }

    /// <summary>
    /// ����� On/Off ��� �Լ�
    /// </summary>
    /// <param name="flag">On - true, Off - false</param>
    private void ToggleBGMMute(bool flag)
    {
        musicOn = _musicOn = flag;
        musicSource.mute = !musicOn;
    }

    /// <summary>
    /// ȿ���� On/Off ��� �Լ�
    /// </summary>
    /// <param name="flag">On - true, Off - false</param>
    private void ToggleSFXMute(bool flag)
    {
        sfxOn = _soundFxOn = flag;

        foreach (SoundEffect sfx in FindObjectsOfType<SoundEffect>())
        {
            sfx.Source.mute = !sfxOn;
        }
    }

    /// <summary>
    /// ����� ���� ũ������ �Լ�
    /// </summary>
    /// <param name="volume">New volume of the background music.</param>
    private void SetBGMVolume(float volume)
    {
        try
        {
            volume = Mathf.Clamp01(volume);
            // ��� ���� ũ�� ������ �Ҵ�
            musicSource.volume = currentMusicVol = _musicVolume = volume;

            if (_musicMixerGroup != null && !string.IsNullOrEmpty(_volumeOfMusicMixer.Trim()))
            {
                float mixerVol = -80f + (volume * 100f);
                _musicMixerGroup.audioMixer.SetFloat(_volumeOfMusicMixer, mixerVol);
            }
        }
        catch (NullReferenceException nre)
        {
            Debug.LogError(nre.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// ȿ���� ���� ũ������ �Լ�
    /// </summary>
    /// <param name="volume">New volume for all the sound effects.</param>
    private void SetSFXVolume(float volume)
    {
        try
        {
            volume = Mathf.Clamp01(volume);
            currentSfxVol = _soundFxVolume = volume;

            foreach (SoundEffect sfx in FindObjectsOfType<SoundEffect>())
            {
                sfx.Source.volume = _soundFxVolume * sfx.OriginalVolume;
                sfx.Source.mute = !_soundFxOn;
            }

            if (_soundFxMixerGroup != null && !string.IsNullOrEmpty(_volumeOfSFXMixer.Trim()))
            {
                float mixerVol = -80f + (volume * 100f);
                _soundFxMixerGroup.audioMixer.SetFloat(_volumeOfSFXMixer, mixerVol);
            }
        }
        catch (NullReferenceException nre)
        {
            Debug.LogError(nre.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// ����� ������ ���� ũ�⸦ 0- 1 �� ����ȭ�ϴ� �Լ�
    /// </summary>
    /// <returns>The normalised volume between the range of zero and one.</returns>
    /// <param name="vol">���� ũ��</param>
    private float NormaliseVolume(float vol)
    {
        vol += 80f;
        vol /= 100f;
        return vol;
    }

    /// <summary>
    /// ����� ���� ũ�⸦ PlayerPrefs���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    private float LoadBGMVolume()
    {
        return PlayerPrefs.HasKey(BgMusicVolKey) ? PlayerPrefs.GetFloat(BgMusicVolKey) : _musicVolume;
    }

    /// <summary>
    /// ȿ���� ���� ũ�⸦ PlayerPrefs���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    private float LoadSFXVolume()
    {
        return PlayerPrefs.HasKey(SoundFxVolKey) ? PlayerPrefs.GetFloat(SoundFxVolKey) : _soundFxVolume;
    }

    /// <summary>
    /// int���� bool������ ��ȯ�ϴ� �Լ�
    /// </summary>
    private bool ToBool(int integer)
    {
        return integer == 0 ? false : true;
    }

    /// <summary>
    /// ����� On/Off ���θ� PlayerPrefs���� �������� �Լ�
    /// </summary>
    /// <returns>Returns the value of the background music mute key from the saved preferences if it exists or the defaut value if it does not</returns>
    private bool LoadBGMMuteStatus()
    {
        return PlayerPrefs.HasKey(BgMusicMuteKey) ? ToBool(PlayerPrefs.GetInt(BgMusicMuteKey)) : _musicOn;
    }

    /// <summary>
    /// ȿ���� On/Off ���θ� PlayerPrefs���� �������� �Լ�
    /// </summary>
    /// <returns>Returns the value of the sound effect mute key from the saved preferences if it exists or the defaut value if it does not</returns>
    private bool LoadSFXMuteStatus()
    {
        return PlayerPrefs.HasKey(SoundFxMuteKey) ? ToBool(PlayerPrefs.GetInt(SoundFxMuteKey)) : _soundFxOn;
    }

    /// <summary>
    /// ����� On/Off ���ο� ���� ũ�⸦ PlayerPrefs�� �����ϴ� �Լ�
    /// </summary>
    public void SaveBGMPreferences()
    {
        PlayerPrefs.SetInt(BgMusicMuteKey, _musicOn ? 1 : 0);
        PlayerPrefs.SetFloat(BgMusicVolKey, _musicVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ȿ���� On/Off ���ο� ���� ũ�⸦ PlayerPrefs�� �����ϴ� �Լ�
    /// </summary>
    public void SaveSFXPreferences()
    {
        PlayerPrefs.SetInt(SoundFxMuteKey, _soundFxOn ? 1 : 0);
        PlayerPrefs.SetFloat(SoundFxVolKey, _soundFxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ��� PlayerPrefs �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    public void ClearAllPreferences()
    {
        PlayerPrefs.DeleteKey(BgMusicVolKey);
        PlayerPrefs.DeleteKey(SoundFxVolKey);
        PlayerPrefs.DeleteKey(BgMusicMuteKey);
        PlayerPrefs.DeleteKey(SoundFxMuteKey);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ��� ���� �ɼ��� PlayerPrefs�� �����ϴ� �Լ�
    /// </summary>
    public void SaveAllPreferences()
    {
        PlayerPrefs.SetFloat(SoundFxVolKey, _soundFxVolume);
        PlayerPrefs.SetFloat(BgMusicVolKey, _musicVolume);
        PlayerPrefs.SetInt(SoundFxMuteKey, _soundFxOn ? 1 : 0);
        PlayerPrefs.SetInt(BgMusicMuteKey, _musicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ����� Ŭ�� ����Ʈ�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    public void EmptyPlaylist()
    {
        _playlist.Clear();
    }

    /// <summary>
    /// ����� Ŭ�� ����Ʈ�� ����� Ŭ���� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    public void AddToPlaylist(AudioClip clip)
    {
        if (clip != null)
        {
            _playlist.Add(clip);
        }
    }

    /// <summary>
    /// ����� Ŭ�� ����Ʈ�� ����� Ŭ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="clip">����� Ŭ��</param>
    public void RemoveFromPlaylist(AudioClip clip)
    {
        if (clip != null && GetClipFromPlaylist(clip.name))
        {
            _playlist.Remove(clip);
            _playlist.Sort((x, y) => x.name.CompareTo(y.name));
        }
    }

    /// <summary>
    /// ����� �̸����� ����� Ŭ�� ����Ʈ���� ����� Ŭ�� �������� �Լ�
    /// </summary>
    /// <param name="clip_name">Ŭ�� �̸�</param>
    /// <returns>The AudioClip from the pool or null if no matching name can be found</returns>
    public AudioClip GetClipFromPlaylist(string clip_name)
    {

        for (int i = 0; i < _playlist.Count; i++)
        {
            if (clip_name == _playlist[i].name)
            {
                return _playlist[i];
            }
        }

        Debug.LogWarning(clip_name + " does not exist in the playlist.");
        return null;
    }

    /// <summary>
    /// Resources ���� ��ο� �ִ� ��� ����� Ŭ���� ����� Ŭ�� ����Ʈ�� �������� �Լ�
    /// </summary>
    /// <param name="path">Resoures ���� �� ������� ��) "" �Է� �� Resources �� ��� Ŭ���� ������.</param>
    /// <param name="overwrite">������� ����, true - ����Ʈ �����, false - ����Ʈ�� ���޾Ƽ� �߰�</param>
    public void LoadPlaylist(string path, bool overwrite)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);

        // ���ο� ����Ʈ�� ������� üũ
        if (clips != null && clips.Length > 0 && overwrite)
        {
            _playlist.Clear();
        }

        for (int i = 0; i < clips.Length; i++)
        {
            _playlist.Add(clips[i]);
        }
    }

    /// <summary>
    /// ���� ����� Ŭ���� �������� �Ӽ�
    /// </summary>
    /// <value>The current music clip.</value>
    public AudioClip CurrentMusicClip
    {
        get { return backgroundMusic.CurrentClip; }
    }

    /// <summary>
    /// ȿ���� Ǯ�� �������� �Ӽ�
    /// </summary>
    public List<SoundEffect> SoundFxPool
    {
        get { return sfxPool; }
    }

    /// <summary>
    /// ����� �Ŵ����� Ŭ�� ����Ʈ�� �������� �Ӽ�
    /// </summary>
    public List<AudioClip> Playlist
    {
        get { return _playlist; }
    }

    /// <summary>
    /// ������� ��������� üũ�ϴ� �Ӽ�
    /// </summary>
    public bool IsMusicPlaying
    {
        get { return musicSource != null && musicSource.isPlaying; }
    }

    /// <summary>
    /// ����� ���� ũ�⸦ �������ų� �����ϴ� �Ӽ�
    /// </summary>
    /// <value>���� ũ��</value>
    public float MusicVolume
    {
        get { return _musicVolume; }
        set { SetBGMVolume(value); }
    }

    /// <summary>
    /// ȿ���� ���� ũ�⸦ �������ų� �����ϴ� �Ӽ�
    /// </summary>
    /// <value>���� ũ��</value>
    public float SoundVolume
    {
        get { return _soundFxVolume; }
        set { SetSFXVolume(value); }
    }

    /// <summary>
    /// ����� On/Off üũ�ϰų� �����ϴ� �Ӽ�
    /// </summary>
    /// <value><c>true</c> - BGM On; <c>false</c> - BGM Off</value>
    public bool IsMusicOn
    {
        get { return _musicOn; }
        set { ToggleBGMMute(value); }
    }

    /// <summary>
    /// ȿ���� On/Off üũ�ϰų� �����ϴ� �Ӽ�
    /// </summary>
    /// <value><c>true</c> - SFX On; <c>false</c> - SFX Off</value>
    public bool IsSoundOn
    {
        get { return _soundFxOn; }
        set { ToggleSFXMute(value); }
    }

    /// <summary>
    /// ������� ȿ���� On/Off üũ�ϰų� �����ϴ� �Ӽ�
    /// </summary>
    /// <value><c>true</c> - BGM+SFX On; <c>false</c> - BGM+SFX Off</value>
    public bool IsMasterMute
    {
        get { return !_musicOn && !_soundFxOn; }
        set { ToggleMute(value); }
    }

}

/// <summary>
/// ��ȯȿ��
/// </summary>
public enum MusicTransition
{
    /// <summary>
    /// (����) ���������� ��� ���
    /// </summary>
    Swift,
    /// <summary>
    /// (���̵� ��/�ƿ�) ���̵� �ƿ��ǰ� ���� ���� ���̵� ��
    /// </summary>
    LinearFade,
    /// <summary>
    /// (ũ�ν�) �������ǰ� ���������� ũ�ν�
    /// </summary>
    CrossFade
}

/// <summary>
/// ����� ����
/// </summary>
[System.Serializable]
public struct BackgroundMusic
{
    /// <summary>
    /// ����� ���� Ŭ��
    /// </summary>
    public AudioClip CurrentClip;
    /// <summary>
    /// ����� ���� Ŭ��
    /// </summary>
    public AudioClip NextClip;
    /// <summary>
    /// ��ȯȿ��
    /// </summary>
    public MusicTransition MusicTransition;
    /// <summary>
    /// ��ȯȿ�� �ð�
    /// </summary>
    public float TransitionDuration;
}

/// <summary>
/// ȿ���� ������ ����
/// </summary>
[System.Serializable]
public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float originalVolume;
    [SerializeField] private float duration;
    [SerializeField] private float playbackPosition;
    [SerializeField] private float time;
    [SerializeField] private Action callback;
    [SerializeField] private bool singleton;

    /// <summary>
    /// ȿ���� �̸� �Ӽ�
    /// </summary>
    /// <value>�̸�</value>
    public string Name
    {
        get { return audioSource.clip.name; }
    }

    /// <summary>
    /// ȿ���� ���� �Ӽ� (�� ����)
    /// </summary>
    /// <value>����</value>
    public float Length
    {
        get { return audioSource.clip.length; }
    }

    /// <summary>
    /// ȿ���� ����� �ð� �Ӽ� (�� ����)
    /// </summary>
    /// <value>����� �ð�</value>
    public float PlaybackPosition
    {
        get { return audioSource.time; }
    }

    /// <summary>
    /// ȿ���� Ŭ�� �Ӽ�
    /// </summary>
    /// <value>����� Ŭ��</value>
    public AudioSource Source
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

    /// <summary>
    /// ȿ���� ���� ���� �Ӽ�
    /// </summary>
    /// <value>���� ���� ũ��</value>
    public float OriginalVolume
    {
        get { return originalVolume; }
        set { originalVolume = value; }
    }

    /// <summary>
    /// ȿ���� �� ����ð� �Ӽ� (�ʴ���)
    /// </summary>
    /// <value>�� ����ð�</value>
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    /// <summary>
    /// ȿ���� ���� ����ð� �Ӽ� (�ʴ���)
    /// </summary>
    /// <value>���� ����ð�</value>
    public float Time
    {
        get { return time; }
        set { time = value; }
    }

    /// <summary>
    /// ȿ���� ����ȭ�� ������൵ �Ӽ� (����ȭ 0~1)
    /// </summary>
    /// <value>����ȭ�� ������൵</value>
    public float NormalisedTime
    {
        get { return Time / Duration; }
    }

    /// <summary>
    /// ȿ���� �Ϸ� �� �ݹ� �׼� �Ӽ�
    /// </summary>
    /// <value>�ݹ� �׼�</value>
    public Action Callback
    {
        get { return callback; }
        set { callback = value; }
    }

    /// <summary>
    /// ȿ���� �ݺ� �� �̱��� ����, �ݺ��� ��쿡 true �ƴϸ� false
    /// </summary>
    /// <value><c>true</c> �ݺ� ��; �� ��, <c>false</c>.</value>
    public bool Singleton
    {
        get { return singleton; }
        set { singleton = value; }
    }
}