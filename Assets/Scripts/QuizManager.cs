using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private List<QuizDataScriptable> quizData; // Список викторин
    [SerializeField] private QuizUI quizUi;
    [SerializeField] private float timeLimit = 30f; // Ограничение таймера

    [Header("AudioSource")]
    [SerializeField] private Audio audioManager;

    private int ScoreCount = 0; // Количество очков
    private float currentTimer; // Таймер
    private int lifeRemaining = 3; // Оставшаяся жизнь
    private List<Question> questions; // Создаём массив элементов класса
    private Question SelectedQuestion; // Выберите вопрос

    private GameStatus gameStatus = GameStatus.NEXT; // Игра не запущена, если NEXT

    public GameStatus GameStatus { get { return gameStatus; } } // Возвращаем GameStatus

    public Audio AudioManager 
    {
        get { return audioManager; }
        set { audioManager = value; }
    }
    public void InitializeAudioManager() 
    {
        audioManager.SourceMusic = gameObject.AddComponent<AudioSource>();
        audioManager.SourceSFX = gameObject.AddComponent<AudioSource>();
        audioManager.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioListener>();
    }
    private void Awake()
    {
        InitializeAudioManager();
    }
    private void Start()
    {
        bool menu = true;
        if (quizUi.MainMenuPanel || quizUi.OptionsPanel)
        {
            audioManager.PlayMusic(menu);
        }
    }
    // Находим кнопку в главном меню, которую мы нажали. У этой кнопки задан определённый индекс, и в этом индексе имеется список викторин по этой кнопку (Например по птицам)
    public void StartGame(int index) // Начало игры, кнопки типов в главном меню под определённым индексом
    {
        bool menuBool = false;
        audioManager.PlayMusic(menuBool);
        ScoreCount = 0; // Количество очков
        currentTimer = timeLimit; // Таймер
        lifeRemaining = 3; // Оставшаяся жизнь

        questions = new List<Question>(); // Список равен нулю

        for (int i = 0; i < quizData[index].questions.Count; i++)// Количество элементов списка в определённом индексу
        {
            questions.Add(quizData[index].questions[i]); // Добавляем весь список элементов в questions с quizData.questions
        }


        SelectQuestion(); // Выбираем вопрос
        gameStatus = GameStatus.PLAYING;
    }

    [System.Obsolete]
    private void Update()
    {
            if (GameStatus == GameStatus.PLAYING) // Если мы находимся в игре
            {
                currentTimer -= Time.deltaTime; // Отнимаем время
                SetTimer(currentTimer); // Задаём время
            }
    }
    private void SelectQuestion() // Выбираем вопрос
    {
        int val = UnityEngine.Random.Range(0,questions.Count); // Рандомно выбирает вопросы с элементами
        SelectedQuestion = questions[val]; // В вопрос помещаем выбранный элемент по индексу
        quizUi.SetQuestion(SelectedQuestion); // Задаём вариант вопроса в SetQuestion
        questions.RemoveAt(val); // Удаляем данный индекс
    }

    private void SetTimer(float value) 
    {
      TimeSpan time = TimeSpan.FromSeconds(value); // В секундах задаём
      quizUi.TimerText.text = "Time:" + time.ToString("mm':'ss"); // В текст передаём таймер

      if (currentTimer <= 0)
      {
            currentTimer = timeLimit;
            lifeRemaining--;
            quizUi.ReduceLife(lifeRemaining);
            SelectQuestion();
      }
    }

    public bool Answer(string pressed)  // pressed - это имя кнопки по которой мы нажали
    {
        bool correctAns = false;
            if (pressed == SelectedQuestion.correctAnswer) // Если название кнопки с вариантом ответа равна правильному ответу
            {
                // Правильно
                correctAns = true; // Ответ правильный
                audioManager.PlaySound("Won");
                ScoreCount += 50; // Добавляем количество очков
                quizUi.ScoreText.text = $"Score: {ScoreCount}"; // Переносим количество очков в текстовый объект
                currentTimer = timeLimit + 1;
            }
            else
            {
                // Не правильно
                lifeRemaining--; // Отнимаем жизнь
                audioManager.PlaySound("Fail");
                quizUi.ReduceLife(lifeRemaining); // Метод регулирующий цвет изображения по числу(индексу)
                currentTimer = timeLimit + 1;
                if (lifeRemaining <= 0)
                {
                    gameStatus = GameStatus.NEXT;
                    quizUi.QuizGamePanel.transform.Find("QuestionInfo").gameObject.SetActive(false);
                    quizUi.QuizGamePanel.transform.Find("OptionsHolder").gameObject.SetActive(false);
                    quizUi.GameOverPanel.SetActive(true);
                }
            }

            if (gameStatus == GameStatus.PLAYING)
            {
                if (questions.Count > 0)
                {
                    Invoke("SelectQuestion", 0.4f);
                }
                else
                {
                    gameStatus = GameStatus.NEXT;
                    quizUi.QuizGamePanel.transform.Find("QuestionInfo").gameObject.SetActive(false);
                    quizUi.QuizGamePanel.transform.Find("OptionsHolder").gameObject.SetActive(false);
                    quizUi.GameOverPanel.SetActive(true);
                }
            }
        return correctAns; // Выводим булевое значение
    }
    public void MuteButton(Slider slider)
    {
        slider.value = 0;
    }
}
[System.Serializable]
public class Question 
{
    public string questionInfo; // Вопрос
    public QuestionType questionType; // Типы изображений
    public VideoClip questionVideo; // Видео
    public Sprite questionImg; // Фото
    public List<string> options; // Варианты ответов
    public string correctAnswer; // Правильный ответ
}
[System.Serializable]
public enum QuestionType 
{
    IMAGE,
    VIDEO
}
[System.Serializable]
public enum GameStatus 
{
    NEXT,
    PLAYING
}
