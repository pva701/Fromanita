using UnityEngine;
using System.Collections;

public class MosquitoController : MonoBehaviour {

    public float moveTime = 0.1f;
    public MosquitoRouteGenerator routeGenerator;

    private Rigidbody2D rg2D;
	void Awake() {
        rg2D = GetComponent<Rigidbody2D>();
	}

    public void StartMoving()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        for (; ; )
        {
            if (routeGenerator == null)
                print("rout is null");
            print("move mosq = " + routeGenerator);
            Vector2 point = routeGenerator.NextPoint();
            if (!routeGenerator.HasNextPoint())
                DestroyObject(gameObject);
            else
                rg2D.MovePosition(point);
            yield return new WaitForSeconds(moveTime);
        }
    }
}
