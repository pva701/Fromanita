using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
	private Rigidbody2D rb;
	public float moveTime = .5f;
	private float inverseMoveTime;
	public float power = 1f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}
	
	// Update is called once per frame
	void Update () {
		int h = 0;
		if (Input.GetAxis ("Horizontal") > float.Epsilon)
			h = 1;
		if (Input.GetAxis ("Horizontal") < -float.Epsilon)
			h = -1;
		if (h == 0)
			return;
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (h, 0);
		//Vector3 np = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
		Vector2 f = new Vector2 (h * power, 0);
		rb.AddForce (f);
		//rb.MovePosition (np);
	}
}
