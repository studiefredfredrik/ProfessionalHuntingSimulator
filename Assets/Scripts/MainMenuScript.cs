using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// The XML we save is composed of a List<> og HighScore
public class HighScore
{
    public string Name;
    public int Score;
    public int ShotsFire;	// Not implemented yet, but can be nice to see the 
	public int ShotsHit;	// shot/kill ratio of each player	
    public int SecondsUsed;
}


public class MainMenuScript : MonoBehaviour {
	private string state = "FirstMenu";
	private string lastState = "";
	private string userName = "Enter your name here";
	private static bool hasSoundTrack = false;
	public AudioSource soundtrack;


	void OnGUI(){
		if (!hasSoundTrack) {
			hasSoundTrack = true;
			soundtrack.Play ();
		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		if (ApplicationModel.Score != 0)
			state = "ResultMenu";
		if (state != lastState) {
			if (state == "FirstMenu")
				FirstMenu ();	
			if (state == "HighScoreMenu")
				HighScoreMenu ();
			if (state == "ResultMenu")
				ResultsMenu ();
		}
	}

	public static void WriteXML(List<HighScore> scores)
	{
		PlayerPrefs.SetString ("HighScores", scores.ToXmlString ());
		PlayerPrefs.Save ();
	}

	public List<HighScore> ReadXML()
	{
		if (PlayerPrefs.GetString ("HighScores") == "")return new List<HighScore> ();
		System.Xml.Serialization.XmlSerializer serializer = 
			new System.Xml.Serialization.XmlSerializer(typeof(List<HighScore> ));
		using(StringReader stringReader = new StringReader(PlayerPrefs.GetString ("HighScores")))
		{
			List<HighScore> scores = (List<HighScore>)serializer.Deserialize(stringReader);
			return scores;
		}
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
		if (GUI.Button (new Rect ((Screen.width / 2) + 100, (Screen.height / 2) - 10, 150, 40), "Back")) 
		{
			state = "FirstMenu";
		}
		string scoresText = "There are no highscores... yet";

		if (PlayerPrefs.GetString ("HighScores") != "") {
			scoresText = "";
			List <HighScore> scores = ReadXML ();
			scores = scores.OrderByDescending (x => x.Score).ToList();
			foreach (HighScore s in scores) {
				scoresText += s.Score + "\t" + s.Name + "\n";
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
			List<HighScore> scores;
			if (PlayerPrefs.GetString ("HighScores") != "")
				scores = ReadXML ();
			else
				scores = new List<HighScore>();
			HighScore score = new HighScore();
			score.Name = userName;
			score.Score = ApplicationModel.Score;
			scores.Add(score);
			WriteXML(scores);
			ApplicationModel.Score = 0;
			state = "HighScoreMenu";
		}
	}
}