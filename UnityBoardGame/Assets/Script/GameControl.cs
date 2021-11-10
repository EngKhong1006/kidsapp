using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText, player1Round, player2Round;

    private static GameObject player1, player2, player1WinIcon, player2WinIcon, crown, blackScreen;

    public AudioSource audioSource;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static int roundToWin = 1;

    public static bool gameOver = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        player1StartWaypoint = 0;
        player2StartWaypoint = 0;
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
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }
    }
    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;
            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
    public void printWinner(int player)
    {
        Debug.Log("Winner");
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
        this.enabled = false;
    }
}
