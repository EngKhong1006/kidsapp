using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpQuestion : MonoBehaviour
{
    private bool coroutineAllowed = true;
    private int whosTurn = 1;
    private int randBuff = 0;

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

    [Header("Buff Screen Setting")]
    [SerializeField] private GameObject buffScreen;
    [SerializeField] private Text randomBuffText;
    [SerializeField] private Text playerCoinText;
    [SerializeField] private GameObject player1Icon;
    [SerializeField] private GameObject player2Icon;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button rejectButton;

    private const int TIMERCOUNTER = 15;
    private static int randomQuestionIdex;
    private bool correct = false;
    private bool affortable = false;

    private MyQuestion currentQuestion;

    [Header("Animator")]
    public Animator animatorQuestionScreen;
    public Animator animatorAnsScreen;

    // Start is called before the first frame update
    void Start()
    {
        diceScreen.SetActive(false);
        ansScreen.SetActive(false);
        buffScreen.SetActive(false);
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

    public void rollDice()
    {
        if (whosTurn == 1)
        {
            player1Coin.text = (GameControl.player1.GetComponent<FollowThePath>().coin -= 30).ToString();
        }
        else if (whosTurn == -1)
        {
            player2Coin.text = (GameControl.player2.GetComponent<FollowThePath>().coin -= 30).ToString();
        }
        StartCoroutine("RollTheDice");
    }

    public void rollDiceWithoutBuff()
    {
        randBuff = 0;
        StartCoroutine("RollTheDice");
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
        int tempCoin = 0;
        ansScreen.SetActive(true);
        animatorAnsScreen.Play("ZoomIn", -1);
        correctScreen.color = Color.green;
        correctText.text = "CORRECT";
        correct = true;
        increaseCoin();
        StartCoroutine("printPlayerCoin");
        StartCoroutine("Delay");

        if (whosTurn == 1)
            tempCoin = GameControl.player1.GetComponent<FollowThePath>().coin;
        else
            tempCoin = GameControl.player2.GetComponent<FollowThePath>().coin;

        if (tempCoin >= 30) {
            affortable = true;
            StartCoroutine("RandBuff");
        }
        else
        {
            randBuff = 0;
            affortable = false;
            StartCoroutine("RollTheDice");
        }
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

    private void increaseCoin()
    {
        if (whosTurn == 1)
        {
            GameControl.player1.GetComponent<FollowThePath>().coin += 10;
        }
        else if (whosTurn == -1)
        {
            GameControl.player2.GetComponent<FollowThePath>().coin += 10;
        }
    }

    private IEnumerator printPlayerCoin()
    {
        int tempCoin = 0;
        if (whosTurn == 1)
        {
            tempCoin = GameControl.player1.GetComponent<FollowThePath>().coin;
        }
        else if (whosTurn == -1)
        {
            tempCoin = GameControl.player2.GetComponent<FollowThePath>().coin;
        }

        for (int i = 0; i < 10; i++)
        {
            int x = (tempCoin - 10 + i + 1);
            if (x < 0)
                x = 0;
            else if (!correct)
            {
                x = tempCoin;
            }
            coinText.text = x.ToString();
            yield return new WaitForSeconds(0.2f);
        }

        player1Coin.text = (GameControl.player1.GetComponent<FollowThePath>().coin).ToString();
        player2Coin.text = (GameControl.player2.GetComponent<FollowThePath>().coin).ToString();
    }

    private IEnumerator RollTheDice()
    {
        if (!affortable)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        buffScreen.SetActive(false);
        diceScreen.SetActive(true);
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6) + 1;
            diceText.text = randomDiceSide.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        if(randBuff != 0)
            diceText.text = randomDiceSide.ToString() + " + " + randBuff;
        else
            diceText.text = randomDiceSide.ToString();

        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        diceScreen.SetActive(false);
        movePlayer(randomDiceSide + randBuff);
    }

    private IEnumerator RandBuff()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        buffScreen.SetActive(true);

        if(whosTurn == 1)
        {
            player1Icon.SetActive(true);
            player2Icon.SetActive(false);
            playerCoinText.text = (GameControl.player1.GetComponent<FollowThePath>().coin).ToString();
        }
        else
        {
            player1Icon.SetActive(false);
            player2Icon.SetActive(true);
            playerCoinText.text = (GameControl.player2.GetComponent<FollowThePath>().coin).ToString();
        }

        randBuff = 0;
        for (int i = 0; i <= 20; i++)
        {
            randBuff = Random.Range(0, 6) + 1;
            randomBuffText.text = " +" + randBuff.ToString();
            yield return new WaitForSeconds(0.1f);
        }
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
