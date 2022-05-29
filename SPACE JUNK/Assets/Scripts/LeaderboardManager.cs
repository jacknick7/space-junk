using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    private const int LEADERBOARD_SIZE = 5;
    private const string SAVE_FILE_NAME = "/gamedata.bin";

    private LeaderboardData lbData;

    [SerializeField] private TextMeshProUGUI[] nameText;
    [SerializeField] private TextMeshProUGUI[] scoreText;


    void Start()
    {
        LoadLeaderboard();
        DisplayLeaderboard();
    }


    private void CreateLeaderboard()
    {
        lbData = new LeaderboardData();
        lbData.leaderboard = new lbentry[LEADERBOARD_SIZE];
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            lbData.leaderboard[i].isUsed = false;
        }
    }

    // TODO: find a way to encrypt the leaderboard so it can't be modified easly by the player
    private void LoadLeaderboard()
    {
        string path = Application.persistentDataPath + SAVE_FILE_NAME;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            lbData = formatter.Deserialize(stream) as LeaderboardData;
        }
        else
        {
            CreateLeaderboard();
            SaveLeaderboard();
        }
    }


    private void SaveLeaderboard()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SAVE_FILE_NAME;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, lbData);
        stream.Close();
    }


    public bool IsNewRecord(int newScore)
    {
        if (!lbData.leaderboard[0].isUsed || lbData.leaderboard[0].score < newScore) return true;
        return false;
    }


    public void AddNewRecord(string newName, int newScore)
    {
        int newRecordIndex = -1;
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            if (lbData.leaderboard[i].isUsed && lbData.leaderboard[i].score > newScore)
            {
                newRecordIndex = i - 1;
                break;
            }
        }
        if (newRecordIndex < 0) newRecordIndex = LEADERBOARD_SIZE - 1;

        for (int i = 0; i < newRecordIndex; i++)
        {
            if (lbData.leaderboard[i + 1].isUsed)
            {
                lbData.leaderboard[i].name = lbData.leaderboard[i + 1].name;
                lbData.leaderboard[i].score = lbData.leaderboard[i + 1].score;
            }
            lbData.leaderboard[i].isUsed = lbData.leaderboard[i + 1].isUsed;
        }

        lbData.leaderboard[newRecordIndex].isUsed = true;
        lbData.leaderboard[newRecordIndex].name = newName;
        lbData.leaderboard[newRecordIndex].score = newScore;

        SaveLeaderboard();
        DisplayLeaderboard();
    }


    private void DisplayLeaderboard()
    {
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            if (lbData.leaderboard[i].isUsed)
            {
                nameText[i].text = lbData.leaderboard[i].name;
                scoreText[i].text = lbData.leaderboard[i].score.ToString();
            }
            else
            {
                nameText[i].text = "---";
                scoreText[i].text = "---";
            }
        }
    }
}
