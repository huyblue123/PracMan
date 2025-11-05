using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource ghostSource;   // Ghost loop
    [SerializeField] private AudioSource pracmanSource; // Pacman loop
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip ghostMoveClip;
    [SerializeField] private AudioClip ghostScaredClip;
    [SerializeField] private AudioClip pracManMoveClip;
    [SerializeField] private AudioClip pracManDieClip; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ==== GHOST SOUNDS ====
    public void PlayGhostMoveSound()
    {
        if (ghostSource.clip == ghostMoveClip && ghostSource.isPlaying) return;

        ghostSource.Stop();
        ghostSource.loop = true;
        ghostSource.clip = ghostMoveClip;
        ghostSource.Play();
    }

    public void PlayGhostScared()
    {
        if (ghostSource.clip == ghostScaredClip && ghostSource.isPlaying) return;

        ghostSource.Stop();
        ghostSource.loop = true;
        ghostSource.clip = ghostScaredClip;
        ghostSource.Play();
    }

    // ==== PRACMAN SOUNDS ====
    public void PlayPracManMoveLoop()
    {
        pracmanSource.clip = pracManMoveClip;
        pracmanSource.loop = true;
        pracmanSource.Play();
    }

    public void StopPracManMoveLoop()
    {
        pracmanSource.Stop();
    }

    public void PlayPracManDieClip()
    {
        sfxSource.PlayOneShot(pracManDieClip);
    }
    public void StopAllAudio()
    {
        //startGameAudioSource.Stop();
        ghostSource.Stop();
        pracmanSource.Stop();
    }
}
