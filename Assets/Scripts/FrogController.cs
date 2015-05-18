//#define INERTIA
using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
    public float moveTime;

    private Animator animator;
    private Rigidbody2D rb2D;
    private float STEP_FORCE = 10f;

	void Awake() {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            //print("ground");
            GameManager.instance.GameOver();
        } else if (collision.gameObject.name == "Amanita")
        {
            //print("amanita");
            animator.SetTrigger("frogJump");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO write add score
        if (collider.gameObject.tag == "Mosquito")
            DestroyObject(collider.gameObject);
    }

#if INERTIA
    void Update () {
        if (InputManager.instance.isLeft) rb2D.AddForce(new Vector2(-STEP_FORCE, 0));
        else if (InputManager.instance.isRight) rb2D.AddForce(new Vector2(STEP_FORCE, 0));
        else if (InputManager.instance.isDown) rb2D.AddForce(new Vector2(0, -STEP_FORCE * 2));
    }
#else

	void FixedUpdate() {
		Vector2 v = rb2D.velocity;
		v.x = 0;
		rb2D.velocity = v;
	}

    void Update () {
        if (InputManager.instance.isLeft) {
			Vector2 pos = rb2D.position;
			pos += new Vector2(-0.1f, 0f);
			rb2D.position = pos;
		} else if (InputManager.instance.isRight) {
			Vector2 pos = rb2D.position;
			pos += new Vector2(0.1f, 0f);
			rb2D.position = pos;
		} else if (InputManager.instance.isDown) {
            print("down");
            rb2D.AddForce(new Vector2(0, -STEP_FORCE*2));
        }
	}
#endif

    public void Die()
    {
        enabled = false;
        //animator.SetTrigger("frogDie");
        //TODO will good
    }
}
