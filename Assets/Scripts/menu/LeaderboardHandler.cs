using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

class LeaderboardInformation
{
    public string user_username;
    public int user_plays;
}

public class LeaderboardHandler : MonoBehaviour
{

    public TextMeshProUGUI Leaderboard;
    public Button LeaderboardMenuButton;

    private List<LeaderboardInformation> players;
    
    // Start is called before the first frame update
    void Start()
    {
        LeaderboardMenuButton.onClick.AddListener(delegate
        {
            StartCoroutine(GetLeaderboard());
        });
    }

    private void ParseLeaderboardInformation()
    {
        Debug.Log("running");
        string parsedString = "";
        
        for (int i = 0; i < players.Count; i++)
        {
            parsedString += $"{i+1}. {players[i].user_username} played {players[i].user_plays} times\n";
        }

        Leaderboard.text = parsedString;
    }
    
    IEnumerator GetLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(Domain.subDomain("includes/get-leaderboard.inc.php"));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string json = www.downloadHandler.text;
            players = JsonConvert.DeserializeObject<List<LeaderboardInformation>>(json);
            ParseLeaderboardInformation();
        }
    }
}
