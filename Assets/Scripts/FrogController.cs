using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {
    public float moveTime;

    private float inverseMoveTime;
    private Animator animator;
    private Rigidbody2D rb2D;
    private bool isMove = false;

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
        if (!isMove)
        {
            print("move");
            StartCoroutine(SmoothMovementByX(xDir));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("collision");
        animator.SetTrigger("frogJump");
    }

    int counter = 0;
	void Update () {
        //++counter;
        //print(counter);
        if (InputManager.instance.isLeft) MoveByX(-1);
        else if (InputManager.instance.isRight) MoveByX(1);

        if (counter == 50)
        {
            counter = 0;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(1, 0, 0));
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
	}
}
