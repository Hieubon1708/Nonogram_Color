using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool IsActiveMusic
    {
        get
        {
            return PlayerPrefs.GetInt("Music", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Music", value ? 1 : 0);
        }
    }

    public bool IsAtiveSound
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
        }
    }

    public bool IsActiveVibrate
    {
        get
        {
            return PlayerPrefs.GetInt("Vibrate", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Vibrate", value ? 1 : 0);
        }
    }
}
