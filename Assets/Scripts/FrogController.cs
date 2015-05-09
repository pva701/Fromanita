using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
    public float moveTime;

    private float inverseMoveTime;
    private Animator animator;
    private Rigidbody2D rb2D;
    private bool isMove = false;
    private float STEP = 10.0f;

	void Awake() {
        inverseMoveTime = 1.0f / moveTime;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
	}


    void MoveByX(float xDir)
    {
        rb2D.AddForce(new Vector2(xDir, 0));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            print("ground");
            //TODO write check if bounds
            GameManager.instance.GameOver();
        } else
        {
            print("amanita");
            animator.SetTrigger("frogJump");
        }
    }

	void Update () {
        if (InputManager.instance.isLeft) MoveByX(-STEP);
        else if (InputManager.instance.isRight) MoveByX(STEP);
	}

    public void DieAnimation()
    {
        animator.SetTrigger("frogDie");
    }
}
