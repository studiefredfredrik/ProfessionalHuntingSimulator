using UnityEngine;

public class GiveTheFinger : MonoBehaviour {
	private Vector3 storagePoint = new Vector3(-0.54f,-2.46f,-1.92f);
	private Vector3 endPoint = new Vector3(-0.54f,-1.46f,-1.92f);
	private GameObject hand;
    private float step;

    public float speed;

    void Start () {
		hand = GameObject.Find ("Rigged Hand");
	}
	
	void Update () {
		step = speed * Time.deltaTime;
		if (Input.GetButton ("Fire2")) 
		{
			hand.transform.localPosition = Vector3.Lerp (hand.transform.localPosition, endPoint, step);
		}
		else
		{
			hand.transform.localPosition = Vector3.Lerp (hand.transform.localPosition, storagePoint, step);
		}
	}
}