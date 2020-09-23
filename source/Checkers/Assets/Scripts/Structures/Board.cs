using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board basic dimensions")]
    [SerializeField]
    private int rows = 8;
    [SerializeField]
    private int columns = 8;

    [Header("Tile prefab used instantiate board")]
    [SerializeField]
    private BoardTile tilePrefab;
    [SerializeField]
    private GameObject tileParent;

    private BoardTile[,] board;

    void Start()
    {
        if (tilePrefab != null)
        {
            Sprite targetSprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;

            if (targetSprite != null)
            {
                Vector2 offset = targetSprite.bounds.size;
                Vector2 baseSpaw = SetBaseSpaw(new Vector2(0f, 0f), offset);

                CreateBoard(offset, baseSpaw);
            }
            else
                Debug.Log("Class Board, method Start: Cant acess tile prefab sprite");
        }
        else
            Debug.Log("Class Board, method Start: Tile prefab is null");
    }

    private void CreateBoard(Vector2 offset, Vector2 baseSpaw)
    {
        board = new BoardTile[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                BoardTile tile = Instantiate(tilePrefab,
                                        new Vector3(baseSpaw.x + (offset.x * j),
                                                    baseSpaw.y + (offset.y * i),
                                                    0f),
                                        tilePrefab.transform.rotation);

                tile.transform.SetParent(tileParent.transform);

                board[i, j] = tile;
                board[i, j].SetBoardPosition(i, j);
            }
        }
    }

    private Vector2 SetBaseSpaw(Vector2 baseSpaw, Vector2 offset)
    {
        if (columns > 1)
        {
            float baseX = baseSpaw.x;
            baseX = baseX + (-1 * (offset.x * columns) / 2) + (offset.x / 2);
            baseSpaw.x = baseX;
        }
        if (rows > 1)
        {
            float baseY = baseSpaw.y;
            baseY = baseY + (-1 * (offset.y * rows) / 2) + (offset.y / 2);
            baseSpaw.y = baseY;
        }

        return baseSpaw;
    }
}
