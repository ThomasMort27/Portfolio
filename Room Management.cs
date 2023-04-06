using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
public class RoomManagement : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreateServerName;
    public TMP_InputField JoinServerName;
    public TextMeshProUGUI MSGText;
    public TextMeshProUGUI CreateServerFailed;
    public TextMeshProUGUI JoinServerFailed;
    public void CreateRoom()
    {
        if(CreateServerName.text != null && CreateServerName.text != "Room Name")
        {
                RoomOptions rOption = new RoomOptions();
                rOption.IsOpen = true;
                rOption.IsVisible = true;
                rOption.MaxPlayers = 10;
                PhotonNetwork.CreateRoom(CreateServerName.text, rOption);
                Debug.Log(CreateServerName.text);
        }
        else
        {
            MSGText.text = "Please input a valid Server Name!";
        }
    }

    public void JoinRoom()
    {
        if (JoinServerName.text != null && JoinServerName.text != "Room Name")
        {
            PhotonNetwork.JoinRoom(JoinServerName.text);
            Debug.Log("Joining Room...");
        }
        else
        {
            MSGText.text = "Please input a valid Server Name!";
        }
    }

    //will handle the success of joining the room.
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
        base.OnJoinedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        message = CreateServerFailed.text;
        CreateServerFailed.text = "Creation of room has failed. Please try again.";
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        message = JoinServerFailed.text;
        JoinServerFailed.text = "Room either doesnt exist or there are interruptions. Please try again.";
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
    }
}
