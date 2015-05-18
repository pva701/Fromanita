using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public float minForce = .5f;
	public float maxForce = 1.0f;

	// Use this for initialization
	void Start () {
		float force = Random.Range (minForce, maxForce);
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0f;
		rb.AddForce (new Vector2 (force, 0f));
		BackgroundManager.instance.cloudCount += 1;
	}

	void Update() {

	}

	void OnDestroy() {
		BackgroundManager.instance.cloudCount -= 1;
	}

	void OnBecameInvisible() {
		DestroyObject (gameObject);
	}
}
