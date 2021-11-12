using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameObject whoWinsTextShadow, player1MoveText, player2MoveText, player1Round, player2Round;
    
    public static GameObject player1, player2, player1WinIcon, player2WinIcon, crown, blackScreen;

    public AudioSource audioSource;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public int roundToWin = 1;

    public static bool gameOver = false;

    public Animator animator;

    public TextAsset jsonFile;

    public static List<MyQuestion> questionPool = new List<MyQuestion>();

    void loadAllQuestion()
    {
        Questions questiosInJson = JsonUtility.FromJson<Questions>(jsonFile.text);

        foreach (MyQuestion question in questiosInJson.questions)
        {
            questionPool.Add(question);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        loadAllQuestion();
        gameOver = false;
        roundToWin = PlayerPrefs.GetInt("RoundValue", 1);

        player1WinIcon = GameObject.Find("Player1WinIcon");
        player2WinIcon = GameObject.Find("Player2WinIcon");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");
        player1Round = GameObject.Find("Player1Round");
        player2Round = GameObject.Find("Player2Round");
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        crown = GameObject.Find("Crown");
        whoWinsTextShadow = GameObject.Find("WhoWinText");
        blackScreen = GameObject.Find("BlackScreen");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1WinIcon.gameObject.SetActive(false);
        player2WinIcon.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
        crown.gameObject.SetActive(false);
        blackScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player1Round.GetComponent<Text>().text = "Round "+(player1.GetComponent<FollowThePath>().waypointIndex / player1.GetComponent<FollowThePath>().waypoints.Length).ToString() + "/" + roundToWin;
        player2Round.GetComponent<Text>().text = "Round " + (player2.GetComponent<FollowThePath>().waypointIndex / player2.GetComponent<FollowThePath>().waypoints.Length).ToString() + "/" + roundToWin;


        if (player1.GetComponent<FollowThePath>().waypointIndex == roundToWin * player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            printWinner(1);
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex == roundToWin * player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            printWinner(2);
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }
    }
    
    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(true);
                break;
            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                player2MoveText.gameObject.SetActive(false);
                player1MoveText.gameObject.SetActive(true);
                break;
        }
    }
    
    public void printWinner(int player)
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        if (player == 1)
        {
            player1WinIcon.gameObject.SetActive(true);
        }
        else
        {
            player2WinIcon.gameObject.SetActive(true);
        }

        audioSource.PlayOneShot(audioSource.clip, volumeValue);
        blackScreen.gameObject.SetActive(true);
        whoWinsTextShadow.gameObject.SetActive(true);
        player1MoveText.gameObject.SetActive(false);
        player2MoveText.gameObject.SetActive(false);
        crown.gameObject.SetActive(true);
        gameOver = true;
        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        animator.Play("Dropdown", -1);

        player1StartWaypoint = 0;
        player2StartWaypoint = 0;
        this.enabled = false;
    }
}
