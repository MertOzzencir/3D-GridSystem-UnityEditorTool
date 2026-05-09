using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float zOffset;

    Vector3 currentVelocity;
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - zOffset), ref currentVelocity, 100 * Time.deltaTime);
    }
}
