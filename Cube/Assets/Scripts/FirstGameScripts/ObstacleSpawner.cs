using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float minXPOS, maxXPOS;
    [Space]
    [SerializeField] float minZOffset, maxZOffset;
    [Space]
    [SerializeField] float minObstacleXScale, maxObstacleXScale;
    [Space]
    [SerializeField] GameObject obstaclePrefab;
    [Space]
    [SerializeField] int minNumOfObstacles, maxNumOfObstacles;
    [Space]
    [SerializeField] Vector3 startingPosition;
    [Space]
    [SerializeField] Transform endLineTransform;

    private Vector3 currentSpawnPosition;
    private float lastXPos;

    private void Awake()
    {
        currentSpawnPosition = startingPosition;
        GenerateObstacles();
    }

    public void GenerateObstacles()
    {
        int numberOfObstacles = Random.Range(minNumOfObstacles, maxNumOfObstacles);

        for (int i = 0; i < numberOfObstacles; i++)
        {
            float xPos = GetNextXPos();
            currentSpawnPosition.x = xPos;

            GameObject obstacleClone = Instantiate(obstaclePrefab, currentSpawnPosition, Quaternion.identity);
            float xScale = Random.Range(minObstacleXScale, maxObstacleXScale);
            obstacleClone.transform.localScale = new Vector3(xScale, obstacleClone.transform.localScale.y, obstacleClone.transform.localScale.z);

            float zOffset = Random.Range(minZOffset, maxZOffset);
            currentSpawnPosition.z += zOffset;
        }

        // Set the end line position based on the last obstacle's position
        SetEndLinePosition(currentSpawnPosition);
    }

    private float GetNextXPos()
    {
        float xPos = lastXPos;

        // Move to the next position within the bounds
        if (xPos == 0f)
        {
            xPos = Random.Range(0, 2) == 0 ? -5.5f : 5.5f;
        }
        else if (xPos == -5.5f)
        {
            xPos = 0f;
        }
        else if (xPos == 5.5f)
        {
            xPos = 0f;
        }

        lastXPos = xPos;
        return xPos;
    }

    private void SetEndLinePosition(Vector3 position)
    {
        // Adjust the end line position based on the last obstacle's position
        if (endLineTransform != null)
        {
            endLineTransform.position = new Vector3(position.x, position.y, position.z + 10f); // Adjust the Z offset as needed
        }
    }
}
