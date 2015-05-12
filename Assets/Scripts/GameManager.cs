using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float gameOverDelay = 1.0f;
    public float generateNextMosquitoDelay = 10.0f;
    public float timeLiveMosquito = 3.0f;

    public GameObject amanita;
    public GameObject frog;
    public GameObject ground;
    public GameObject prefabMosquito;

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
        if (isDie && InputManager.instance.dir != 0)
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

    IEnumerator StartMosquito()
    {
        for (; ; )
        {
            //MosquitoController controller = new MosquitoController();
            //MosquitoController controller = prefabMosquito.GetComponent<MosquitoController>();
            GameObject instance = Instantiate(prefabMosquito) as GameObject;
            MosquitoController controller = instance.GetComponent<MosquitoController>();
            controller.liveTime = timeLiveMosquito;
            controller.StartMoving();
            yield return new WaitForSeconds(generateNextMosquitoDelay);
        }
    }

}
