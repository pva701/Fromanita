using UnityEngine;
using System.Collections;

/*
 * Singleton class for detecting touches and key downs. 
*/

public class InputManager : MonoBehaviour {
    public static InputManager instance = null;

    public bool isLeft; //{ get; }
    public bool isRight; //{ get; }
    public int dir;// {get;}

	void Awake () {
        if (instance == null)
            instance = this;
        else
            DestroyObject(gameObject);
        DontDestroyOnLoad(gameObject);
	}

	void Update () {
        dir = 0;
        isLeft = false;
        isRight = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        dir = (int)Input.GetAxis("Horizontal");
#else
        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];
            dir = (touch.position.x > Screen.width / 2 ? 1 : -1);
        }
#endif
        if (dir < 0) isLeft = true;
        else if (dir > 0) isRight = true;
	}
}
