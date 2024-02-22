using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace dahyeon
{
    public class CameraMove : MonoBehaviour
    {

        //public GameObject Target;               // 카메라가 따라다닐 타겟


        //public float CameraSpeed { get; private set; } = 2;//카메라의 속도
        //private Vector3 targetPos;     // 타겟의 위치

        private Transform CameraArm;
        private Transform player;

        [SerializeField]
        private GameObject[] map;
        [SerializeField]
        private float Y = 0f;



        Vector3 targetPos;

        private void Start()
        {
            int RandomNumber = Random.Range(0, 4);
            Instantiate(map[RandomNumber]);
            CameraArm = GameObject.FindWithTag("CameraArm").gameObject.transform;
            player = GameObject.FindWithTag("CameraLookat").gameObject.transform ;

        }
        private void FixedUpdate()
        {

            transform.position = CameraArm.transform.position;
            transform.LookAt(player);
            
        }
    }
}
