using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MovingMosquito : MonoBehaviour {

    private float changeVelocityTime = 0.1f;
    private Rigidbody2D rg2D;
    private float y;
    private float liveTime;
    private Rect boundRect;
    private int ANGLE_RANGE = 15;
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
        this.liveTime = liveTime;
        Rect rect = GameManager.instance.GetFieldRect();
        Random.seed = System.DateTime.Now.Millisecond;
        print("rect = " + rect.yMax);
        y = Random.Range(rect.yMin + 2, rect.yMax);
        print("y start = " + y);
        //print("XMA = " + rect.xMax);
        boundRect = new Rect(rect.xMin, rect.yMin + 2, rect.xMax - rect.xMin, rect.yMax - rect.yMin - 2);
        leftDown = new Vector2(boundRect.xMin, boundRect.yMin);
        leftTop = new Vector2(boundRect.xMin, boundRect.yMax);
        rightDown = new Vector2(boundRect.xMax, boundRect.yMin);
        rightTop = new Vector2(boundRect.xMax, boundRect.yMax);
        print("yMin = " + leftDown.y);
        print("xMax = " + rightDown.x);

        Vector2 ans;
        print("rightDown = " + rightDown);
        print("rightTop = " + rightTop);
        print("dist = " + FindMinDist(new Vector2(-0.4f, 1.6f), new Vector2(-0.4f, 1.6f) + new Vector2(0.8f, -1.8f)));
        if (LineIntersectionPoint(new Vector2(0.2f, 1.9f), new Vector2(0.2f, 1.9f) + new Vector2(0.9f, -0.5f), leftDown, rightDown, out ans))
        {
            print("ans = " + ans);
        }
        else print("Not Int");

        transform.position = new Vector2(rect.xMin, y);
        StartCoroutine(ChangeVelocity());
    }

    private IEnumerator ChangeVelocity()
    {
        float currentTime = 0;
        rg2D.velocity = new Vector2(2f, 0f);
        float lenVelocity = rg2D.velocity.magnitude;
        Vector2 currentPosition = transform.position;
        int bigRotateLen = 0;
        while (currentTime < liveTime)
        {
            int angGrad; 
            if (bigRotateLen == 0)
                angGrad = GetRandomAngle2(-ANGLE_RANGE, ANGLE_RANGE, lenVelocity);
            else if (bigRotateLen < 0) {
                ++bigRotateLen;
                angGrad = GetRandomAngle2(-ANGLE_RANGE, 0, lenVelocity);
            } else
            {
                --bigRotateLen;
                angGrad = GetRandomAngle2(0, ANGLE_RANGE, lenVelocity);
            }

            float angle =  angGrad / 180.0f * Mathf.PI;
            //Debug.Log("angGrad = " + angGrad);
            Vector2 st = transform.position;
            float ds = FindMinDist(st, st + turnRad(rg2D.velocity, angle));
            //Debug.Log("DS = " + ds);
            if (lenVelocity <= ds &&  ds <= 2 * lenVelocity)
            {
                if (angGrad < 0) bigRotateLen = -Random.Range(5, 10);
                else bigRotateLen = Random.Range(5, 10);
            } else if (ds < lenVelocity)
            {
                float c1 = (90.0f - 2 * ANGLE_RANGE) / ANGLE_RANGE;
                float coef = c1 * (1 - ds / lenVelocity);
                int ang = (int)(2 * ANGLE_RANGE + c1 * ANGLE_RANGE);

                //print("ang = " + ang + " ds = " + ds);
                angGrad = GetRandomAngle2(-90, 90, lenVelocity);
                angle = angGrad / 180.0f * Mathf.PI;
                //print("angGrad = " + angGrad);
                ds = FindMinDist(st, st + turnRad(rg2D.velocity, angle));
                bigRotateLen = 0;
            }

            rg2D.velocity = turnRad(rg2D.velocity, angle) * Mathf.Min(lenVelocity, ds) / rg2D.velocity.magnitude;
            //print("vel = " + rg2D.velocity);
            yield return new WaitForSeconds(changeVelocityTime);
            currentTime += changeVelocityTime;

            if (bigRotateLen == 0) // && Random.Range(0, 4) == 0)
                bigRotateLen = Random.Range(-20, 20);
        }
        if (rg2D.velocity.x < 0)
            rg2D.velocity = new Vector2(-1, Mathf.Sqrt(lenVelocity * lenVelocity - 1));
        else
            rg2D.velocity = new Vector2(1, Mathf.Sqrt(lenVelocity * lenVelocity - 1));
        print("EXIT FROM CHANGE");
    }

    private Rect IntersectRects(Rect a, Rect b)
    {
        float x = Mathf.Max(a.xMin, b.xMin);
        float y = Mathf.Max(a.yMin, b.yMin);
        return new Rect(x, y, Mathf.Min(a.xMax, b.xMax) - x, Mathf.Min(a.yMax, b.yMax) - y);
    }

    private int GetRandomAngle2(int leftAngle, int rightAngle, float lenVelocity)
    {
        List<int> a = new List<int>();
        float mx = 0;
        int ret = 0;
        Vector2 start = transform.position;
        for (int i = leftAngle; i <= rightAngle; ++i)
        {
            float ang = i / 180.0f * Mathf.PI;
            Vector2 end = start + turnRad(rg2D.velocity, ang);
            float d = FindMinDist(start, end);
            if (d > mx) {
                mx = d;
                ret = i;
            }
            if (d >= lenVelocity) a.Add(i);
        }
        if (a.Count == 0)
            return ret;
        return a[Random.Range(0, a.Count)];
    }

    private float FindMinDist(Vector2 start, Vector2 end)
    {
        Vector2 ans;
        float dist = 1000000f;
        if (LineIntersectionPoint(start, end, leftTop, leftDown, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (LineIntersectionPoint(start, end, leftDown, rightDown, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (LineIntersectionPoint(start, end, rightDown, rightTop, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (LineIntersectionPoint(start, end, rightTop, leftTop, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        
        if (dist > 1e3)
        {
            dist = 0;
            print("PIZDEC " + start + " " + (end - start));
        }
        return dist;
    }

    private Vector2 turnRad(Vector2 v, float angle)
    {
        float x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
        float y = v.y * Mathf.Cos(angle) + v.x * Mathf.Sin(angle);
        return new Vector2(x, y);
    }

    private Vector2 turnGrad(Vector2 v, float angle)
    {
        return turnRad(v, angle / 180.0f * Mathf.PI);
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

    /*
     * private int GetRandomAngle(int leftAngle, int rightAngle)
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
    }*/

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

    /*
     * private float[] GetProbablies(int leftAngle, int rightAngle)
    {
        float[] probs = new float[rightAngle - leftAngle + 1];
        Vector2 start = transform.position;

        for (int i = leftAngle; i <= rightAngle; ++i)
        {
            float ang = i / 180.0f * Mathf.PI;
            Vector2 end = start + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
            int j = i - leftAngle;
            probs[j] = FindMinDist(start, end);
        }

        float sm = 0;
        int n = rightAngle - leftAngle + 1;
        for (int i = 0; i < n; ++i)
            if (probs[i] > 0)
                sm += probs[i];

        for (int i = 0; i < n; ++i)
            if (Mathf.Abs(probs[i]) > EPS)
                probs[i] = probs[i] / sm;
            else
                probs[i] = 0;
        return probs;
    }*/
}
