using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public float gameOverDelay = 0.7f;
    public GameObject amanita;
    public GameObject frog;
    public GameObject ground;

	void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        GenerateField();
	}

    void GenerateField()
    {
        //EdgeCollider2D groundEdge = ground.AddComponent<EdgeCollider2D>();
        //Vector2[] points = new Vector2[]{new Vector2(1, 1)};
        //groundEdge.points
    }

	void Update () {
	
	}

    public void GameOver()
    {
        AmanitaController amanitaController = (AmanitaController)amanita.GetComponent<AmanitaController>();
        FrogController frogController = (FrogController)frog.GetComponent<FrogController>();
        amanitaController.StopMoving();
        frogController.Die();
        Invoke("ExitToMainMenu", gameOverDelay);
    }

    public void ExitToMainMenu()
    {
        //TODO write this
    }

}
