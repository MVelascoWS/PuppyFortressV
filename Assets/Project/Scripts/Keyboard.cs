
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public static Keyboard instance;
    public TextMeshProUGUI recordName;
    public AudioClip keyTap;

    private char[] nameStr = { '_', '_', '_', '_', '_', '_', '_', '_', '_', '_' };
    private int characters = 0;
    private string letters = "abcdefghijklmnopqrstuvwxyz";
    private string[] names;
    private string[] points;
    private int newRecord = 0;
    private int newRecordIndex = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadRecords();
    }

    public void AddLetter(int letter)
    {
        if (characters >= 10)
            return;
        nameStr[characters] = letters[letter];
        recordName.text = new string(nameStr);
        characters++;
        AudioManager.instance.PlaySound(keyTap);
    }

    public void DeleteLetter()
    {
        if (characters <= 0)
            return;
        characters--;
        nameStr[characters] = '_';
        recordName.text = new string(nameStr);
        AudioManager.instance.PlaySound(keyTap);
    }

    public void LoadRecords()
    {
        names = new string[5];
        points = new string[5];

        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("Name" + i.ToString("00")))
            {
                names[i] = PlayerPrefs.GetString("Name" + i.ToString("00"));
            }
            else
            {
                PlayerPrefs.SetString("Name" + i.ToString("00"), "__________");
                names[i] = PlayerPrefs.GetString("Name" + i.ToString("00"));
            }

            if (PlayerPrefs.HasKey("Points" + i.ToString("00")))
            {
                points[i] = PlayerPrefs.GetString("Points" + i.ToString("00"));
            }
            else
            {
                PlayerPrefs.SetString("Points" + i.ToString("00"), "0");
                points[i] = PlayerPrefs.GetString("Points" + i.ToString("00"));
            }
        }
    }

    public bool CheckIfNewRecord(int pointsToCheck)
    {
        for (int i = 0; i < 5; i++)
        {
            if (pointsToCheck > int.Parse(points[i]))
            {
                newRecord = pointsToCheck;
                newRecordIndex = i;
                return true;
            }
        }
        return false;
    }

    public void SaveRecords()
    {
        PlayerPrefs.SetString("Points" + newRecordIndex.ToString("00"), newRecord.ToString());
        PlayerPrefs.SetString("Name" + newRecordIndex.ToString("00"), new string(nameStr));

        for (int i = newRecordIndex + 1; i < 5; i++)
        {
            PlayerPrefs.SetString("Points" + i.ToString("00"), points[i - 1]);
            PlayerPrefs.SetString("Name" + i.ToString("00"), names[i - 1]);
        }

        LoadRecords();
        AudioManager.instance.PlaySound(keyTap);
    }

    public void Reset()
    {
        for (int i = 0; i < 10; i++)
            nameStr[i] = '_';
    }
}
