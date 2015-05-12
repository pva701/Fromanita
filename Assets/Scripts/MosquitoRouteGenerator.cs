using UnityEngine;
using System.Collections;

public class MosquitoRouteGenerator {

    private float y;
    private Vector2 startPoint;
    private Vector2 currentPoint;
    private Vector2 step;
    private int totalIterations;
    private int curIteration = 0;

    public MosquitoRouteGenerator(float moveTime, float liveTime)
    {
        y = Random.Range(3.0f, 7.5f);//TODO change
        startPoint = new Vector2(-2.34f, y);
        currentPoint = startPoint;
        int iters = (int)(liveTime / moveTime);
        totalIterations = iters;
        step = new Vector2(5.64f + 2.34f, 0f) / iters;
    }

    public Vector2 StartPoint()
    {
        return startPoint;
    }

    public Vector2 NextPoint() //return next point of rout or null if mosquito must die
    {
        ++curIteration;
        currentPoint += step;
        return currentPoint;
    }

    public bool HasNextPoint()
    {
        return curIteration < totalIterations;
    }
}
