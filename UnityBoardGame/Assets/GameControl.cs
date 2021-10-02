using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText, player1Round, player2Round;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static int roundToWin = 2;

    public static bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        whoWinsTextShadow = GameObject.Find("WhoWinText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");
        player1Round = GameObject.Find("Player1Round");
        player2Round = GameObject.Find("Player2Round");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        player1Round.GetComponent<Text>().text = "Round "+(player1.GetComponent<FollowThePath>().waypointIndex / player1.GetComponent<FollowThePath>().waypoints.Length).ToString() + "/" + roundToWin;
        player2Round.GetComponent<Text>().text = "Round " + (player2.GetComponent<FollowThePath>().waypointIndex / player2.GetComponent<FollowThePath>().waypoints.Length).ToString() + "/" + roundToWin;


        if (player1.GetComponent<FollowThePath>().waypointIndex == roundToWin * player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            whoWinsTextShadow.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex == roundToWin * player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            whoWinsTextShadow.gameObject.SetActive(true);
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
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








        /*if (player1.GetComponent<FollowThePath>().waypointIndex % player1.GetComponent<FollowThePath>().waypoints.Length > ((player1StartWaypoint + diceSideThrown) % player1.GetComponent<FollowThePath>().waypoints.Length))
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex == player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            if (player1.GetComponent<FollowThePath>().round == roundToWin)
            {
                whoWinsTextShadow.gameObject.SetActive(true);
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(false);
                whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins";
                gameOver = true;
            }
            else
            {
                player1StartWaypoint = 0;
            }
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex == player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            if (player2.GetComponent<FollowThePath>().round == roundToWin)
            {
                whoWinsTextShadow.gameObject.SetActive(true);
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(false);
                whoWinsTextShadow.GetComponent<Text>().text = "Player 2 Wins";
                gameOver = true;
            }
            else
            {

            }
        }*/
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
}
