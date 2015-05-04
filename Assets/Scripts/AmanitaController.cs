using UnityEngine;
using System.Collections;

public class AmanitaController : MonoBehaviour {
	private Rigidbody2D rb;
	public float force = 1.0f;
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (new Vector2(force, 0));
	}


	void Update () {

	}
}
