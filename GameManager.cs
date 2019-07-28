using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GridInstantiator GI;

	public static int[,] gridsSaved = new int[4, 81];
	public static int[] grid0, grid1, grid2, grid3;
	public static int lvl;
	public static int mistakes;
	public static int mistakes0;
	public static int mistakes1;
	public static int mistakes2;
	public static int mistakes3;
	public static int numOfHints;
	public static int totalWins;
	public static int easyWins;
	public static int mediumWins;
	public static int hardWins;
	public static int expertWins;
	public static bool timerEnabled = false;
	public static string songName = "happyMusic";

	public static bool margins = false;

	public static bool notes = false;

	void Awake()
	{
		//Check if there is already an instance of SoundManager
	  if (instance == null)
		{
			//if not, set it to this.
			instance = this;
		}
	  //If instance already exists:
	  else if (instance != this)
		{
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
		}
	  //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
	  DontDestroyOnLoad (gameObject);

	}

	void Start()
	{
		if (PlayerPrefs.HasKey("music"))
		{
			songName = PlayerPrefs.GetString("music");
		}
		if (PlayerPrefs.HasKey("timerEnabled") && PlayerPrefs.GetInt("timerEnabled") == 0)
		{
			timerEnabled = false;
		}
		else
		{
			timerEnabled = true;
			PlayerPrefs.SetInt("timerEnabled", 1);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			SaveManager.ClearAllWins();
			Debug.Log("Wins cleared");
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			SaveManager.ClearWinTimes();
			Debug.Log("Win times cleared");
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			SaveManager.ClearAllGrids();
			Debug.Log("Grids cleared");
		}
	}

	public static void SaveGridArray(int[] gridOriginal, int saveSlot)
	{
		for (int i = 0; i < 81; i++)
		{
			gridsSaved[saveSlot, i] = gridOriginal[i];
		}
		Debug.Log("Saved grid number " + saveSlot + " to Game Manager");
		int[] tempGrid = new int[81];
		for (int i = 0; i < 81; i++)
		{
			tempGrid[i] = gridsSaved[lvl, i];
		}
		SaveManager.SaveGrid(tempGrid, lvl);
	}

	int[] CopyGridArray(int[] gridCopy)
	{
		int[] gridHolder = new int[81];
		for (int i = 0; i < 81; i++)
		{
			gridHolder[i] = gridCopy[i];
		}
		return gridHolder;
	}

	public static void LoadGridArray()
	{
		lvl = ReturnLvlByScene();
		if (SaveManager.LoadGrid(lvl) != null)
		{
			switch (lvl)
			{
				case 0:
					grid0 = SaveManager.LoadGrid(lvl);
					break;
				case 1:
					grid1 = SaveManager.LoadGrid(lvl);
					break;
				case 2:
					grid2 = SaveManager.LoadGrid(lvl);
					break;
				case 3:
					grid3 = SaveManager.LoadGrid(lvl);
					break;
				default:
					break;
			}
		}
	}

	// public static string ReturnSongName()
	// {
	//
	// }
	public static int[] ReturnGrid()
	{
		int i = ReturnLvlByScene();
		switch (i)
		{
			case 0:
			return grid0;
			case 1:
			return grid1;
			case 2:
			return grid2;
			case 3:
			return grid3;
			default:
			return null;
		}
	}

	public static string ReturnSceneNameByDifficulty(int dif)
	{
		switch (dif)
		{
			// return scene name eg "GridEasy";
			case 0:
				return "GridEasy";
			case 1:
				return "GridMedium";
			case 2:
				return "GridHard";
			case 3:
				return "GridExpert";
			default:
				return "";
		}
		// return "GridEasy";
	}

	public static int ReturnLvlByScene()
	{
		// return which scene player is looking at, so they can
		// start whichever scripts necessary
		Scene scene = SceneManager.GetActiveScene();
		switch (scene.name)
		{
			case "GridEasy":
				lvl = 0;
				return 0;
			case "GridMedium":
				lvl = 1;
				return 1;
			case "GridHard":
				lvl = 2;
				return 2;
			case "GridExpert":
				lvl = 3;
				return 3;
			default:
				return 4;
		}
	}

}
