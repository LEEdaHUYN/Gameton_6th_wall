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

        float offsetX = 0.0f;
        float offsetY = 8.8f;
        float offsetZ = -3.3f;

        Vector3 targetPos;

        private void Start()
        {
            int RandomNumber = Random.Range(0, 4);
            Instantiate(map[RandomNumber]);
            CameraArm = GameObject.FindWithTag("CameraArm").gameObject.transform;
            player = GameObject.FindWithTag("Player").gameObject.transform;
            //offset = transform.position - player.transform.position;
        }
        private void FixedUpdate()
        {
            targetPos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);

            transform.position = targetPos;
            //transform.rotation.y = Quaternion.Euler(player.transform.rotation.y);
        }
    }
}
