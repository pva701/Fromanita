using UnityEngine;
using System.Collections;

public class MovingMosquito : MonoBehaviour {

    private float changeVelocityTime = 0.5f;
    private Rigidbody2D rg2D;
    private float y;
    private float liveTime;
    private Rect boundRect;
    private int ANGLE_DIAPASON = 30;
    private Vector2 leftDown;
    private Vector2 leftTop;
    private Vector2 rightDown;
    private Vector2 rightTop;
    private float EPS = 0.0001f;

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
        //print("XMA = " + rect.xMax);
        boundRect = new Rect(rect.xMin, rect.yMin + 2, rect.xMax - rect.xMin, rect.yMax - rect.yMin - 4);
        leftDown = new Vector2(boundRect.xMin, boundRect.yMin);
        leftTop = new Vector2(boundRect.xMin, boundRect.yMax);
        rightDown = new Vector2(boundRect.xMax, boundRect.yMin);
        rightTop = new Vector2(boundRect.xMax, boundRect.yMax);


        /*Vector2 ans;
        print("rightDown = " + rightDown);
        print("rightTop = " + rightTop);
        if (LineIntersectionPoint(new Vector2(-2f, 1.8f), new Vector2(-2f, 1.8f) + new Vector2(1.0f, 0.1f), rightDown, rightTop, out ans))
        {
            print("ans = " + ans);
        }
        else print("Not Int");*/
        transform.position = new Vector2(rect.xMin, y);
        StartCoroutine(ChangeVelocity());
    }

    private IEnumerator ChangeVelocity()
    {
        float currentTime = 0;
        rg2D.velocity = new Vector2(2f, 0f);
        float lenVelocity = rg2D.velocity.magnitude;
        Vector2 currentPosition = transform.position;
        while (currentTime < liveTime)
        {
            /*Rect intersect = IntersectRects(boundRect, new Rect(currentPosition.x - 3, currentPosition.y - 3, 6, 6));

            Vector2 to = new Vector2(Random.Range(intersect.xMin, intersect.xMax),
                                     Random.Range(intersect.yMin, intersect.yMax));
            //Vector2 to = currentPosition + new Vector2(Mathf.Cos(angleTo), Mathf.Sin(angleTo));
            //float len = Random.Range(2f, 0.4f) * (currentPosition - to).magnitude;
            print("cur pos = " + currentPosition);
            while (currentTime < liveTime && Vector2.Distance(currentPosition, to) > 0.1) 
            {
                int angle = Random.Range(-30, 30);
                float cDist = (to - currentPosition).magnitude;
                if (cDist < lenVelocity)
                    rg2D.velocity = to - currentPosition;
                else
                    rg2D.velocity = turn((to - currentPosition) / lenVelocity, angle);
                yield return new WaitForSeconds(changeVelocityTime);
                currentPosition = rg2D.transform.position;
                currentTime += changeVelocityTime;
                print("cur = " + currentPosition + " to = " + to);
            }*/

            int left = -ANGLE_DIAPASON;
            int right = ANGLE_DIAPASON;
            /*if (Random.Range(0, 5) == 0)
            {
                left = 30;
                right = 45;
                if (Random.Range(0, 2) == 0)
                {
                    left = -45;
                    right = -30;
                }
            }*/

            int angle = GetRandomAngle(left, right);
            //print("angle = " + angle);
            rg2D.velocity = turn(rg2D.velocity, angle);
            //print("vel = " + rg2D.velocity);
            yield return new WaitForSeconds(changeVelocityTime);
            currentTime += changeVelocityTime;
        }
        print("EXIT FROM CHANGE");
    }

    private Rect IntersectRects(Rect a, Rect b)
    {
        float x = Mathf.Max(a.xMin, b.xMin);
        float y = Mathf.Max(a.yMin, b.yMin);
        return new Rect(x, y, Mathf.Min(a.xMax, b.xMax) - x, Mathf.Min(a.yMax, b.yMax) - y);
    }

    private int GetRandomAngle(int leftAngle, int rightAngle)
    {
        float p = Random.Range(0f, 1.0f);
        float s = 0, sm = 0;
        int n = rightAngle - leftAngle + 1;
        float[] probs = GetProbablies(leftAngle, rightAngle);
        for (int i = 0; i < n; ++i) sm += probs[i];
        if (Mathf.Abs(sm) < EPS)
            return 180 - Random.Range(-10, 10);

        for (int i = 0; i < n; ++i)
        {
            s += probs[i];
            if (s >= p) return i + leftAngle;
        }
        return rightAngle;
    }

    private float[] GetProbablies(int leftAngle, int rightAngle)
    {
        float[] probs = new float[rightAngle - leftAngle + 1];
        Vector2 start = transform.position;

        for (int i = leftAngle; i <= rightAngle; ++i)
        {
            float ang = i / 180.0f * Mathf.PI;
            Vector2 end = start + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
            Vector2 ans;
            int j = i - leftAngle;
            if (LineIntersectionPoint(start, end, leftTop, leftDown, out ans)) probs[j] = Vector2.Distance(ans, start);
            else if (LineIntersectionPoint(start, end, leftDown, rightDown, out ans)) probs[j] = Vector2.Distance(ans, start);
            else if (LineIntersectionPoint(start, end, rightDown, rightTop, out ans))
            {
                probs[j] = Vector2.Distance(ans, start);
                //print("ang = " + ang);
                //print("dist = " + probs[j] + " start " + start + " vec = " + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) + " ans = " + ans);
            } 
            else if (LineIntersectionPoint(start, end, rightTop, leftTop, out ans)) probs[j] = Vector2.Distance(ans, start);
            //else print("PIZDEC " + start + " " + end);
        }

        float sm = 0;
        int n = rightAngle - leftAngle + 1;
        for (int i = 0; i < n; ++i)
        {
            if (probs[i] < 2) probs[i] = 0;
            if (probs[i] > 0)
                sm += probs[i];
        }

        for (int i = 0; i < n; ++i)
            if (Mathf.Abs(probs[i]) > EPS)
                probs[i] = probs[i] / sm;
            else
                probs[i] = 0;
        return probs;
    }


    private Vector2 turn(Vector2 v, float angle)
    {
        angle =  angle / 180.0f * Mathf.PI;
        float x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
        float y = v.y * Mathf.Cos(angle) + v.x * Mathf.Sin(angle);
        return new Vector2(x, y);
    }

    private bool LineIntersectionPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 ans)//я в ахуе
    {
        Vector2 d1 = b - a;
        Vector2 d2 = d - c;
        float cp = CrossProduct(d1, d2);
        ans = new Vector2();
        if (Mathf.Abs(cp) < EPS)
            return false;
        float t2 = CrossProduct(d1, a - c) / cp;
        float t1 = CrossProduct(d2, a - c) / cp;
        if (Mathf.Abs(t1) < EPS)
        {
            if (CrossProduct(d - a, b - a) >= 0)
                return false;
            ans = a;
            return true;
        }
        if (t1 >= 0 && t2 >= 0 && t2 <= 1) {
            ans = a + d1 * t1;
            return true;
        }
        return false;
    }

    private float CrossProduct(Vector2 a, Vector2 b) {
        return a.x * b.y - a.y * b.x;
    }

}
