// ﻿using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class SaveManager
{

  public static string ReturnDifficultyString(int dif)
  {
    switch (dif)
    {
      case 0:
        return "easy";
      case 1:
        return "medium";
      case 2:
        return "hard";
      case 3:
        return "expert";
      default:
        return null;
    }
  }

  public static bool CheckIfGridIsSaved(int saveSlot)
  {
    // gridExists is int pref, 0 is it doesn't exists
    // 1 is it does;
    switch (saveSlot)
    {
      case 0:
        if (PlayerPrefs.HasKey("easyExists") && PlayerPrefs.GetInt("easyExists") == 0)
        {
          return false;
        }
        else
        {
          return true;
        }
      case 1:
        if (PlayerPrefs.HasKey("mediumExists") && PlayerPrefs.GetInt("mediumExists") == 0)
        {
          return false;
        }
        else
        {
          return true;
        }
      case 2:
        if (PlayerPrefs.HasKey("hardExists") && PlayerPrefs.GetInt("hardExists") == 0)
        {
          return false;
        }
        else
        {
          return true;
        }
      case 3:
        if (PlayerPrefs.HasKey("expertExists") && PlayerPrefs.GetInt("expertExists") == 0)
        {
          return false;
        }
        else
        {
          return true;
        }
      default:
        return false;
    }
  }

  public static void SaveTimer(float seconds, int saveSlot)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/time" + saveSlot.ToString() + ".bin";
    FileStream stream = new FileStream(path, FileMode.Create);
    formatter.Serialize(stream, seconds);
    stream.Close();
  }

  public static int LoadTimer(int saveSlot)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/time" + saveSlot.ToString() + ".bin";
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      float t = (float)formatter.Deserialize(stream);
      int time = (int)Mathf.Floor(t);
      stream.Close();
      return time;
    }
    else
    {
      return 0;
    }
  }

  public static void SaveNumsFilled(bool[] numsCorrect, int saveSlot)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/numsFilled" + saveSlot.ToString() + ".bin";
    FileStream stream = new FileStream(path, FileMode.Create);

    formatter.Serialize(stream, numsCorrect);
    stream.Close();

  }

  public static bool[] LoadNumsFilled(int saveSlot)
  {
    string path = Application.persistentDataPath + "/numsFilled" + saveSlot.ToString() + ".bin";
    if (File.Exists(path))
    {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);
      bool[] gridNumsFilled = (bool[])formatter.Deserialize(stream);
      stream.Close();
      // Debug.Log("Loaded nums correct in save slot: " + saveSlot.ToString());
      return gridNumsFilled;
    }
    else
    {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream1 = new FileStream(path, FileMode.Create);
      bool[] gridNumsFilled = GridFI.RevealSpotsFilled(saveSlot);
      Debugger.LogBoolGrid(gridNumsFilled, "Saved numbers filled:");
      formatter.Serialize(stream1, gridNumsFilled);
      stream1.Close();
      return gridNumsFilled;
    }
  }

  public static void SaveGrid(int[] grid, int saveSlot)
  {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/grid" + saveSlot.ToString() + ".bin";
    FileStream stream1 = new FileStream(path, FileMode.Create);

    formatter.Serialize(stream1, grid);
    Debugger.LogGrid(grid, "Saved grid: ");
    stream1.Close();
    PlayerPrefs.SetInt(ReturnDifficultyString(saveSlot) + "Exists", 1);
  }

  public static int[] LoadGrid(int saveSlot)
  {
    string path = Application.persistentDataPath + "/grid" + saveSlot.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int[] grid = (int[])formatter.Deserialize(stream);
      stream.Close();
      return grid;
    }
    else
    {
      Debug.Log("Save file does not exist: must create grid for save slot \"" + saveSlot.ToString() + "\"");
      return null;
    }
  }

  public static void ClearAllGrids()
  {
    string[] paths = new string[4];
    string[] grnPaths = new string[4];
    string path = Application.persistentDataPath;
    for (int i = 0; i < paths.Length; i++)
    {
      paths[i] = path + "/grid" + i.ToString() + ".bin";
      File.Delete(paths[i]);
      grnPaths[i] = path + "/numsFilled" + i.ToString() + ".bin";
      File.Delete(grnPaths[i]);
      PlayerPrefs.SetInt("mistakes" + i.ToString(), 0);
    }
  }

  public static void ClearGrid(int saveSlot)
  {
    string path = Application.persistentDataPath + "/grid" + saveSlot.ToString() + ".bin";
    File.Delete(path);
    path = Application.persistentDataPath + "/numsFilled" + saveSlot.ToString() + ".bin";
    File.Delete(path);
    PlayerPrefs.SetInt("mistakes" + saveSlot.ToString(), 0);
    PlayerPrefs.SetInt(ReturnDifficultyString(saveSlot) + "Exists", 0);
  }

  public static void SaveTotalWins()
  {
    string path = Application.persistentDataPath + "/totalWins.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int wins = (int)formatter.Deserialize(stream);
      wins += 1;
      stream.Close();
      FileStream stream1 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream1, wins);
      stream1.Close();
      Debug.Log("Saved +1 to totalWins file. New value is: " + wins.ToString());
    }
    else
    {
      FileStream stream2 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream2, 1);
      stream2.Close();
      Debug.Log("Created new totalWins file");
    }
  }

  public static string LoadTotalWins()
  {
    string path = Application.persistentDataPath + "/totalWins.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int wins = (int)formatter.Deserialize(stream);
      stream.Close();
      Debug.Log("totalWins file exists, and was loaded. Value is: " + wins.ToString());
      return wins.ToString();
    }
    else
    {
      Debug.Log("totalWins file does not exist. Returned \"0\"");
      return "0";
    }
  }

  public static void SavePlayWins(int dif)
  {
    string path = Application.persistentDataPath + "/winsOnGrid" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int? wins = (int)formatter.Deserialize(stream);
      if (wins == null)
      {
        Debug.Log("Recognized wins are at 0");
        wins = 0;
      }
      wins += 1;
      stream.Close();
      FileStream stream1 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream1, wins);
      Debug.Log(wins.ToString() + " times have you won difficulty: " + dif.ToString());
      stream1.Close();
    }
    else
    {
      int wins = 1;
      FileStream stream2 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream2, wins);
      Debug.Log(wins.ToString() + " times have you won difficulty: " + dif.ToString());
      stream2.Close();
    }
  }

  public static string LoadPlayWins(int dif)
  {
    string path = Application.persistentDataPath + "/winsOnGrid" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int winsOnGrid = (int)formatter.Deserialize(stream);
      stream.Close();
      return winsOnGrid.ToString();
    }
    else
    {
      return "0";
    }
  }

  public static void SaveWinTime(int dif, int hours, int minutes, int seconds)
  {
    string path = Application.persistentDataPath + "/winTimeOnGrid" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(path, FileMode.Create);
    int[] time = new int[3];
    time[0] = hours;
    time[1] = minutes;
    time[2] = seconds;
    formatter.Serialize(stream, time);
    stream.Close();
  }

  public static string LoadWinTime(int dif)
  {
    string path = Application.persistentDataPath + "/winTimeOnGrid" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      int[] time = (int[])formatter.Deserialize(stream);
      int hours = time[0];
      int minutes = time[1];
      int seconds = time[2];
      string timeString = TextManager.ReturnTimeText(hours, minutes, seconds);
      stream.Close();
      return timeString;
    }
    else
    {
      return "N/A";
    }
  }

  public static void ClearWinTimes()
  {
    string path = Application.persistentDataPath + "/fastestWinTime.bin";
    File.Delete(path);
    for (int dif = 0; dif < 4; dif++)
    {
      string path2 = Application.persistentDataPath + "/winTimeOnGrid" + dif.ToString() + ".bin";
      File.Delete(path2);
    }
  }

  public static float LoadFastestWinTime()
  {
    string path = Application.persistentDataPath + "/fastestWinTime.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      float time = (float)formatter.Deserialize(stream);
      stream.Close();
      return time;
    }
    else
    {
      return -1;
    }
  }


  public static bool SaveFastestWinTime(int dif, float time)
  {
    string path = Application.persistentDataPath + "/fastestWinTime.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      float winTime = LoadFastestWinTime();
      if (winTime < time)
      {
        return false;
      }
      else
      {
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, time);
        stream.Close();
        return true;
      }
    }
    else
    {
      FileStream stream1 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream1, time);
      stream1.Close();
      return true;
    }
  }

  public static void SaveFastestWinDifficulty(int dif)
  {
    string path = Application.persistentDataPath + "/fastestWinDifficulty.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(path, FileMode.Create);
    string difficulty;
    switch (dif)
    {
      case 0:
        difficulty = "easy";
        break;
      case 1:
        difficulty = "medium";
        break;
      case 2:
        difficulty = "hard";
        break;
      case 3:
        difficulty = "expert";
        break;
      default:
        difficulty = "";
        stream.Close();
        return;
    }
    formatter.Serialize(stream, difficulty);
    stream.Close();
  }

  public static string LoadFastestWinDifficulty()
  {
    string path = Application.persistentDataPath + "/fastestWinDifficulty.bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      string dif = (string)formatter.Deserialize(stream);
      stream.Close();
      return dif;
    }
    else
    {
      return "N/A";
    }
  }

  public static void ClearAllWins()
  {
    string path = Application.persistentDataPath + "/totalWins.bin";
    File.Delete(path);
    for (int dif = 0; dif < 4; dif++)
    {
      string path2 = Application.persistentDataPath + "/winsOnGrid" + dif.ToString() + ".bin";
      File.Delete(path2);
    }
  }

  public static void SaveNotes(int dif, int input, int space, bool create = true)
  {
    string path = Application.persistentDataPath + "/notes" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (!File.Exists(path))
    {
      bool[,] notes = new bool[81, 9];
      FileStream stream = new FileStream(path, FileMode.Create);
      notes[space, input] = true;
      formatter.Serialize(stream, notes);
      stream.Close();
      return;
    }
    else if (create)
    {
      FileStream stream1 = new FileStream(path, FileMode.Open);
      bool[,] notes = (bool[,])formatter.Deserialize(stream1);
      notes[space, input] = true;
      stream1.Close();
      FileStream stream2 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream2, notes);
      stream2.Close();
    }
    else
    {
      FileStream stream3 = new FileStream(path, FileMode.Open);
      bool[,] notes = (bool[,])formatter.Deserialize(stream3);
      notes[space, input] = false;
      stream3.Close();
      FileStream stream4 = new FileStream(path, FileMode.Create);
      formatter.Serialize(stream4, notes);
      stream4.Close();
    }
  }

  public static bool[,] LoadNotes(int dif)
  {
    string path = Application.persistentDataPath + "/notes" + dif.ToString() + ".bin";
    BinaryFormatter formatter = new BinaryFormatter();
    if (File.Exists(path))
    {
      FileStream stream = new FileStream(path, FileMode.Open);
      bool[,] notes = (bool[,])formatter.Deserialize(stream);
      stream.Close();
      return notes;
    }
    else
    {
      return null;
    }
  }

  public static void ClearNotes(int dif)
  {
    string path = Application.persistentDataPath + "/notes" + dif.ToString() + ".bin";
    File.Delete(path);
  }
}
