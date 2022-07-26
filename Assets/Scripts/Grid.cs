using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePref;

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject tile = Instantiate(tilePref, tempPos, Quaternion.identity) as GameObject;
                tile.transform.parent = transform;
                tile.name = i + "," + j;
            }
        }
    }
}
