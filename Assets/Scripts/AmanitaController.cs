using UnityEngine;
using System.Collections;

public class AmanitaController : MonoBehaviour {
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private Transform transform;
	private Animator animator;

	public float moveTime = .5f;
	private float inverseMoveTime;
	private int dir = 1;
	private int counter = 0;
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		transform = GetComponent<Transform> ();
		animator = GetComponent<Animator> ();
		inverseMoveTime = 1f / moveTime;
	}

	void Update () {
		//Move (dir, 0);
		if (counter++ == 100) {
			counter = 0;
			if (Random.Range (0, 2) == 0)
				animator.SetTrigger ("needBlink");
		}
	}

	protected void Move (int xDir, int yDir) 
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		RaycastHit2D hit = Physics2D.Linecast (start, end);
		boxCollider.enabled = true;
		if (hit.transform != null) {
			Vector3 tmp = transform.localScale;
			tmp.x *= -1;
			dir *= -1;
			transform.localScale = tmp;
		}
		Vector3 np = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
		rb.MovePosition(np);
	}
	
}
