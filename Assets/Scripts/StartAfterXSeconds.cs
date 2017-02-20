using UnityEngine;

public class StartAfterXSeconds : MonoBehaviour {
    private float startTime = 0;
    private bool actionHasRun = false;

    public AudioSource audioSource;
	public int secondsToWait;

    void Start () {
		startTime = Time.time;
	}
	
	void Update () {
		if (!actionHasRun && Time.time - startTime >= secondsToWait) {
			actionHasRun = true;
			audioSource.Play ();
		}
	}
}
