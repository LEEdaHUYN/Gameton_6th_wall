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

        //public GameObject Target;               // ī�޶� ����ٴ� Ÿ��


        //public float CameraSpeed { get; private set; } = 2;//ī�޶��� �ӵ�
        //private Vector3 targetPos;     // Ÿ���� ��ġ

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
