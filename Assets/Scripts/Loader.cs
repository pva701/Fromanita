using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
    public GameObject inputManager;
    public GameObject gameManager;

	void Awake() {
        print("awake loader");
        if (InputManager.instance == null)
            Instantiate(inputManager);
        if (GameManager.instance == null)
            Instantiate(gameManager);
	}
}
