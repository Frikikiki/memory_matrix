using System;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public Position(int _i, int _j)
    {
        i = _i;
        j = _j;
    }
    
    public int i;
    public int j;
}

public class RandomGenerator
{
    public RandomGenerator()
    {
        for(int i = 0; i <= 4; i++)
        {
            for(int j = 0; j <= 4; j++)
            {
                freePositions.Add(new Position(i,j));
            }
        }

        System.Random rand = new System.Random();
        for (int i = freePositions.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);
    
            Position tmp = freePositions[j];
            freePositions[j] = freePositions[i];
            freePositions[i] = tmp;
        }
    }
    
    public Position GetNextPosition()
    {
        System.Random rand = new System.Random();
        int randIndex = rand.Next(freePositions.Count - 1);
        Position result = freePositions[randIndex];
        freePositions.RemoveAt(randIndex);
        return result;
    }
    
    private List<Position> freePositions = new List<Position>();
}
