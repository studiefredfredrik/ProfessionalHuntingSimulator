using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour {
	public GameObject bloodSplat;
	public Text scoreKeeper;
	public static int currentScore = 0;

	void Start () {
		scoreKeeper = GameObject.Find ("ScoreText").GetComponent<Text>();
	}
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "animal")
		{
			Destroy(col.gameObject);
			Instantiate (bloodSplat, col.transform.position, col.transform.rotation);
			currentScore += 100;
			scoreKeeper.text = "Score: " + currentScore;
			Destroy(this);
		}
	}
}
