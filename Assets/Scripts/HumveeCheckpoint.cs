using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HumveeCheckpoint : MonoBehaviour {
	public Text InfoText;

    void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "FPSController") {
			InfoText.text = "Press E to leave the hunting zone...";
		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.name == "FPSController" && Input.GetButtonDown ("e")) {
			ApplicationModel.Score = Projectile.currentScore;
			SceneManager.UnloadSceneAsync("HuntingGameScene");
			SceneManager.LoadScene ("MainMenuScene");
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.name == "FPSController"){
			InfoText.text = "";	
		}
	}

}
