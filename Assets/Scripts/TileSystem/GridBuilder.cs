using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject mainPrefab;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private List<GameObject> createdTiles;

    [ContextMenu("Build grid")]
    private void BuildGrid()
    {
        ClearGrid();
        createdTiles = new();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                CreateTile(x, z);
            }
        }
    }

    [ContextMenu("Delete grid")]
    private void ClearGrid()
    {
        foreach (GameObject tile in createdTiles)
        {
            DestroyImmediate(tile);
        }

        createdTiles.Clear();
    }

    private void CreateTile(float xPos, float zPos)
    {
        Vector3 newPos = new Vector3(xPos, 0, zPos);
        GameObject newTile = Instantiate(mainPrefab, newPos, Quaternion.identity, transform);

        createdTiles.Add(newTile);
    }
}
