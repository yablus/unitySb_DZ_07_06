using UnityEngine;

public class SoundPack : MonoBehaviour
{
    [SerializeField] private AudioSource warriorCreatedSound;
    [SerializeField] private AudioSource peasantCreatedSound;
    [SerializeField] private AudioSource wheatCreatedSound;
    [SerializeField] private AudioSource timeToEatSound;
    [SerializeField] private AudioSource timeToFightSound;
    [SerializeField] private AudioSource gameWinSound;
    [SerializeField] private AudioSource gameLostSound;

    [SerializeField] private AudioSource gameSoundAudio;
    [SerializeField] private AudioClip gameSoundClip;

    public AudioSource GetAudio(string s)
    {
        switch (s)
        {
            case "gameSound": return gameSoundAudio;
            case "warriorCreated": return warriorCreatedSound;
            case "peasantCreated": return peasantCreatedSound;
            case "wheatCreated": return wheatCreatedSound;
            case "timeToEat": return timeToEatSound;
            case "timeToFight": return timeToFightSound;
            case "gameWin": return gameWinSound;
            case "gameLost": return gameLostSound;
            default: return null;
        }
    }

    public AudioClip GetClip(string c)
    {
        switch (c)
        {
            case "gameSound": return gameSoundClip;
            default: return null;
        }
    }
}