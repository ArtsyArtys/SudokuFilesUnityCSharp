using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shuffler {

	public static int[] FillArray(int[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = i;
		}
		return arr;
	}
	public static List<int> ShuffleList(List<int> arr)
	{
		for (int i = 0; i < arr.Count; i++)
		{
			int tempNum = arr[i];
			int k = Random.Range(i, arr.Count);
			arr[i] = arr[k];
			arr[k] = tempNum;
		}
		return arr;
	}

	public static int[] ShuffleArray(int[] arr, bool needsToBeFilled = false)
	{
		if (needsToBeFilled)
		{
			arr = FillArray(arr);
		}
		for (int i = 0; i < arr.Length; i++)
		{
			int tempNum = arr[i];
			int k = Random.Range(i, arr.Length);
			arr[i] = arr[k];
			arr[k] = tempNum;
		}
		return arr;
	}

	public static int ShuffleIntFromArray(int[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
    {
      int tempNum = arr[i];
      int k = Random.Range(i, arr.Length);
      arr[i] = arr[k];
      arr[k] = tempNum;
    }
    return arr[0];
	}

	public static int ShuffleIntFromList(List<int> arr)
	{
		for (int i = 0; i < arr.Count; i++)
    {
      int tempNum = arr[i];
      int k = Random.Range(i, arr.Count);
      arr[i] = arr[k];
      arr[k] = tempNum;
    }
    return arr[0];
	}


}
