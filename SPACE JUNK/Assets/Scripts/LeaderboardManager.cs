using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardManager : MonoBehaviour
{
    private const int LEADERBOARD_SIZE = 5;

    private const string SAVE_FILE_NAME = "/gamedata.bin";

    private LeaderboardData lb;


    void Start()
    {
        LoadLeaderboard();
    }


    private void CreateLeaderboard()
    {
        lb = new LeaderboardData();
        lb.isUsed = new bool[LEADERBOARD_SIZE];
        lb.name = new string[LEADERBOARD_SIZE];
        lb.score = new int[LEADERBOARD_SIZE];
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            lb.isUsed[i] = false;
        }
    }


    private void LoadLeaderboard()
    {
        string path = Application.persistentDataPath + SAVE_FILE_NAME;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            lb = formatter.Deserialize(stream) as LeaderboardData;
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
        formatter.Serialize(stream, lb);
        stream.Close();
    }


    public bool IsNewRecord(int newScore)
    {
        if (!lb.isUsed[0] || lb.score[0] < newScore) return true;
        return false;
    }


    public void AddNewRecord(string newName, int newScore)
    {
        int newRecordIndex = -1;
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            if (lb.isUsed[i] && lb.score[i] > newScore)
            {
                newRecordIndex = i - 1;
                break;
            }
        }
        if (newRecordIndex < 0) newRecordIndex = LEADERBOARD_SIZE - 1;

        for (int i = 0; i < newRecordIndex; i++)
        {
            if (lb.isUsed[i + 1])
            {
                lb.name[i] = lb.name[i + 1];
                lb.score[i] = lb.score[i + 1];
            }
            lb.isUsed[i] = lb.isUsed[i + 1];
        }

        lb.isUsed[newRecordIndex] = true;
        lb.name[newRecordIndex] = newName;
        lb.score[newRecordIndex] = newScore;

        SaveLeaderboard();
    }


    public void DisplayLeaderboard()
    {
        Debug.Log("Position   Name   Score");
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            if (lb.isUsed[i]) Debug.Log((LEADERBOARD_SIZE - i) + "   ---   ---");
            else Debug.Log((LEADERBOARD_SIZE - i) + "   " + lb.name[i] + "   " + lb.score[i]);
        }
    }
}
