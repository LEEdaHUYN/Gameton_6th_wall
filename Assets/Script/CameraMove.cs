using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraMove : MonoBehaviour
{

    //public GameObject Target;               // 카메라가 따라다닐 타겟


    //public float CameraSpeed { get; private set; } = 2;//카메라의 속도
    //private Vector3 targetPos;     // 타겟의 위치

    private Transform CameraArm;
    private Transform player;
    [SerializeField]
    private float dist = 10.0f;
    [SerializeField]
    private float height = 5.0f;
    [SerializeField]
    private float damptrace = 20.0f;
    [SerializeField]
    private GameObject[] map;

    private void Start()
    {
        int RandomNumber = Random.Range(0, 4);
        Instantiate(map[RandomNumber]);
        CameraArm = GameObject.FindWithTag("CameraArm").gameObject.transform;
        player = GameObject.FindWithTag("Player").gameObject.transform;
    }
    private void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, CameraArm.position - (CameraArm.forward * dist) + (Vector3.up * height), Time.deltaTime * damptrace);
        transform.LookAt(player);
    }



}
