using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpQuestion : MonoBehaviour
{
    private bool coroutineAllowed = true;
    private int whosTurn = 1;

    [Header("Question Screen Setting")]
    [SerializeField] private Text questionText;
    [SerializeField] private Text[] questionChoice;
    [SerializeField] private Text timer;

    [Header("Answer Screen Setting")]
    [SerializeField] private Image correctScreen;
    [SerializeField] private Text correctText;
    [SerializeField] private GameObject ansScreen;
    [SerializeField] private Text coinText;

    [Header("Dice Setting")]
    [SerializeField] private GameObject diceScreen;
    [SerializeField] private Text diceText;

    [Header("Player Coin Text")]
    [SerializeField] private Text player1Coin;
    [SerializeField] private Text player2Coin;

    private const int TIMERCOUNTER = 15;
    private static int randomQuestionIdex;
    private bool correct = false;

    private MyQuestion currentQuestion;

    [Header("Animator")]
    public Animator animatorQuestionScreen;
    public Animator animatorAnsScreen;

    // Start is called before the first frame update
    void Start()
    {
        diceScreen.SetActive(false);
        ansScreen.SetActive(false);
        player1Coin.text = 0.ToString();
        player2Coin.text = 0.ToString();
    }

    public void randQuestion()
    {
        if (!GameControl.gameOver && coroutineAllowed)
        {
            coroutineAllowed = false;
            randomQuestionIdex = Random.Range(0, GameControl.questionPool.Count);
            currentQuestion = GameControl.questionPool[randomQuestionIdex];
            questionText.text = currentQuestion.question;

            for(int i = 0; i < currentQuestion.choice.Length; i++)
            {
                questionChoice[i].text = currentQuestion.choice[i];
            }

            animatorQuestionScreen.Play("SlideIn", -1);
            timer.text = TIMERCOUNTER.ToString() + " s";
            timer.color = Color.green;
            StartCoroutine("StartTimer");
        }
    }

    public void clickedAns()
    {
        StopCoroutine("StartTimer");
        string choice = EventSystem.current.currentSelectedGameObject.name;
        int selectedChoice = 999;
        switch (choice)
        {
            case "QuestionAns1":
                selectedChoice = 0;
                break;
            case "QuestionAns2":
                selectedChoice = 1;
                break;
            case "QuestionAns3":
                selectedChoice = 2;
                break;
            case "QuestionAns4":
                selectedChoice = 3;
                break;
        }

        if (GameControl.questionPool[randomQuestionIdex].checkAns(selectedChoice)) {
            printCorrect();
        }
        else
        {
            printWrong();
        }
    }

    private void printWrong()
    {
        ansScreen.SetActive(true);
        animatorAnsScreen.Play("ZoomIn", -1);
        correctScreen.color = Color.red;
        correctText.text = "WRONG";
        correct = false;
        StartCoroutine("printPlayerCoin");
        StartCoroutine("Delay");
        movePlayer(0);
    }

    private void printCorrect()
    {
        ansScreen.SetActive(true);
        animatorAnsScreen.Play("ZoomIn", -1);
        correctScreen.color = Color.green;
        correctText.text = "CORRECT";
        correct = true;
        StartCoroutine("printPlayerCoin");
        StartCoroutine("Delay");
        StartCoroutine("RollTheDice");
    }

    private void movePlayer(int step)
    {
        GameControl.diceSideThrown = step;
        if (whosTurn == 1)
        {
            GameControl.MovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameControl.MovePlayer(2);
        }

        whosTurn *= -1;
    }

    private IEnumerator printPlayerCoin()
    {
        for (int i = 0; i < 20; i++)
        {
            coinText.text = Random.Range(0, 999).ToString();
            yield return new WaitForSeconds(0.1f);
        }

        if (whosTurn == 1)
        {
            if (correct)
                GameControl.player1.GetComponent<FollowThePath>().coin += 10;
            coinText.text = (GameControl.player1.GetComponent<FollowThePath>().coin).ToString();
        }
        else if (whosTurn == -1)
        {
            if (correct)
                GameControl.player2.GetComponent<FollowThePath>().coin += 10;
            coinText.text = (GameControl.player2.GetComponent<FollowThePath>().coin).ToString();
        }

        player1Coin.text = (GameControl.player1.GetComponent<FollowThePath>().coin).ToString();
        player2Coin.text = (GameControl.player2.GetComponent<FollowThePath>().coin).ToString();
    }

    private IEnumerator RollTheDice()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        diceScreen.SetActive(true);
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6) + 1;
            diceText.text = randomDiceSide.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        diceScreen.SetActive(false);
        movePlayer(randomDiceSide);
    }

    private IEnumerator Delay()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        animatorAnsScreen.Play("ZoomOut", -1);
        animatorQuestionScreen.Play("SlideOut", -1);
        coroutineAllowed = true;
    }

    private IEnumerator StartTimer()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        for (int i = TIMERCOUNTER; i >= 0; i--)
        {
            if(i <= 3)
            {
                timer.color = Color.red;
            }
            else if( i <= (TIMERCOUNTER / 2))
            {
                timer.color = new Color(255, 167, 0, 255);
            }
            timer.text = i.ToString() + " s";
            yield return new WaitForSeconds(1f);
        }
        printWrong();
        coroutineAllowed = true;
    }
}
