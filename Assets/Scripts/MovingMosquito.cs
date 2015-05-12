using UnityEngine;
using System.Collections;

public class MovingMosquito : MonoBehaviour {

    public float moveTime = 0.1f;

    private float y;
    private Vector2 startPoint;
    private Vector2 currentPoint;
    private Vector2 step;
    private int totalIterations;
    private int curIteration = 0;
    private Rigidbody2D rg2D;
    private float liveTime;

    void Awake() {
         rg2D = GetComponent<Rigidbody2D>();
    }

    public void StartMoving(float liveTime)
    {
        y = Random.Range(3.0f, 7.5f);
        print("xMin = " + GameManager.instance.GetFieldRect().xMin);
        transform.position =  new Vector2(GameManager.instance.GetFieldRect().xMin, y);
        //int it = (int)(liveTime / moveTime);
        //rg2D.velocity = new Vector2((5.64f + 2.34f) / liveTime, 0);
        //StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        for (; ; )
        {
            //rg2D.AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
            /*Vector2 point = routeGenerator.NextPoint();
            if (!routeGenerator.HasNextPoint())
                DestroyObject(gameObject);
            else
                rg2D.MovePosition(point);*/
            yield return new WaitForSeconds(moveTime);
        }
    }
}
