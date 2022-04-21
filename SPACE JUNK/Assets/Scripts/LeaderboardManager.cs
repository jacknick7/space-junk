using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardManager : MonoBehaviour
{
    private const int LEADERBOARD_SIZE = 5;

    private LeaderboardData lb;

    void Start()
    {
        LoadLeaderboard();
    }

    private void CreateLeaderboard()
    {
        lb = new LeaderboardData();
        lb.isUsed = new bool[LEADERBOARD_SIZE];
        lb.names = new string[LEADERBOARD_SIZE];
        lb.scores = new int[LEADERBOARD_SIZE];
        for (int i = 0; i < LEADERBOARD_SIZE; i++)
        {
            lb.isUsed[i] = false;
        }
    }

    private void LoadLeaderboard()
    {
        string path = Application.persistentDataPath + "/gamedata.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            lb = formatter.Deserialize(stream) as LeaderboardData;
        }
        else
        {
            CreateLeaderboard();
            // SaveLeaderboard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
