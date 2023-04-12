using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingNumbers : MonoBehaviour
{
    public float Speed;
    public TextMeshProUGUI TMPRO;
    public Transform Player;
    private void Start()
    {
        Destroy(this.gameObject, 2.5f);
    }
    void Update()
    {
        Player = FPSController.PlayerObject.transform;
        this.transform.rotation = Camera.main.transform.rotation;
        transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Speed * Time.deltaTime, this.transform.position.z);
    }
}
