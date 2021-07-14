using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private QuizManager quizManager; // Получаем класс QuizManager
    [SerializeField] private TMP_Text questionText, scoreText, timerText; // Получаем текст вопроса, число очков, таймер
    [SerializeField] private List<Image> lifeImageList; // Список жизней
    [SerializeField] private GameObject quizGamePanel, mainMenuPanel, gameOverPanel, optionsPanel; // Игровое меню, Главное меню, Конец игры
    [SerializeField] private VideoPlayer VideoPlayer; // Получаем видео элемент 
    [SerializeField] private Image Image; // Получаем фото элемент
    [SerializeField] private List<Button> optionsButtons, uiButtons; // Получаем кнопки вариантов ответа, кнопки выбора типов
    [SerializeField] private Color correctColor, wrongColor, normalColor; // Зелёный цвет, красный цвет, обычный цвет
    [SerializeField] private float timerColor, color;

    private Question question;
    private bool pressed; // Нажал

    public TMP_Text ScoreText { get { return scoreText; } }
    
    public TMP_Text TimerText { get { return timerText; } }
    public GameObject GameOverPanel { get { return gameOverPanel; } }
    public GameObject QuizGamePanel { get { return quizGamePanel; } }
    public GameObject MainMenuPanel { get { return mainMenuPanel; } }
    public GameObject OptionsPanel { get { return optionsPanel; } }

    private void Awake()
    {
        for (int i = 0; i < optionsButtons.Count; i++) // Перебираем кнопки
        {
            Button localButton = optionsButtons[i]; // Кнопку под определённым индексом вставляем в переменную
            localButton.onClick.AddListener(() => OnClick(localButton));
        }
        for (int i = 0; i < uiButtons.Count; i++) // Перебираем кнопки
        {
            Button localButton = uiButtons[i]; // Кнопку под определённым индексом вставляем в переменную
            localButton.onClick.AddListener(() => OnClick(localButton));
        }

    }

    public void SetQuestion(Question question) // Заданный вопрос
    {
            this.question = question; // С question внутри (рандомный вопрос, который мы получили) вставляем в this.question

            switch (question.questionType) // Проверяем тип вопроса (картинка или видео?)
            {
                // Если картинка, то по методу ImageHolder() мы переходим в активацию изображения и отключение видео
                case QuestionType.IMAGE:
                    ImageHolder();
                    Image.sprite = question.questionImg; // Объекту картинке мы задаём спрайт от question.questionImg, который мы задавали в QuizData
                    break; // Остановка
                           // Если видео, то по методу ImageHolder() мы переходим в активацию изображения и отключение видео, но мы после отключения видео,
                           //включаем видео (мы так делаем, чтобы не загружать процессор (Не создавать ещё один метод))
                case QuestionType.VIDEO:
                    ImageHolder();
                    VideoPlayer.gameObject.SetActive(true); // Включили видео
                    VideoPlayer.clip = question.questionVideo; // Добавляем клип от question.questionVideo.    -> Все картинки и видео мы задавали в QuizData в инспекторе
                    break; // Остановка
                default:
                    break;
            }
            questionText.text = question.questionInfo; // Добавляем текстовый вопрос с question.questionInfo

            List<string> answerList = ShuffleList.ShuffleListItems<string>(question.options); // Создаём answerList c теми же элементами, которые расположены в рандомных местах

            for (int i = 0; i < optionsButtons.Count; i++) // Перечисляем массив кнопок
            {
                // В массив кнопки под определённым индексом вычисляем дочерний элемент и 
                //в него вкладываем под определённым индексом текстовый вариант ответа
                optionsButtons[i].GetComponentInChildren<TMP_Text>().text = answerList[i];
                // В кнопку под определённым индексом записываем имя кнопки от массива ответов с определённым индексом
                optionsButtons[i].name = answerList[i];
                optionsButtons[i].image.color = normalColor; // Задаём каждой кнопке обычный цвет
            }
            pressed = false; // Ещё пока не нажимал кнопку
    }
    private void ImageHolder() 
    {
            Image.transform.parent.gameObject.SetActive(true);
            Image.gameObject.SetActive(true);
            VideoPlayer.gameObject.SetActive(false);
    }

    public void OnClick(Button btn) 
    {
        if (quizManager.GameStatus == GameStatus.PLAYING) 
        {
            if (!pressed)
            {
                pressed = true; // Кнопка нажата
                bool val = quizManager.Answer(btn.name); // Значение булевой переменной зависит от метода Answer
                if (val)
                {
                    btn.image.color = correctColor; // Задаём кнопке, на которую мы нажали зелёный цвет
                }
                else
                {
                    btn.image.color = wrongColor; // Задаём кнопке, на которую мы нажали красный цвет
                }
            }
        }
        switch (btn.name)
        {
            case "Insectivores":
                quizManager.StartGame(0);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Bird":
                quizManager.StartGame(1);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Plants":
                quizManager.StartGame(2);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Predators":
                quizManager.StartGame(3);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Rodents":
                quizManager.StartGame(4);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Chiropteran":
                quizManager.StartGame(5);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Artiodactyl":
                quizManager.StartGame(6);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            case "Fish":
                quizManager.StartGame(7);
                mainMenuPanel.gameObject.SetActive(false);
                quizGamePanel.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void ReduceLife(int index) 
    {
       lifeImageList[index].color = wrongColor; // По назначенному индексу изображения списка жизней, делаем изображение в красный цвет
    }

    public void RetryButton() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OptionsButton() 
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void BackButton() 
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void MainMenuButton() 
    {
        mainMenuPanel.gameObject.SetActive(true);
        quizGamePanel.gameObject.SetActive(false);
    }
    public void SetSoundValue(Slider slider) 
    {
        quizManager.AudioManager.VolumeSFX = slider.value;
    }
    public void SetMusicValue(Slider slider) 
    {
        quizManager.AudioManager.VolumeMusic = slider.value;
    }
}
