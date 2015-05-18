using UnityEngine;
using System.Collections;

public class AmanitaController : MonoBehaviour {
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private Transform transform;
	private Animator animator;

	public float hSpeed = 0.05f;

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
		Move (dir, 0);
		if (counter++ == 100) {
			counter = 0;
			if (Random.Range (0, 2) == 0)
				animator.SetTrigger ("needBlink");
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		print ("Collision " + collision.gameObject.name);
		//return;
		if (collision.gameObject.name == "Wall")
		{
			ChangeDirection();
		} else if (collision.gameObject.name == "Stump")
		{
			ChangeDirection();
		}
	}

	private void ChangeDirection() {
		Vector3 tmp = transform.localScale;
		tmp.x *= -1;
		dir *= -1;
		transform.localScale = tmp;
	}
	
	protected void Move (int xDir, int yDir) 
	{
		Vector2 pos = rb.position;
		pos += new Vector2 (dir, 0) * hSpeed;
		rb.position = pos;
	}

    public void StopMoving()
    {
        enabled = false;
        //TODO write this
    }
	
}
