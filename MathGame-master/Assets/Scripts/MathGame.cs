using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MathGame : MonoBehaviour
{
    public Text questionText;
    public InputField answerInput;
    public Button submitButton;
    public Text scoreText;
    public Text timerText;
    public GameObject gameOverUI;
    public Text bilgilendirmeText;
    public Text finalScoreText; // New Text component for displaying final score

    private int correctAnswer;
    private char correctOperation;
    private int questionType; // 0: Find the operation, 1: Find the first number, 2: Find the second number, 3: Find the result
    private int score = 0;
    private float timerDuration = 61f;
    private float timer;

    void Start()
    {
        timer = timerDuration;
        GenerateQuestion();
        submitButton.onClick.AddListener(CheckAnswer);
        UpdateScoreText();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        UpdateTimerText();

        if (Input.GetKeyDown(KeyCode.Escape) || timer <= 0f)
        {
            EndGame();
        }
    }

    void GenerateQuestion()
    {
        questionType = Random.Range(0, 4); 
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        int result = 0;
        char[] operations = { '+', '-', '*', '/' };
        correctOperation = operations[Random.Range(0, 4)];

        switch (correctOperation)
        {
            case '+':
                result = num1 + num2;
                break;
            case '-':
                result = num1 - num2;
                break;
            case '*':
                result = num1 * num2;
                break;
            case '/':
                while (num2 == 0 || num1 % num2 != 0)
                {
                    num1 = Random.Range(1, 10);
                    num2 = Random.Range(1, 10);
                }
                result = num1 / num2;
                break;
        }

        switch (questionType)
        {
            case 0: // (num1) ? (num2) = result
                questionText.text = num1 + " ? " + num2 + " = " + result;
                correctAnswer = correctOperation;
                break;
            case 1: // ? (operation) (num2) = result
                questionText.text = "? " + correctOperation + " " + num2 + " = " + result;
                correctAnswer = num1;
                break;
            case 2: // (num1) (operation) ? = result
                questionText.text = num1 + " " + correctOperation + " ? = " + result;
                correctAnswer = num2;
                break;
            case 3: // (num1) (operation) (num2) = ?
                questionText.text = num1 + " " + correctOperation + " " + num2 + " = ?";
                correctAnswer = result;
                break;
        }

        answerInput.text = "";
    }

    void CheckAnswer()
    {
        if (questionType == 0)
        {
            // Check the operation
            char playerAnswer;
            bool isChar = char.TryParse(answerInput.text, out playerAnswer);

            if (isChar && playerAnswer == correctOperation)
            {
                score++;
                UpdateScoreText();
            }
            else
            {
                EndGame(); // Call EndGame if the answer is incorrect
            }
        }
        else
        {
            // Check the number or result
            int playerAnswer;
            bool isNumeric = int.TryParse(answerInput.text, out playerAnswer);

            if (isNumeric && playerAnswer == correctAnswer)
            {
                score++;
                UpdateScoreText();
            }
            else
            {
                EndGame(); // Call EndGame if the answer is incorrect
            }
        }

        GenerateQuestion();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Skor: " + score;
    }

    void UpdateTimerText()
    {
        timerText.text = "Kalan süre: " + Mathf.Round(timer) + " saniye";
    }

    void EndGame()
    {
        gameOverUI.SetActive(true); // Activate the Game Over UI
        Time.timeScale = 0f;

        if (timer <= 0f)
        {
            bilgilendirmeText.text = "Süreniz doldu!";
        }
        else if (answerInput.text != correctAnswer.ToString()) // Wrong answer check
        {
            bilgilendirmeText.text = "Yanlýþ cevap verdiniz!";
        }

        // Display final score on the Game Over UI
        finalScoreText.text = "Skor: " + score;
    }

    public void WrongAnswer()
    {
        EndGame();
    }

    public void Retry()
    {
        // Reset score and timer
        score = 0;
        UpdateScoreText();
        timer = timerDuration;

        // Generate new question
        GenerateQuestion();

        // Deactivate Game Over UI
        gameOverUI.SetActive(false);

        // Resume time
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
        Time.timeScale = 1f;
    }
}
