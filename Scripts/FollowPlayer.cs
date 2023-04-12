using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool NotY;
    public float lerpRatio;
    public GameObject Player;
    private void Start()
    {
        Player = FPSController.PlayerObject;
    }
    void LateUpdate()
    {
        if (!NotY)
        {
            Vector3 DesiredLoc = Vector3.Lerp(this.transform.position, Player.transform.position, Time.deltaTime * lerpRatio);
            this.transform.position = DesiredLoc;
        }
        else
        {
            Vector3 Bla = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            Vector3 DesiredLoc = Vector3.Lerp(Bla, Player.transform.position, Time.deltaTime * lerpRatio);
            this.transform.position = DesiredLoc;
        }
    }
}
