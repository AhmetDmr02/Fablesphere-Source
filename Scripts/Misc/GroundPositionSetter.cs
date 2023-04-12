using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPositionSetter : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    void Start()
    {
        Saw();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Saw();
        }
    }
    void Saw()
    {
        Debug.Log("called");
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f))
        {
            this.transform.position = new Vector3(hit.point.x,hit.point.y,hit.point.z);
        }
    }
}
