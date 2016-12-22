using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class SceneGeneratorScript : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject yPlane;
	public GameObject xPlane;
	public GameObject zPlane;
	public GameObject yPlane2;
	public GameObject xPlane2;
	public GameObject zPlane2;
	public GameObject button0;
	public GameObject button1;
	public GameObject button2;
	public GameObject m_Camera;
	public Button pauseplay;
	public Sprite playImg;
	public Sprite pauseImg;
	private GyroLook gyro;
	public Canvas canvas;
	public bool paused = true;
	public List<GameObject> tiles = new List<GameObject>();
	public List<GameObject> allTiles = new List<GameObject> ();
	public static float canvasDistance;
	public int gameMode = 0;  // 0 = race, 1 = quick fill, 2 = paint
	public float delay = 1f;
	private float time = 0;
	private float elapsedTime = 0f;
	public GameObject crosshair;
	public Image crosshair1;
	public Image crosshair2;


	// Use this for initialization
	void Start () {
		//Instantiate (floorPlane, new Vector3(0.5f+1f, 0.5f, 0.5f), floorPlane.transform.rotation);
		//generateRoom();
		gyro = m_Camera.GetComponent<GyroLook> ();
	}

	public void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

	public void restartGame(int newMode) {
		while (allTiles.Count > 0) {
			GameObject tileObject = allTiles [0];
			allTiles.RemoveAt (0);
			if (tiles.Count > 0)
				tiles.RemoveAt (0);
			Destroy (tileObject);
		}
		if (gameMode == 2 && newMode != 2) {
			crosshair1.color = Color.clear;
			crosshair2.color = Color.clear;
		}
		int chance = Random.Range (0, 100);
		if (chance < 33)
			ShowAd();

	}

	public void generateRoom () {
		int t = (int)(Random.Range (0, 3));
		float col = Random.Range (0, 0.6f);
		for (float i = 0; i < 8; i+=1) {
			for (float j = 0; j < 8; j+=1) {
				GameObject Z = (GameObject)Instantiate (tilePrefab, new Vector3 (0.5f + i, 0.5f + j, 0), zPlane.transform.rotation);
				TileScript zScript = Z.GetComponent<TileScript> ();
				switch (t) {
				case 0:
					zScript.setColor (i / 8, j / 8, col);
					break;
				case 1:
					zScript.setColor (i / 8, col, j / 8);
					break;
				case 2:
					zScript.setColor (col, i / 8, j / 8);
					break;
				}
				if (gameMode == 2) {
					if (j >= 0) {
						GameObject Y = (GameObject)Instantiate (tilePrefab, new Vector3 (0.5f + i, 0, 0.5f + j), yPlane.transform.rotation);
						TileScript yScript = Y.GetComponent<TileScript> ();
						yScript.makePallete((int) i);
					}
					GameObject X = (GameObject)Instantiate (tilePrefab, new Vector3 (0, 0.5f + i, 0.5f + j), xPlane.transform.rotation);
					TileScript xScript = X.GetComponent<TileScript> ();
					xScript.setColor (.5f, 0.5f, 0.5f);
					GameObject Y2 = (GameObject)Instantiate (tilePrefab, new Vector3 (0.5f + i, 8, 0.5f + j), yPlane2.transform.rotation);
					GameObject X2 = (GameObject)Instantiate (tilePrefab, new Vector3 (8, 0.5f + i, 0.5f + j), xPlane2.transform.rotation);
					GameObject Z2 = (GameObject)Instantiate (tilePrefab, new Vector3 (0.5f + i, 0.5f + j, 8), zPlane2.transform.rotation);
					TileScript yScript2 = Y2.GetComponent<TileScript> ();
					TileScript xScript2 = X2.GetComponent<TileScript> ();
					TileScript zScript2 = Z2.GetComponent<TileScript> ();
					yScript2.setColor (.5f, 0.5f, 0.5f);
					xScript2.setColor (.5f, 0.5f, 0.5f);
					zScript2.setColor (.5f, 0.5f, 0.5f);
				}
			}
		}
	}

	//Flip random tile
	void randomFlip () {
		if (tiles.Count > 0) {
			int i = Random.Range (0, tiles.Count);
			TileScript tile = tiles [i].GetComponent<TileScript> ();
			tile.setTarget ();
		}

	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime; //used to keep track of the time passed since start of current game
		elapsedTime += Time.deltaTime; //elapsed time from last tile flip
		//  For VR depth of crosshair
		//canvas.planeDistance = canvasDistance;
		if (gameMode == 0) {
			if (tiles.Count == 32) {
				//game over!
			}
			else if (elapsedTime > delay) {
				randomFlip ();
				if (delay > 1)
					delay -= 0.09f;
				elapsedTime = 0f;
			}

		}
		// for VR escape key to access menu
		/*if (Input.GetKeyDown (KeyCode.Escape)) {
			if (button0.activeSelf) {
				paused = false;
				pauseplay.image.sprite = pauseImg;
				gyro.startLooking ();
				button0.SetActive (false);
				button1.SetActive (false);
				button2.SetActive (false);
			} else if (!paused) {
				paused = true;
				pauseplay.image.sprite = playImg;
				gyro.menuView ();
				button0.SetActive (true);
				button1.SetActive (true);
				button2.SetActive (true);
			}
		}*/
		RaycastHit seen;

		//	Check if pause/play button was hit
		Vector2 touchSpot = new Vector2 (0, 0);
		bool touchedNow = false;
		float butX = pauseplay.transform.position.x;
		float butY = pauseplay.transform.position.y;
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began) {
				// Construct a ray from the current touch coordinates
				touchSpot = Input.GetTouch (i).position;
				touchedNow = true;
			}
		}
		//  Actual check mentioned above
		if (touchedNow && touchSpot.x > butX - 40 && touchSpot.y > butY - 40) {
			if (button0.activeSelf) {
				paused = false;
				pauseplay.image.sprite = pauseImg;
				gyro.startLooking ();
				crosshair.SetActive (true);
				button0.SetActive (false);
				button1.SetActive (false);
				button2.SetActive (false);
			} else if (!paused) {
				paused = true;
				pauseplay.image.sprite = playImg;
				//gyro.menuView ();
				crosshair.SetActive(false);
				button0.SetActive (true);
				button1.SetActive (true);
				button2.SetActive (true);
			}
		}
		// 	Pause/play button not clicked, check if button or tile was clicked
		else {
			//  buttons active so not paying attention to tiles
			if (button0.activeSelf) {
				for (int i = 0; i < Input.touchCount; ++i) {
					if (Input.GetTouch (i).phase == TouchPhase.Began) {
						// Construct a ray from the current touch coordinates
						Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (i).position);
						if (Physics.Raycast (ray, out seen, 100)) {
							if (seen.collider.gameObject.tag == "button") {
								buttonScript button = seen.collider.gameObject.GetComponent<buttonScript> ();
								button.startGame ();
			
								crosshair.SetActive (true);
								gyro.startLooking ();
								pauseplay.gameObject.SetActive (true);
								paused = false;
								pauseplay.image.sprite = pauseImg;
							}
						}
					}
				}
			
			} 
			else {
				//  button not active so we only worry about tiles
				Ray raydirection = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
				bool hit = Input.GetMouseButtonDown (0) || Input.GetKey (KeyCode.Joystick1Button0)
				         || Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (1);
				if (Physics.Raycast (raydirection, out seen, 100)) {				
					if (hit) {
						// This if block wouldn't run because button isn't active
						/*
						if (seen.collider.gameObject.tag == "button") {
							pauseplay.image.sprite = pauseImg;
							buttonScript button = seen.collider.gameObject.GetComponent<buttonScript> ();
							button.startGame ();
							gyro.startLooking ();
							paused = false;
						}
						*/
						if (paused) {
							Debug.LogError ("paused shouldn't be true here ever!!!");
						}
						if (!paused && seen.collider.gameObject.tag == "tile") {
							TileScript seenTile = seen.collider.gameObject.GetComponent<TileScript> ();
							// For VR right click to change color
							/*if (Input.GetMouseButtonDown (1))
								seenTile.changeColor ();
							else*/
							seenTile.resetColor ();
						}
					}
					//  For VR depth of crosshair
					//SceneGeneratorScript.canvasDistance = seen.distance - 0.4f;
				}
			}
		}
	}
}
