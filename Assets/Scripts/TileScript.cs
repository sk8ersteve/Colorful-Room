using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileScript : MonoBehaviour {

	private float red;
	private float green;
	private float blue;
	public static Color paintColor;
	bool flipped = false;
	public bool special;
	public GameObject gameManager;
	public SceneGeneratorScript managerScript;
	private int gameMode;
	public Image crosshair1;
	public Image crosshair2;
	public int c = 0;
	private bool isPallete;
	public Color[] pallete;

	// Use this for initialization
	void Start () {
		managerScript = gameManager.GetComponent<SceneGeneratorScript> ();
		pallete = new Color[] {Color.black, Color.blue, Color.cyan, Color.green, Color.yellow, Color.red, Color.magenta, Color.gray};
		//Debug.Log (pallete [0]);
	}

	public void setColor(float r, float g, float b) {
		red = r;
		green = g;
		blue = b;
		Renderer ren = GetComponent<Renderer> ();
		ren.material.color = new Color (red, green, blue);
		gameMode = managerScript.gameMode;
		if (gameMode == 0)
			managerScript.tiles.Add (gameObject);
		managerScript.allTiles.Add (gameObject);
		if (gameMode >= 1)
			setTarget ();
	}

	public void makePallete (int i) {
		isPallete = true;
		c = i;
		Renderer ren = GetComponent<Renderer> ();
		ren.material.color = pallete [i];
		paintColor = Color.black;
		crosshair1.color = paintColor;
		crosshair2.color = paintColor;
		managerScript.allTiles.Add (gameObject);
	}

	public void changeColor() {
		paintColor = pallete [c];
		crosshair1.color = paintColor;
		crosshair2.color = paintColor;
		red = paintColor.r;
		green = paintColor.g;
		blue = paintColor.b;
	}
	
	public void setTarget() {
		Renderer ren = GetComponent<Renderer> ();
		float otherColor = 1f;
		ren.material.color = new Color (otherColor, otherColor, otherColor);
		flipped = true;
		if (!managerScript)
		managerScript.tiles.Remove (gameObject);
	}

	public void resetColor () {
		if (isPallete) {
			changeColor ();
		}
		else if (flipped) {
			Renderer ren = GetComponent<Renderer> ();
			ren.material.color = new Color (red, green, blue);
			if (gameMode == 2)
				ren.material.color = paintColor;
			flipped = false;
			managerScript.tiles.Add (gameObject);
		} else
			setTarget ();
	}

	// Update is called once per frame
	void Update () {
		/*if (Random.Range (0, 80000) == 42 && !flipped)
			setTarget ();*/
	}
}
