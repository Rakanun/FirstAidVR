using UnityEngine;

// Singleton audio manager providing an 8-channel pooled playback system.
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private const int AUDIO_CHANNEL_NUM = 8;

    private struct Channel
    {
        public AudioSource source;
        public float keyOnTime; // Timestamp of the most recent play call on this channel
    }

    private Channel[] m_channels;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        m_channels = new Channel[AUDIO_CHANNEL_NUM];
        for (int i = 0; i < AUDIO_CHANNEL_NUM; i++)
        {
            m_channels[i].source = gameObject.AddComponent<AudioSource>();
            m_channels[i].keyOnTime = 0f;
        }
    }

    // Plays a one-shot clip. Prevents double-triggering the same clip within 30ms.
    // Returns the channel index used, or -1 if playback was skipped.
    public int PlayOneShot(AudioClip clip, float volume, float pitch = 1.0f)
    {
        for (int i = 0; i < m_channels.Length; i++)
        {
            if (m_channels[i].source.isPlaying &&
                m_channels[i].source.clip == clip &&
                m_channels[i].keyOnTime >= Time.time - 0.03f)
                return -1;
        }

        int oldest = -1;
        float oldestTime = float.MaxValue;

        for (int i = 0; i < m_channels.Length; i++)
        {
            if (!m_channels[i].source.isPlaying)
            {
                PlayOnChannel(i, clip, volume, pitch, false);
                return i;
            }

            if (!m_channels[i].source.loop && m_channels[i].keyOnTime < oldestTime)
            {
                oldest = i;
                oldestTime = m_channels[i].keyOnTime;
            }
        }

        // No free channel — evict the oldest non-looping one
        if (oldest >= 0)
        {
            PlayOnChannel(oldest, clip, volume, pitch, false);
            return oldest;
        }

        return -1;
    }

    // Plays a looping clip (e.g. background music). Returns the channel index, or -1 if all channels are busy.
    public int PlayLoop(AudioClip clip, float volume, float pan, float pitch = 1.0f)
    {
        for (int i = 0; i < m_channels.Length; i++)
        {
            if (!m_channels[i].source.isPlaying)
            {
                m_channels[i].source.panStereo = pan;
                PlayOnChannel(i, clip, volume, pitch, true);
                return i;
            }
        }

        return -1;
    }

    public void StopAll()
    {
        foreach (Channel channel in m_channels)
            channel.source.Stop();
    }

    public void Stop(int id)
    {
        if (id >= 0 && id < m_channels.Length)
            m_channels[id].source.Stop();
    }

    private void PlayOnChannel(int index, AudioClip clip, float volume, float pitch, bool loop)
    {
        m_channels[index].source.clip = clip;
        m_channels[index].source.volume = volume;
        m_channels[index].source.pitch = pitch;
        m_channels[index].source.loop = loop;
        m_channels[index].source.Play();
        m_channels[index].keyOnTime = Time.time;
    }
}
