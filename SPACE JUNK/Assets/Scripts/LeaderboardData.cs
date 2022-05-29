[System.Serializable]
public struct lbentry
{
    public bool isUsed;
    public string name;
    public int score;
}


[System.Serializable]
public class LeaderboardData
{
    public lbentry[] leaderboard;
}
