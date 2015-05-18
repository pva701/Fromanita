﻿using UnityEngine;
using System.Collections;

public class AmanitaController : MonoBehaviour {
	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private Transform transform;
	private Animator animator;

	public float moveTime = .5f;
	private float inverseMoveTime;
	private int dir = 1;
    private int SPEED = 1;
	private int counter = 0;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		transform = GetComponent<Transform> ();
		animator = GetComponent<Animator> ();
		inverseMoveTime = 1f / moveTime;
        rb.velocity = new Vector2(SPEED, 0);
	}

	void Update () {
		//Move (dir, 0);
		if (counter++ == 100) {
			counter = 0;
			if (Random.Range (0, 2) == 0)
				animator.SetTrigger ("needBlink");
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//print ("Collision " + collision.gameObject.name);
		//return;
		if (collision.gameObject.name == "Wall")
		{
            //print("WALL");
			ChangeDirection();
		} else if (collision.gameObject.name == "Stump")
		{
			ChangeDirection();
        }
        else if (collision.gameObject.name == "Frog")
        {
            if (dir > 0) rb.velocity = new Vector2(SPEED, 0);
            else rb.velocity = new Vector2(-SPEED, 0);
        }
	}

	private void ChangeDirection() {
		Vector3 tmp = transform.localScale;
		tmp.x *= -1;
		dir *= -1;
		transform.localScale = tmp;
        rb.velocity = new Vector2(dir * SPEED, 0);
	}
	
	protected void Move (int xDir, int yDir) 
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir) / 2;
		boxCollider.enabled = false;
		RaycastHit2D hit = Physics2D.Linecast (start, end);
		boxCollider.enabled = true;
		if (hit.transform != null) {
			ChangeDirection();
		}
		Vector3 np = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
		rb.MovePosition(np);
		//print (np);
	}

    public void StopMoving()
    {
        //rb.velocity = new Vector2(0, 0);
        enabled = false;
        //TODO write this
    }
	
}
