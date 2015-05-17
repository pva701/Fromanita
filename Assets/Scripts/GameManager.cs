using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float gameOverDelay = 1.0f;

    public GameObject amanita;
    public GameObject frog;
    public GameObject ground;
    public GameObject prefabMosquito;
    public GameObject walls;


    private float generateNextMosquitoDelay = 10.0f;
    private float timeLiveMosquito = 10.0f;
    private bool isDie = false;
    private Vector3 frogPosition;
    private Vector3 amanitaPosition;
    private GameObject frogStore;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        frogPosition = frog.transform.position;
        amanitaPosition = amanita.transform.position;
        StartCoroutine(StartMosquito());
    }

    void Update()
    {
        if (isDie && InputManager.instance.dirHoriz != 0)
        {
            //TODO change it
            frog.transform.position = frogPosition;
            amanita.transform.position = amanitaPosition;
            frog.GetComponent<FrogController>().enabled = true;
            amanita.GetComponent<AmanitaController>().enabled = true;
            isDie = false;
        }
    }

    public void GameOver()
    {
        AmanitaController amanitaController = (AmanitaController)amanita.GetComponent<AmanitaController>();
        FrogController frogController = (FrogController)frog.GetComponent<FrogController>();
        amanitaController.StopMoving();
        frogController.Die();
        isDie = true;
    }

    private IEnumerator StartMosquito()
    {
        for (; ; )
        {
            //MosquitoController controller = new MosquitoController();
            //MosquitoController controller = prefabMosquito.GetComponent<MosquitoController>();
            GameObject instance = Instantiate(prefabMosquito) as GameObject;
            MosquitoController controller = instance.GetComponent<MosquitoController>();
            controller.StartMoving(timeLiveMosquito);
            yield return new WaitForSeconds(generateNextMosquitoDelay);
        }
    }

    public Rect GetFieldRect()
    {
        EdgeCollider2D groundCollider = ground.GetComponent<EdgeCollider2D>();
        EdgeCollider2D[] colliders = walls.GetComponents<EdgeCollider2D>();
        float yMin = groundCollider.points[0].y + ground.transform.position.y - groundCollider.offset.y;
        float yMax = groundCollider.points[0].y + ground.transform.position.y - groundCollider.offset.y;
        float xMin = Mathf.Min(groundCollider.points[0].x, groundCollider.points[1].x) + ground.transform.position.x - groundCollider.offset.x;
        float xMax = Mathf.Max(groundCollider.points[0].x, groundCollider.points[1].x) + ground.transform.position.x - groundCollider.offset.x;
        for (int i = 0; i < colliders.Length; ++i)
            for (int j = 0; j < colliders[i].points.Length; ++j) {
                Vector2 pnt = colliders[i].points[j];
                yMax = Mathf.Max(yMax, pnt.y + walls.transform.position.y - colliders[i].offset.y);
            }
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}
