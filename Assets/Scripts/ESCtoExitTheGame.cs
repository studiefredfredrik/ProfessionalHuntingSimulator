using UnityEngine;

public class ESCtoExitTheGame : MonoBehaviour {
	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}