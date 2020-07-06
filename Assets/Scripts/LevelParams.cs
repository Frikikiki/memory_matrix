using UnityEngine;
using System;
using System.Collections.Generic;

public enum CellValue{Empty, Square, Circle, Triangle, Rhombus};

[System.Serializable]
public class LevelParams
{
    public LevelParams()
    {
    }

    public LevelParams(int lvlNum)
    {
        levelNumber = lvlNum;
        GenerateMemorizingTime();
        GenerateSolvingTime();
        FillShapesPanel();
        FillMatrix();
    }

    private void GenerateMemorizingTime()
    {
        if (levelNumber <= 5)
            memorizingTime = 5;
        else if (levelNumber <= 10)
            memorizingTime = 4;
        else
            memorizingTime = 3;
    }
    
    private void GenerateSolvingTime()
    {
        if (levelNumber <= 5)
            solvingTime = 30;
        else if (levelNumber <= 10)
            solvingTime = 25;
        else
            solvingTime = 20;
    }

    private void FillShapesPanel()
    {
        List<CellValue> allShapes = new List<CellValue>() {CellValue.Square, 
                                CellValue.Circle, CellValue.Triangle, CellValue.Rhombus};
        System.Random random = new System.Random();
        int shapesCount;
        if (levelNumber <= 1)
            shapesCount = 1;
        else if (levelNumber <= 2)
            shapesCount = 2;
        else if (levelNumber <= 3)
            shapesCount = 3;
        else
            shapesCount = 4;
        while(shapesPanel.Count < shapesCount)
        {
            int nextShapeIndex = random.Next(allShapes.Count - 1);
            shapesPanel.Add(allShapes[nextShapeIndex]);
            allShapes.RemoveAt(nextShapeIndex);   
        }
    }

    private void FillMatrix()
    {
        int shapesCount = Math.Min((int)Math.Round(levelNumber * 1.35 + 1.1),25);
        int shapeIndex = 0;
        RandomGenerator randGenerator = new RandomGenerator();
        for (int k = 0; k < shapesCount; k++)
        {
            Position pos = randGenerator.GetNextPosition();
            CellValue shape = shapesPanel[shapeIndex];
            cells[pos.i,pos.j] = shape;
            if (shapeIndex == shapesPanel.Count - 1)
                shapeIndex = 0;
            else
                shapeIndex++;
        }
    }
    
    public CellValue[,] cells = {{CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty}};

    public int levelNumber;
    public float memorizingTime;
    public float solvingTime;
    public List<CellValue> shapesPanel = new List<CellValue>();
}
