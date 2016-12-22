using UnityEngine;
using System.Collections;

public class buttonScript : MonoBehaviour {

	public int gameMode;
	public GameObject gameManager;
	public GameObject otherButton1;
	public GameObject otherButton2;
	public SceneGeneratorScript scene;
	public static bool started = false;

	// Use this for initialization
	void Start () {
		scene = gameManager.GetComponent<SceneGeneratorScript> ();
	}

	public void startGame () {
		if (started) {
			scene.restartGame (gameMode);
		}
		started = true;
		scene.gameMode = gameMode;
		scene.generateRoom ();
		otherButton1.SetActive (false);
		otherButton2.SetActive (false);
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
