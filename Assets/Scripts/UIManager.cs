using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SoundPack soundPack;

    [SerializeField] private Button buttonPause;
    [SerializeField] private Image buttonPauseOffImg;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Image buttonPlayOffImg;
    [SerializeField] private Button buttonForce;
    [SerializeField] private Image buttonForceOffImg;
    private int playSpeed;

    [SerializeField] private Button buttonSound;
    [SerializeField] private Sprite buttonSoundOnSpr;
    [SerializeField] private Sprite buttonSoundOffSpr;
    private bool soundOn;

    [SerializeField] private Image winLoseImg;
    [SerializeField] private Image winImg;
    [SerializeField] private Image loseImg;

    private Image buttonSoundImg;
    private AudioSource gameSoundAudio;

    private void Start()
    {
        winImg.gameObject.SetActive(false);
        loseImg.gameObject.SetActive(false);
        winLoseImg.gameObject.SetActive(false);

        buttonSoundImg = buttonSound.GetComponent<Image>();

        gameSoundAudio = soundPack.GetAudio("gameSound");

        soundOn = true;
        gameSoundAudio.clip = soundPack.GetClip("gameSound");
        gameSoundAudio.loop = true;
        gameSoundAudio.mute = false;
        gameSoundAudio.volume = 0.3f;
        gameSoundAudio.pitch = 1;

        OnClickPlayPauseForce(buttonPause);
    }

    private void ChangePlaySpeed(int playSp)
    {
        Time.timeScale = playSp;

        if (playSp == 0)
            gameSoundAudio.Pause();
        else if (playSp == 1)
        {
            gameSoundAudio.Play();
            gameSoundAudio.pitch = 1;
            gameSoundAudio.volume = 0.4f;
        }
        else
        {
            gameSoundAudio.Play();
            gameSoundAudio.pitch = 1.3f;
            gameSoundAudio.volume = 0.5f;
        }
    }

    public void ViewWinLoseWindow(bool win)
    {
        if (win)
        {
            winImg.gameObject.SetActive(true);
            loseImg.gameObject.SetActive(false);
        }
        else
        {
            winImg.gameObject.SetActive(false);
            loseImg.gameObject.SetActive(true);
        }
        winLoseImg.gameObject.SetActive(true);
        OnClickPlayPauseForce(buttonPause);
    }

    public void OnClickAgain()
    {
        winLoseImg.gameObject.SetActive(false);
        OnClickPlayPauseForce(buttonPlay);
    }

    public void OnEventPlaySound(AudioSource src)
    {
        if (soundOn)
        {
            src.playOnAwake = false;
            src.loop = false;
            if (playSpeed == 1)
                src.pitch = 1;
            else if (playSpeed == 3)
                src.pitch = 1.3f;
            src.Play();
        }
    }

    public void OnEventPlayLost(AudioSource src)
    {
        if (soundOn)
        {
            src.volume = 0.65f;
            src.PlayDelayed(0.4f);
        }
    }

    public void OnEventPlayWin(AudioSource src)
    {
        if (soundOn)
        {
            src.volume = 0.65f;
            src.PlayDelayed(0.1f);
        }
    }

    public void OnClickPlaySound(AudioSource src)
    {
        if (soundOn)
        {
            src.playOnAwake = false;
            src.loop = false;
            src.volume = 0.75f;
            src.SetScheduledStartTime(0.1f);
            src.Play();
        }
    }

    public void OnClickPlaySoundIgnoreMute(AudioSource src)
    {
        src.playOnAwake = false;
        src.loop = false;
        src.volume = 0.75f;
        src.SetScheduledStartTime(0.1f);
        src.Play();
    }

    public void OnClickPlayPauseForce(Button b)
    {
        buttonPause.interactable = b == buttonPause ? false : true;
        buttonPlay.interactable = b == buttonPlay ? false : true;
        buttonForce.interactable = b == buttonForce ? false : true;

        if (buttonPause.interactable)
            buttonPauseOffImg.gameObject.SetActive(false);
        else
            buttonPauseOffImg.gameObject.SetActive(true);
        if (buttonPlay.interactable)
            buttonPlayOffImg.gameObject.SetActive(false);
        else
            buttonPlayOffImg.gameObject.SetActive(true);
        if (buttonForce.interactable)
            buttonForceOffImg.gameObject.SetActive(false);
        else
            buttonForceOffImg.gameObject.SetActive(true);

        if (b == buttonPause)
            playSpeed = 0;
        else if (b == buttonPlay)
            playSpeed = 1;
        else if (b == buttonForce)
            playSpeed = 3;

        ChangePlaySpeed(playSpeed);
    }

    public void OnClickSoundOnOff()
    {
        if (soundOn)
        {
            buttonSoundImg.sprite = buttonSoundOffSpr;
            soundOn = false;
            gameSoundAudio.mute = true;
        }
        else
        {
            buttonSoundImg.sprite = buttonSoundOnSpr;
            soundOn = true;
            gameSoundAudio.mute = false;
        }
    }
}
