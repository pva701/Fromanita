using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
	public static BackgroundManager instance = null;
	public GameObject[] cloudTiles;
	public int perCount = 100;
	public int randomRange = 10;
	public Rect spawnCloudRect = new Rect (-15f, 5f, 5f, 9f);
	private int counter = 0;
	public int cloudCount = 0;

	// Use this for initialization
	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (counter++ == perCount) {
			counter = 0;
			if (Random.Range(0, 1 << cloudCount) == 0) {
				float x = Random.Range(spawnCloudRect.xMin, spawnCloudRect.xMax);
				float y = Random.Range(spawnCloudRect.yMin, spawnCloudRect.yMax);
				Vector2 position = new Vector2(x, y);
				GameObject cloud = cloudTiles[Random.Range(0, cloudTiles.Length)];
				Instantiate(cloud, position, Quaternion.identity);
			} 	
		}
	}
}
