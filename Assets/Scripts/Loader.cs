using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
    public GameObject inputManager;

	void Awake() {
        if (InputManager.instance == null)
            Instantiate(inputManager);
	}
}
