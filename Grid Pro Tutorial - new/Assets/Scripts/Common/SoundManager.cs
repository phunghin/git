using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public enum SoundType
{
    Replace,
    New,
    Only,
    Loop
}

public enum MusicID
{
    MainMenu,
    MainGame,

    Count
}

public enum SoundID
{
    Move,
	Die,
	Break,

    Count
}

public class SoundManager : Singleton<SoundManager>
{
	/// <summary>
	/// The music clips.
	/// </summary>
	public AudioClip[] musicClips = new AudioClip[(int)MusicID.Count];

	/// <summary>
	/// The sound clips.
	/// </summary>
	public AudioClip[] soundClips = new AudioClip[(int)SoundID.Count];
    
    /// <summary>
    /// The music volume.
    /// </summary>
    [Range(0, 1)]
    public float musicVolume = 1f;

    /// <summary>
    /// The sound volume.
    /// </summary>
    [Range(0, 1)]
    public float soundVolume = 1f;

	// The audio source to play music
	private AudioSource _musicSource;

	// The audio sources to play sound effects
	private AudioSource[] _soundSources;

	// True if enable to play background music
    private bool _isMusicEnabled = true;

	// True if enable to play sound effect
    private bool _isSoundEnabled = true;

    protected override void Awake()
    {
        base.Awake();

		// Create music source
		_musicSource = gameObject.AddComponent<AudioSource>();
		_musicSource.loop = true;
		_musicSource.volume = musicVolume;

		int soundCount = soundClips.Length;

		// Create sound sources
		_soundSources = new AudioSource[soundCount];

		for (int i = 0; i < soundCount; i++)
		{
			AudioSource soundSource = gameObject.AddComponent<AudioSource>();
			soundSource.clip = soundClips[i];
			soundSource.playOnAwake = false;
			soundSource.volume = soundVolume;

			_soundSources[i] = soundSource;
		}
    }

    /// <summary>
	/// Change music volume.
    /// </summary>
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = Mathf.Clamp01(value);

			if (_isMusicEnabled)
			{
				_musicSource.volume = musicVolume;
			}
        }
    }

    /// <summary>
	/// Change sound volume.
    /// </summary>
    public float SoundVolume
    {
        get
        {
            return soundVolume;
        }
        set
        {
            soundVolume = Mathf.Clamp01(value);

			for (int i = 0; i < _soundSources.Length; i++)
			{
				_soundSources[i].volume = soundVolume;
			}
        }
    }

    /// <summary>
    /// Enable/Disable music.
    /// </summary>
    public bool MusicEnabled
    {
        get
        {
            return _isMusicEnabled;
        }
        set
        {
            if (_isMusicEnabled != value)
            {
                _isMusicEnabled = value;

				_musicSource.volume = _isMusicEnabled ? musicVolume : 0;
            }
        }
    }

    /// <summary>
	/// Enable/Disable sound.
    /// </summary>
    public bool SoundEnabled
    {
        get
        {
            return _isSoundEnabled;
        }
        set
        {
            if (_isSoundEnabled != value)
            {
                _isSoundEnabled = value;

                if (!_isSoundEnabled)
                {
					for (int i = 0; i < _soundSources.Length; i++)
					{
						_soundSources[i].enabled = false;
					}
                }
            }
        }
    }

	public static bool PlayMusic(MusicID musicID)
	{
		return Instance.PlayMusicInternal((int)musicID);
	}

	public static void StopMusic()
	{
		Instance.StopMusicInternal();
	}
	
	bool PlayMusicInternal(int musicID)
	{
		// Get audio clip
		AudioClip audioClip = musicClips[musicID];

		if (audioClip != null)
		{
			// Set clip
			_musicSource.clip = audioClip;

			// Play music
			_musicSource.Play();

			// Set volume
			_musicSource.volume = _isMusicEnabled ? musicVolume : 0;

			return true;
		}

		return false;
	}

	void StopMusicInternal()
	{
		_musicSource.Stop();
	}

	public static bool PlaySound(SoundID soundID, SoundType type = SoundType.Replace, float delay = 0f)
	{
		return Instance.PlaySoundInternal((int)soundID, type, delay);
	}

	public static bool PlayRandomSound(params SoundID[] soundIDs)
	{
		return PlaySound(soundIDs[Random.Range(0, soundIDs.Length - 1)]);
	}

	public static void StopSound(SoundID soundID)
	{
		Instance.StopSoundInternal((int)soundID);
	}

	public static void StopAllSounds()
	{
		Instance.StopAllSoundsInternal();
	}

	public static bool IsSoundFinished(SoundID soundID)
	{
		return Instance.IsSoundFinishedInternal((int)soundID);
	}

    bool PlaySoundInternal(int soundID, SoundType type, float delay)
    {
        if (!_isSoundEnabled) return false;

        // Get audio source
		AudioSource audioSource = _soundSources[soundID];

        if (audioSource != null)
        {
            // Set enabled
            audioSource.enabled = true;

            if (type == SoundType.Loop)
            {
                audioSource.loop = true;

                if (!audioSource.isPlaying)
                {
                    if (delay > 0)
                    {
                        audioSource.PlayDelayed(delay);
                    }
                    else
                    {
                        audioSource.Play();
                    }
                }
            }
            else
            {
                audioSource.loop = false;

                if (type == SoundType.Replace)
                {
                    if (delay > 0)
                    {
                        audioSource.PlayDelayed(delay);
                    }
                    else
                    {
                        audioSource.Play();
                    }
                }
                else if (type == SoundType.New)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else if (type == SoundType.Only)
                {
                    if (!audioSource.isPlaying)
                    {
                        if (delay > 0)
                        {
                            audioSource.PlayDelayed(delay);
                        }
                        else
                        {
                            audioSource.Play();
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }

    void StopSoundInternal(int soundID)
    {
		AudioSource audioSource = _soundSources[soundID];

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.enabled = false;
        }
	}

	void StopAllSoundsInternal()
	{
		for (int i = 0; i < _soundSources.Length; i++)
		{
			_soundSources[i].Stop();
			_soundSources[i].enabled = false;
		}
	}

    bool IsSoundFinishedInternal(int soundID)
    {
		AudioSource audioSource = _soundSources[soundID];

        if (audioSource != null)
        {
            return !audioSource.isPlaying;
        }

        return true;
    }
}
