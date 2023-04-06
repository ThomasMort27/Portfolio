using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;


public class PlayerController : MonoBehaviourPunCallbacks
{
    public PhotonView view;
    public float speed;
    // Start is called before the first frame update
void Start()
{
    CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


    if (_cameraWork != null)
    {
        if (photonView.IsMine)
        {
            _cameraWork.OnStartFollowing();
        }
    }
    else
    {
        Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
    }
}

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown("w"))
            {
                Vector3 movement = new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
                transform.position += movement;
            }

            if (Input.GetKeyDown("a"))
            {
                Vector3 movement = new Vector3(-speed * Time.deltaTime, -0.0f, -0.0f);
                transform.position += movement;
            }

            if (Input.GetKeyDown("s"))
            {
                Vector3 movement = new Vector3(-0.0f, -0.0f, -speed * Time.deltaTime);
                transform.position += movement;
            }

            if (Input.GetKeyDown("d"))
            {
                Vector3 movement = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
                transform.position += movement;
            }
        }
    }

}
