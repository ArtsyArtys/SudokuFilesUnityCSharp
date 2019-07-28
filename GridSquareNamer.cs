using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquareNamer : MonoBehaviour
{
    Transform[] gos;

    void Awake()
    {
      gos = GetComponentsInChildren<Transform>();
      for (int i = 0; i < gos.Length; i++)
      {
        if (gos[i] == gameObject.transform)
        {
          continue;
        }
        gos[i].name = (i - 1).ToString();
      }
    }
}
