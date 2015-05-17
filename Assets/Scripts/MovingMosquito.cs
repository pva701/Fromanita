using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MovingMosquito : MonoBehaviour {
    private Rigidbody2D rg2D;
    private float liveTime;
    private Rect boundRect;
    private Vector2 leftDown;
    private Vector2 leftTop;
    private Vector2 rightDown;
    private Vector2 rightTop;
    private float EPS = 0.00001f;
    public float SPEED = 2f;
    private Queue<ITrajectory> queueTrajectory = new Queue<ITrajectory>();

    void Awake() {
        rg2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (queueTrajectory.Count == 0)
            return;
        liveTime -= Time.deltaTime;
        if (transform.position.x < boundRect.xMin || transform.position.x > boundRect.xMax || transform.position.y > boundRect.yMax) {
            //print("destroy");
            Destroy(gameObject);
        } if (liveTime < 0 && liveTime + Time.deltaTime > 0) {
            ITrajectory traj = queueTrajectory.Peek();
            if (traj.HasNext() && traj.NextPoint(Time.deltaTime).x - transform.position.x > 0)
                rg2D.velocity = new Vector2(SPEED, 0);
            else
                rg2D.velocity = new Vector2(-SPEED, 0);
        } else if (queueTrajectory.Count > 0 && liveTime >= 0) {
            if (!queueTrajectory.Peek().HasNext()) queueTrajectory.Dequeue();
            //print("size after = " + queueTrajectory.Count);
            if (queueTrajectory.Count == 1) ContinueTrajectory();
            ITrajectory trajectory = queueTrajectory.Peek();
            Vector2 newPos = trajectory.NextPoint(Time.deltaTime);
            transform.position = newPos;
            //print("pos = " + newPos + " xMax = " + boundRect.xMax);
        }   
    }

    private void ContinueTrajectory()
    {
        ITrajectory trajectory = queueTrajectory.Dequeue();
        Vector2 p1 = trajectory.EndPoint();
        Vector2 p2 = trajectory.StartPoint();
        //Vector2 p3 = RandPointInRect();
        Vector2 p3 = RandPointInSector(p1, p2);
        //print("p1 = " + p1 + " p2 = " + p2 + " p3 = " + p3);

        float maxLen = Mathf.Min((p2 - p1).magnitude, (p3 - p1).magnitude) * 0.5f;
        float len = 0;
        if (maxLen < 1.0f)
            len = maxLen;
        else
            len = Random.Range(1f, maxLen);

        float frac1 = len / (p2 - p1).magnitude;
        float frac2 = len / (p3 - p1).magnitude;
        //print("frac1 = " + frac1 + " frac2 = " + frac2);
        float speed = SPEED;
        Vector2 a = p1 + (p2 - p1) * frac1;
        Vector2 b = p1 + (p3 - p1) * frac2;
        //print("a = " + a + " b = " + b);
        //if (CrossProduct(p2 - p1, p3 - p1) >= 0) speed *= -1;
        Vector2 center;
        IntersectLines(a, a + new Vector2(p2.y - p1.y, -(p2.x - p1.x)), b, b + new Vector2(p3.y - p1.y, -(p3.x - p1.x)), out center);
        float radius = Mathf.Abs(CrossProduct(p2 - p1, center - p1)) / (p2 - p1).magnitude;
        float fromAngle = Mathf.Atan2((a - center).y, (a - center).x);
        float toAngle = Mathf.Atan2((b - center).y, (b - center).x);
        if (fromAngle < 0) fromAngle += 2 * Mathf.PI;
        if (toAngle < 0) toAngle += 2 * Mathf.PI;
        float angle = (fromAngle + toAngle) * 0.5f;
        if (!InTriangle(a, b, p1, center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius)) {
            if (toAngle < fromAngle)
                toAngle += 2 * Mathf.PI;
            else
                toAngle -= 2 * Mathf.PI;
        }
        queueTrajectory.Enqueue(new LineTrajectory(p2, a, SPEED));
        //print("frm to " + fromAngle + " " + toAngle + " " + radius + " cent = " + center);
        queueTrajectory.Enqueue(new CircleTrajectory(fromAngle, toAngle, speed, new Circle(center, radius)));
        queueTrajectory.Enqueue(new LineTrajectory(b, p3, SPEED));
    }

    public void StartMoving(float liveTime)
    {
        this.liveTime = liveTime;
        Rect rect = GameManager.instance.GetFieldRect();
        Random.seed = System.DateTime.Now.Millisecond;
        boundRect = new Rect(rect.xMin, rect.yMin + 2, rect.xMax - rect.xMin, rect.yMax - rect.yMin - 2);
        leftDown = new Vector2(boundRect.xMin, boundRect.yMin);
        leftTop = new Vector2(boundRect.xMin, boundRect.yMax);
        rightDown = new Vector2(boundRect.xMax, boundRect.yMin);
        rightTop = new Vector2(boundRect.xMax, boundRect.yMax);

        //print("a = " + a + " b = " + b + " center = " + center);
        //print("p1 = " + p1 + " p2 = " + p2 + " p3 = " + p3);
        Vector2 p2 = new Vector2(boundRect.xMin, Random.Range(boundRect.yMin, boundRect.yMax));
        Vector2 p1 = RandPointInRect();
        transform.position = p2;
        queueTrajectory.Enqueue(new LineTrajectory(p2, p1, SPEED));
        ContinueTrajectory();
    }

    private Vector2 getBisector(Vector2 a, Vector2 b)
    {
        return (a * b.magnitude + b * a.magnitude) / (2 * a.magnitude * b.magnitude);
    }

    private Circle getInnerCircle(Vector2 a, Vector2 b, Vector2 c)
    {
        Vector2 vecBisector1 = getBisector(b - a, c - a);
        Vector2 vecBisector2 = getBisector(a - b, c - b);
        Vector2 center;
        IntersectLines(a, a + vecBisector1, b, b + vecBisector2, out center);
        return new Circle(center, Mathf.Abs(CrossProduct(center - a, b - a)) / (b - a).magnitude);
    }

    private Vector2 RandPointInRect()
    {
        float x = Random.Range(boundRect.xMin, boundRect.xMax);
        float y = Random.Range(boundRect.yMin, boundRect.yMax);
        return new Vector2(x, y);
    }

    private bool InTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
    {
        float p1 = CrossProduct(b - a, p - a);
        float p2 = CrossProduct(c - b, p - b);
        float p3 = CrossProduct(a - c, p - c);
        return p1 >= 0 && p2 >= 0 && p3 >= 0 || p1 <= 0 && p2 <= 0 && p3 <= 0;
    }

    public Vector2 RandPointInSector(Vector2 a, Vector2 b) {
        float MIN_ANGLE = Mathf.PI / 4;
        float angle = Mathf.Atan2((b - a).y, (b - a).x);
        if (angle < 0) angle += 2 * Mathf.PI;
        float angleRes = 0;
        float minDist = 0;
        int it = 0;
        do {
            ++it;
            if (it > 10) break;
            angleRes = Random.Range(angle + MIN_ANGLE, angle - MIN_ANGLE + 2 * Mathf.PI);
            minDist = FindMinDist(a, a + new Vector2(Mathf.Cos(angleRes), Mathf.Sin(angleRes)));
            //print("md = " + minDist);
        } while (minDist < 0.3f);

        if (Mathf.Abs(minDist) < EPS)
            Destroy(gameObject);
        //print("minDist = " + minDist);
        float radius = 0;
        float bound = 2f;
        if (minDist > bound)
            radius = Random.Range(bound, Mathf.Min(3f, minDist));
        else
            radius = minDist;
        //print("angleRes = " + angleRes + " rad = " + radius);
        return a + new Vector2(Mathf.Cos(angleRes), Mathf.Sin(angleRes)) * radius;
    }

    private Rect IntersectRects(Rect a, Rect b)
    {
        float x = Mathf.Max(a.xMin, b.xMin);
        float y = Mathf.Max(a.yMin, b.yMin);
        return new Rect(x, y, Mathf.Min(a.xMax, b.xMax) - x, Mathf.Min(a.yMax, b.yMax) - y);
    }

    private float FindMinDist(Vector2 start, Vector2 end)
    {
        Vector2 ans;
        float dist = 1000000f;
        if (IntersectSegmentAndRay(start, end, leftTop, leftDown, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (IntersectSegmentAndRay(start, end, leftDown, rightDown, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (IntersectSegmentAndRay(start, end, rightDown, rightTop, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        if (IntersectSegmentAndRay(start, end, rightTop, leftTop, out ans)) dist = Mathf.Min(Vector2.Distance(ans, start), dist);
        
        if (dist > 1e3)
        {
            dist = 0;
            print("PIZDEC " + start + " " + (end - start));
        }
        return dist;
    }

    private bool IntersectLines(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 ans)//я в ахуе
    {
        Vector2 d1 = b - a;
        Vector2 d2 = d - c;
        float cp = CrossProduct(d1, d2);
        ans = new Vector2();
        if (Mathf.Abs(cp) < EPS)
            return false;
        float t1 = CrossProduct(d2, a - c) / cp;
        ans = a + d1 * t1;
        return true;
    }

    private bool IntersectSegmentAndRay(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 ans)//я в ахуе
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
        if (t1 >= 0 && t2 >= 0 && t2 <= 1)
        {
            ans = a + d1 * t1;
            return true;
        }
        return false;
    }

    private float CrossProduct(Vector2 a, Vector2 b) {
        return a.x * b.y - a.y * b.x;
    }

    public abstract class ITrajectory
    {
        public abstract bool HasNext();
        public abstract Vector2 StartPoint();
        public abstract Vector2 EndPoint();
        public abstract Vector2 NextPoint(float delta);
    }

    public class Circle
    {
        public Vector2 center;
        public float radius;
        public Circle(Vector2 c, float r)
        {
            center = c;
            radius = r;
        }
    }

    public class CircleTrajectory : ITrajectory
    {
        private float fromAngle;
        private float toAngle;
        private float angleSpeed;
        private float currentAngle;
        private Circle circle;

        public CircleTrajectory(float fromAngle, float toAngle, float speed, Circle circle)
        {
            this.circle = circle;
            this.fromAngle = fromAngle;
            this.toAngle = toAngle;
            currentAngle = fromAngle;
            this.circle = circle;
            float t = Mathf.Abs(toAngle - fromAngle) * circle.radius / speed;
            this.angleSpeed = (toAngle - fromAngle) / t;
        }

        public override bool HasNext()
        {
            return Mathf.Abs(currentAngle - toAngle) > 0.001;
        }

        public override Vector2 NextPoint(float delta)
        {
            if (angleSpeed > 0)
                currentAngle = Mathf.Min(toAngle, currentAngle + angleSpeed * delta);
            else
                currentAngle = Mathf.Max(toAngle, currentAngle + angleSpeed * delta);
            return getPointOnCircle(currentAngle);
        }

        private Vector2 getPointOnCircle(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circle.radius + circle.center;
        }

        public override Vector2 StartPoint()
        {
            return getPointOnCircle(fromAngle);
        }

        public override Vector2 EndPoint()
        {
            return getPointOnCircle(toAngle);
        }
    }

    public class LineTrajectory : ITrajectory
    {
        private Vector2 start;
        private Vector2 end;
        private float speed;
        private Vector2 currentPoint;

        public LineTrajectory(Vector2 a, Vector2 b, float speed)
        {
            this.start = a;
            this.end = b;
            this.speed = speed;
            this.currentPoint = a;
        }

        public override bool HasNext()
        {
            return (end - currentPoint).magnitude > 0.001;
        }

        public override Vector2 NextPoint(float delta)
        {
            currentPoint = Vector2.MoveTowards(currentPoint, end, delta * speed);
            return currentPoint;
        }

        public override Vector2 StartPoint()
        {
            return start;
        }

        public override Vector2 EndPoint()
        {
            return end;
        }
    }

}
