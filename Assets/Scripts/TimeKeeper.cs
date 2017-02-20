using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeKeeper : MonoBehaviour {
    private float lastTime = 0;
    private bool endingDone = false;

    public Text TimeLeftText;
	public static int timeLeft = 120;
	public Text InfoText;

    void Start () {
		lastTime = Time.time;
	}
	
	void Update () {
		if (Time.time - lastTime >= 1) { // Second has passed
			lastTime = Time.time;
			timeLeft -= 1;
			TimeLeftText.text = "Season ends in: " + timeLeft + "s";
			if (timeLeft <= 0) {
				// game over
				if (!endingDone)
					YouWhereTooLateEnding ();
				else
					EndLevel ();
			}	
		}
	}

	void YouWhereTooLateEnding(){
		Destroy (GameObject.Find ("humvee"));
		InfoText.text = "You where too late, the truck left without you!";
		timeLeft = 5;
		endingDone = true;
	}

	void EndLevel(){
		ApplicationModel.Score = 0;
		SceneManager.UnloadSceneAsync("HuntingGameScene");
		SceneManager.LoadScene ("MainMenuScene");
	}
}
