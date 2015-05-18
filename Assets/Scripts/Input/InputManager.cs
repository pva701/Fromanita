using UnityEngine;
using System.Collections;

/*
 * Singleton class for detecting touches and key downs. 
*/

public class InputManager : MonoBehaviour {
    public static InputManager instance = null;

    public bool isLeft; //{ get; }
    public bool isRight; //{ get; }
    public int dirHoriz;// {get;}
    public bool isDown;
    public int dirVert;

    private bool mobileOutGround = false;
    private Vector2 mobileStartTouch;

	void Awake () {
        if (instance == null)
            instance = this;
        else
            DestroyObject(gameObject);
        DontDestroyOnLoad(gameObject);
	}

	void Update () {
        dirHoriz = 0;
        dirVert = 0;
        isLeft = false;
        isRight = false;
        isDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
//#if UNITY_STANDALONE || UNITY_WEBPLAYER
        dirHoriz = (int)Input.GetAxis("Horizontal");
        dirVert = (int)Input.GetAxis("Vertical");
        if (dirVert > 0) dirVert = 0;
        if (dirVert != 0 && dirHoriz != 0) dirVert = 0;
#else
        
        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                if (worldPos.y > GameManager.instance.GetTopAmanita())
                {
                    mobileOutGround = true;
                    mobileStartTouch = worldPos;
                }
            }

            if (!mobileOutGround) {
                if (worldPos.y <= GameManager.instance.GetTopAmanita()) ;
                    dirHoriz = (touch.position.x > Screen.width / 2 ? 1 : -1);
            } else if (touch.phase == TouchPhase.Ended)
            {
                mobileOutGround = false;
                if (worldPos.y - mobileStartTouch.y < -0.5) dirVert = -1;
            }
        }
#endif
        if (dirHoriz < 0) isLeft = true;
        else if (dirHoriz > 0) isRight = true;
        else if (dirVert < 0) isDown = true;
	}
}
