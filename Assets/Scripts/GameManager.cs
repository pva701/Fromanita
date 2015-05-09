using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public GameObject amanita;
    public GameObject frog;
    public float gameOverDelay = 0.7f;

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
        //TODO write
    }

	void Update () {
	
	}

    public void GameOver()
    {
        AmanitaController amanitaController = (AmanitaController)amanita.GetComponent<AmanitaController>();
        FrogController frogController = (FrogController)frog.GetComponent<FrogController>();
        amanitaController.StopMoving();
        frogController.DieAnimation();
        Invoke("ExitToMainMenu", gameOverDelay);
    }

    public void ExitToMainMenu()
    {
        //TODO write this
    }

}
