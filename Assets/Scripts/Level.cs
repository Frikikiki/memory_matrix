using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{   
    public Sprite emptyButtonBackground;
    public Sprite square;
    public Sprite circle;
    public Sprite triangle;
    public Sprite rhombus;
    public Sprite square_red;
    public Sprite circle_red;
    public Sprite triangle_red;
    public Sprite rhombus_red;
    public Sprite square_crossed;
    public Sprite circle_crossed;
    public Sprite triangle_crossed;
    public Sprite rhombus_crossed;
    public Text trialText;
    public Text timeText;
    public Text finalMessage;
    public Text buttonDescription;
    public GameObject nextLevelButton;
    public GameObject repeatButton;
    public GameObject completeButton;
    public GameObject shapesPanel;
    public GameObject squareButton;
    public GameObject circleButton;
    public GameObject triangleButton;
    public GameObject rhombusButton;
    public GameObject cells;

    private Sprite currentShape;

    private CellValue[,] userCells = {{CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty},
                                    {CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty,CellValue.Empty}};

    
    
    private string levelNumberPath;
    private string levelParamsPath;

    private int currentLevelNumber;
    private static LevelParams levelParams = new LevelParams();
    private int taskNumber = 1;
    private float time;
    private bool isTaskSolvingNow;
    private bool isTaskChecked;

    void Start()
    {
        DetermineLevelParams();
        AddButtonsListener();
        StartTask();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("main");
        }
        if (isTaskChecked)
        {
            return;
        }
        UpdateTime();
    }

    void StartTask()
    {        
        ResetUserCells();
        ChangeButtonInteractable(false);
        isTaskChecked = false;
        isTaskSolvingNow = false;
        trialText.text = "level " + currentLevelNumber + ", task " + taskNumber;
        time = levelParams.memorizingTime;
        timeText.text = time.ToString();
        
        finalMessage.enabled = false;
        completeButton.SetActive(false);
        nextLevelButton.SetActive(false);
        repeatButton.SetActive(false);
        buttonDescription.enabled = false;

        FillMatrix();
        
        shapesPanel.SetActive(false);
        FillShapesPanel();
    }

    private void UpdateTime()
    {
        float roundedTime = Mathf.Round(time);
        if (roundedTime == 0)
        {
            if (isTaskSolvingNow)
            {
                timeText.text = roundedTime.ToString();
                CheckMatrix();
                return;
            }
            else
            {
                SwitchToSolvingMode();
            }
        }
        time -= Time.deltaTime;
        timeText.text = roundedTime.ToString();
    }


    private void DetermineLevelParams()
    {
        DetermineCurrentLevelNumber();
        levelParams = new LevelParams(currentLevelNumber);
    }

    private void DetermineCurrentLevelNumber()
    {
        if(PlayerPrefs.HasKey("levelNumber"))
        {
            currentLevelNumber = PlayerPrefs.GetInt("levelNumber");
        }
        else
        {
            currentLevelNumber = 1;
            PlayerPrefs.SetInt("levelNumber", currentLevelNumber);
        }
    }

    private void FillShapesPanel()
    {
        CellValue[] allShapes = {CellValue.Square, CellValue.Circle, 
                                CellValue.Triangle, CellValue.Rhombus};
        foreach (CellValue shape in allShapes)
        {
            GameObject button = DetermineShapeButton(shape);
            if (levelParams.shapesPanel.Contains(shape))
                button.SetActive(true);
            else
                button.SetActive(false);
        }
    }

    private GameObject DetermineShapeButton(CellValue shape)
    {
        if (shape == CellValue.Square)
            return squareButton;
        else if (shape == CellValue.Circle)
            return circleButton;
        else if (shape == CellValue.Triangle)
            return triangleButton;
        else if (shape == CellValue.Rhombus)
            return rhombusButton;
        else return null;
    }

    private void SwitchToSolvingMode() 
    {
        ChangeButtonInteractable(true);
        SetCurrentShape();

        if (levelParams.shapesPanel.Count > 1)
            shapesPanel.SetActive(true);

        completeButton.SetActive(true);
        time = levelParams.solvingTime;
        isTaskSolvingNow = true;
        ClearMatrix();
    }            
    
    private void AddButtonsListener()
    {
        Button[] buttons = cells.GetComponentsInChildren<Button>();
        int k = 0;
        foreach (Button button in buttons)
        {
            int i = k / 5, j = k % 5;
            button.onClick.AddListener(delegate {onClickCell(button, i, j);});
            k++;
        }
    }

    private void ChangeButtonInteractable(bool isInteractable)
    {
        Button[] buttons = cells.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = isInteractable;
        }
    }

    void onClickCell (Button button, int i, int j)
    {
        if (button.image.sprite == currentShape)
        {
            userCells[i, j] = CellValue.Empty;
            button.image.sprite = emptyButtonBackground;
        }
        else
        {
            button.image.sprite = currentShape;
            
            if (currentShape == square)
                userCells[i, j] = CellValue.Square;
            else if (currentShape == circle)
                userCells[i, j] = CellValue.Circle;
            else if (currentShape == triangle) 
                userCells[i, j] = CellValue.Triangle;
            else if (currentShape == rhombus)
                userCells[i, j] = CellValue.Rhombus;
        }
    }

    private void SetCurrentShape()
    {
        CellValue firstShape = levelParams.shapesPanel[0];
        if (firstShape == CellValue.Square)
            currentShape = square;
        else if (firstShape == CellValue.Circle)
            currentShape = circle;
        else if (firstShape == CellValue.Triangle)
            currentShape = triangle;
        else if (firstShape == CellValue.Rhombus)
            currentShape = rhombus;
    }

    public void ChangeCurrentShape(Sprite shapeSprite)
    {
        currentShape = shapeSprite;
    }

    private void ClearMatrix()
    {
        Button[] buttons = cells.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.image.sprite = emptyButtonBackground;
        }
    }

    private void ResetUserCells()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
               userCells[i,j] = CellValue.Empty;
            }
        }
    }

    private void FillMatrix()
    {
        Button[] buttons = cells.GetComponentsInChildren<Button>();
        int k = 0;
        foreach (Button button in buttons)
        {
            int i = k / 5, j = k % 5;
            if (levelParams.cells[i, j] == CellValue.Square)
                button.image.sprite = square;
            else if (levelParams.cells[i, j] == CellValue.Circle)
                button.image.sprite = circle;
            else if (levelParams.cells[i, j] == CellValue.Triangle)
                button.image.sprite = triangle;
            else if (levelParams.cells[i, j] == CellValue.Rhombus)
                button.image.sprite = rhombus;
            else
                button.image.sprite = emptyButtonBackground;
            k++;
        }
    }

    public void CheckMatrix()
    {
        isTaskChecked = true;
        bool isUserRight = CheckCells();
        if (isUserRight) 
        {
            SwitchToNextLevelMode();
        }
        else
        {
            SwitchToRepeatMode();
        }
    }

    private bool CheckCells()
    {
        bool isUserRight = true;
        Button[] buttons = cells.GetComponentsInChildren<Button>();
        int k = 0;
        foreach (Button button in buttons)
        {
            int i = k / 5, j = k % 5;
            if (userCells[i,j] != levelParams.cells[i,j])
            {
                isUserRight = false;
                FixIncorrectCell(button, i, j);
            }
            k++;
        }
        return isUserRight;
    }

    private void FixIncorrectCell(Button button, int i, int j)
    {
        if (levelParams.cells[i,j] == CellValue.Empty)
        {
            if (userCells[i,j] == CellValue.Square)
                button.image.sprite = square_crossed;
            else if (userCells[i,j] == CellValue.Circle)
                button.image.sprite = circle_crossed;
            else if (userCells[i,j] == CellValue.Triangle)
                button.image.sprite = triangle_crossed;
            else if (userCells[i,j] == CellValue.Rhombus)
                button.image.sprite = rhombus_crossed;
        }
        else
        {
            if (levelParams.cells[i,j] == CellValue.Square)
                button.image.sprite = square_red;
            else if (levelParams.cells[i,j] == CellValue.Circle)
                button.image.sprite = circle_red;
            else if (levelParams.cells[i,j] == CellValue.Triangle)
                button.image.sprite = triangle_red;
            else if (levelParams.cells[i,j] == CellValue.Rhombus)
                button.image.sprite = rhombus_red;
        }
    }

    private void SwitchToNextLevelMode() 
    {
        if(taskNumber < 4)
        {
            taskNumber++;
            levelParams = new LevelParams(currentLevelNumber);
            StartTask();
        }
        else
        {
            ChangeButtonInteractable(false);
            finalMessage.text = "Level complete";
            finalMessage.enabled = true;
            shapesPanel.SetActive(false);
            completeButton.SetActive(false);
            nextLevelButton.SetActive(true);
            buttonDescription.text = "Next level";
            buttonDescription.enabled = true;
        }
    }

    private void SwitchToRepeatMode() 
    {
        ChangeButtonInteractable(false);
        finalMessage.text = "Level failed";
        finalMessage.enabled = true;             
        shapesPanel.SetActive(false);
        completeButton.SetActive(false);
        repeatButton.SetActive(true);
        buttonDescription.text = "Try again";
        buttonDescription.enabled = true;
    }

    public void onClickNextButton()
    {
        PlayerPrefs.SetInt("levelNumber", currentLevelNumber + 1);
        SceneManager.LoadScene("level");
    }

    public void onClickRepeatButton()
    {
        SceneManager.LoadScene("level");
    }

    public void onClickBackButton()
    {
        SceneManager.LoadScene("main");
    }
}