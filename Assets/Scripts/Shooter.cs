using UnityEngine;

public class Shooter : MonoBehaviour {
	public Rigidbody prefab;
	public float speed;
	public AudioSource fire;

	void Update () {
		if (Input.GetButtonDown ("Fire1")) 
		{
			var obj = (Rigidbody)Instantiate (this.prefab, transform.position+(transform.forward*2), Quaternion.AngleAxis(90, Vector3.up) * transform.rotation );
			obj.velocity = (transform.forward) * speed;
			gameObject.GetComponent < AudioSource > ().Play(); // Gunsound
		}
	}
}