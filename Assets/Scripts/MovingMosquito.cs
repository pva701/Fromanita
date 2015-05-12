using UnityEngine;
using System.Collections;

public class MovingMosquito : MonoBehaviour {

    private float changeVelocityTime = 1f;
    private Rigidbody2D rg2D;
    private float y;
    private float liveTime;
    private float xMax;

    void Awake() {
         rg2D = GetComponent<Rigidbody2D>();
    }

    public void StartMoving(float liveTime)
    {
        /*Rect rect = GameManager.instance.GetFieldRect();
        y = Random.Range(rect.yMin + 2, rect.yMax - 2);
        transform.position =  new Vector2(rect.xMin, y);
        rg2D.velocity = new Vector2((5.64f + 2.34f) / liveTime, 0);*/

        this.liveTime = liveTime;
        Rect rect = GameManager.instance.GetFieldRect();
        y = Random.Range(rect.yMin + 2, rect.yMax - 2);
        print("y = " + y);
        transform.position = new Vector2(rect.xMin, y);
        xMax = rect.xMax;
        StartCoroutine(ChangeVelocity());
    }

    private IEnumerator ChangeVelocity()
    {
        float currentTime = 0;
        rg2D.velocity = new Vector2(1, 0);
        while (currentTime < liveTime)
        {
            float angle = Random.Range(-10.0f, 10.0f);
            //print("angle = " + angle);
            rg2D.velocity = turn(rg2D.velocity, angle);
            //print("vel = " + rg2D.velocity);
            currentTime += changeVelocityTime;
            yield return new WaitForSeconds(changeVelocityTime);
        }
    }

    private Vector2 turn(Vector2 v, float angle)
    {
        angle =  angle / 180 * Mathf.PI;
        float x = v.x * Mathf.Cos(angle) - v.y * Mathf.Cos(angle);
        float y = v.y * Mathf.Cos(angle) + v.x * Mathf.Cos(angle);
        v.x = x;
        v.y = y;
        return v;
    }
}
