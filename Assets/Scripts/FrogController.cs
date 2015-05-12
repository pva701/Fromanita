using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
    public float moveTime;

    private float inverseMoveTime;
    private Animator animator;
    private Rigidbody2D rb2D;
    private float STEP_FORCE = 10.0f;

	void Awake() {
        inverseMoveTime = 1.0f / moveTime;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
	}


    void MoveByX(float xForce)
    {
        rb2D.AddForce(new Vector2(xForce, 0));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            print("ground");
            GameManager.instance.GameOver();
        } else if (collision.gameObject.name == "Amanita")
        {
            print("amanita");
            animator.SetTrigger("frogJump");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO write add score
        if (collider.gameObject.tag == "Mosquito")
            DestroyObject(collider.gameObject);
    }

	void Update () {
        if (InputManager.instance.isLeft) MoveByX(-STEP_FORCE);
        else if (InputManager.instance.isRight) MoveByX(STEP_FORCE);
	}

    public void Die()
    {
        enabled = false;
        //animator.SetTrigger("frogDie");
        //TODO will good
    }
}
