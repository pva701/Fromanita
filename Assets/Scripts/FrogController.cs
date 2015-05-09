using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
    public float moveTime;

    private float inverseMoveTime;
    private Animator animator;
    private Rigidbody2D rb2D;
    private bool isMove = false;
    private float STEP = 10.2f;

	void Awake() {
        inverseMoveTime = 1.0f / moveTime;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
	}


    protected IEnumerator SmoothMovementByX(float xDir)
    {
        isMove = true;
        float speedByX = xDir * inverseMoveTime;
        float rem = xDir;
        Vector3 shiftVector = new Vector3(speedByX * Time.deltaTime, 0, 0);
        int iteration = (int)(moveTime / Time.deltaTime);
        for (int i = 0; i < iteration; ++i) {
            Vector3 newPos = transform.position + shiftVector;
            rb2D.MovePosition(newPos);
            yield return null;
        }
        isMove = false;
    }

    void MoveByX(float xDir)
    {
        //rb2D.velocity += new Vector2(xDir, 0);
        //Vector3 newPos = transform.position + new Vector3(xDir, 0, 0);
        rb2D.AddForce(new Vector2(xDir, 0));
        //rb2D.AddRelativeForce(new Vector2(xDir, 0));
        //rb2D.AddForce(new Vector2(xDir, 0));
        //return;
        /*if (!isMove)
        {
            print("move");
            StartCoroutine(SmoothMovementByX(xDir));
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            print("ground");
            animator.SetTrigger("frogDie");
        } 
        else
        {
            print("amanita");
            animator.SetTrigger("frogJump");
        }
    }

    int counter = 0;
	void Update () {
        //rb2D.AddForce(Physics.gravity);
        //rb2D.velocity = Physics.gravity;
        //++counter;
        //print(counter);
        if (InputManager.instance.isLeft) MoveByX(-STEP);
        else if (InputManager.instance.isRight) MoveByX(STEP);

        if (counter == 50)
        {
            counter = 0;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(1, 0, 0));
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
	}
}
