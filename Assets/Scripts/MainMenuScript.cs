using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Text;

[Serializable]
public class HighScore
{
    public string name;
    public int score;
    public DateTime timeOfEntry;
}

[Serializable]
public class HighScores
{
    public List<HighScore> Scores;
}

public class MainMenuScript : MonoBehaviour {
	private string state = "FirstMenu";
	private string userName = "Enter your name here";
	private static bool hasSoundTrack = false;
	public AudioSource soundtrack;
    private const string ServerGetUrl = "http://flake.tech:9090/api/HighScore";
    private const string ServerPostUrl = "http://flake.tech:9090/api/HighScore?password=PeopleCantPostWithoutPlaying";
    private List<HighScore> highScores = new List<HighScore>();
    private DateTime LastGet = DateTime.MinValue;
    private DateTime LastPost = DateTime.MinValue;

    private void Start()
    {
        if(CanGet())
            StartCoroutine(GetFromServer());
    }

    void OnGUI(){
		if (!hasSoundTrack) {
			hasSoundTrack = true;
			soundtrack.Play ();
		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		if (ApplicationModel.Score != 0)
			state = "ResultMenu";

		if (state == "FirstMenu")
			FirstMenu ();	
		if (state == "HighScoreMenu")
			HighScoreMenu ();
		if (state == "ResultMenu")
			ResultsMenu ();

    }

    void FirstMenu()
	{
		if (GUI.Button (new Rect ((Screen.width / 2) - 200, (Screen.height / 2) - 10, 150, 40), "New game")) 
		{
			TimeKeeper.timeLeft = 120;
			DontDestroyOnLoad (GameObject.Find ("Soundtrack"));
			float fadeTime = GameObject.Find ("Soundtrack").GetComponent<FadeScene>().BeginFade (1);
			SceneManager.UnloadSceneAsync("MainMenuScene");
			SceneManager.LoadScene ("HuntingGameScene");
		}
		if (GUI.Button (new Rect ((Screen.width / 2) + 100, (Screen.height / 2) - 10, 150, 40), "Show highscores")) 
		{
			state = "HighScoreMenu";
		}
	}

	void HighScoreMenu(){
        if(CanGet())
            StartCoroutine(GetFromServer());

        if (GUI.Button (new Rect ((Screen.width / 2) + 100, (Screen.height / 2) - 10, 150, 40), "Back")) 
		{
			state = "FirstMenu";
		}
		string scoresText = "There are no highscores... yet";

		if (highScores != null && highScores.Any()) {
			scoresText = "";
			foreach (HighScore s in highScores) {
				scoresText += s.score + "\t" + s.name + "\n";
			}
		}
		GUI.TextArea (new Rect ((Screen.width / 2) - 400, (Screen.height / 2) - 200, 450, 400), scoresText);
	}

	void ResultsMenu(){
		// User has played
		GUI.TextArea (new Rect ((Screen.width / 2) - 400, (Screen.height / 2) - 200, 150, 150), "Your score was: " + ApplicationModel.Score );
		userName = GUI.TextField(new Rect ((Screen.width / 2) - 200, (Screen.height / 2) - 200, 150, 40), userName);
		if (GUI.Button (new Rect ((Screen.width / 2) - 200, (Screen.height / 2) + 0, 150, 40), "Submit score")) 
		{
            HighScore score = new HighScore
            {
                name = userName,
                score = ApplicationModel.Score
            };
            highScores.Add(
                new HighScore
                {
                    name = userName,
                    score = ApplicationModel.Score
                }
            );
            highScores = highScores.OrderByDescending(doc => doc.score).ToList();
            if(CanPost())
                StartCoroutine(PushToServer(score));

            ApplicationModel.Score = 0;
			state = "HighScoreMenu";
		}
	}

    public IEnumerator PushToServer(HighScore score)
    {
        string postDataJson = JsonUtility.ToJson(score);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" }
        };

        byte[] body = Encoding.UTF8.GetBytes(postDataJson);

        WWW www = new WWW(ServerPostUrl, body, headers);

        yield return www;
    }

    public IEnumerator GetFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get(ServerGetUrl);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            Debug.Log("{ \"Scores\":" + www.downloadHandler.text + "}");

            highScores = (JsonUtility.FromJson<HighScores>(EncapsulateJsonList(www.downloadHandler.text)))
                .Scores
                .ToList();
        }
    }

    private bool CanPost()
    {
        var res = DateTime.Now > LastPost.AddSeconds(10);
        LastPost = DateTime.Now;
        return res;
    }

    private bool CanGet()
    {
        var res = DateTime.Now > LastGet.AddSeconds(10);
        LastGet = DateTime.Now;
        return res;
    }

    private string EncapsulateJsonList(string jsonList)
    {
        // JsonUtil needs lists to be encapsulated
        return "{ \"Scores\":" + jsonList + "}";
    }
}

