using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float zOffset;
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z-zOffset),10*Time.deltaTime);
    }
}
