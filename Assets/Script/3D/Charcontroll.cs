using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace dahyeon
{
    public class Charcontroll : MonoBehaviour
    {

        [SerializeField]
        private float walkSpeed;
        [SerializeField]
        private Inventory inventory;
        //  [SerializeField]
        //private Camera charCamera;
        [SerializeField]
        private Camera exitfalseCamera;
        [SerializeField]
        private GameObject exitfalsepos;

        public Image clockimage;
        Clock clockscript;

        private CharacterController characterController;
        public Animator Myanimator;
        public ParticleSystem[] Particle;

        [SerializeField]
        private float lineSize = 16f;
        private Outline outlinescript;

        private float xmove;
        private float ymove;
        private float xrotation;

        public Vector2 LockAxis;
        public FloatingJoystick Joystick_left;
        public FloatingJoystick Joystick_right;
       float MouseSensitivity = 80f;
        Vector3 move;
        bool grabbtnon;

        public Vector3 sponposition;

        private void Awake()
        {
            Myanimator = this.transform.GetChild(0).GetComponent<Animator>();
        }
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            clockscript = clockimage.GetComponent<Clock>();
            exitfalseCamera.gameObject.SetActive(false);
            Particle[0].enableEmission = false;
            Particle[1].enableEmission = false;
            sponposition = this.transform.position;
        }


        private void FixedUpdate()
        {
            CharacterMove();
            CharTurn();

            Debug.DrawRay(transform.position, transform.forward * lineSize, Color.yellow);

        }

        private void Update()
        {
            StartCoroutine("CameraOnOff");
        }

        private void CharacterMove()
        {
            float x = Joystick_left.Horizontal;
            float z = Joystick_left.Vertical;

            move = transform.right * x + transform.forward * z;
            characterController.Move(move * walkSpeed * Time.deltaTime); //�̵�

            if (Joystick_left.background.gameObject.active == true) //���̽�ƽ���� �̵��� �� �ִϸ��̼�
            {
                Myanimator.SetBool("move", true);
                Particle[0].enableEmission = true;
                Particle[1].enableEmission = true;
            }
            else
            {
                Myanimator.SetBool("move", false);
                Particle[0].enableEmission = false;
                Particle[1].enableEmission = false;
            }

        }

        private void CharTurn()
        {
            float x = Joystick_right.Horizontal;
            float z = Joystick_right.Vertical;

            xmove = x * MouseSensitivity * Time.deltaTime;
            //ymove = y * MouseSensitivity * Time.deltaTime;
            this.gameObject.transform.Rotate(Vector3.up * xmove);
        }
        IEnumerator CameraOnOff()
        {
            yield return new WaitForSeconds(3f);

            RaycastHit hit; //ray�� ������ ������ ���
            if (Physics.Raycast(transform.position, transform.forward, out hit, lineSize))
            {
                if (hit.collider.tag == "Item")
                {
                    outlinescript = hit.collider.transform.GetChild(1).GetComponent<Outline>();
                    //outlinescript = hit.collider.gameObject.GetComponent<Outline>();
                    outlinescript.OutlineColor = outlinescript.OutlineColorSelected;
                    Selectobject(hit.collider);
                }
                else
                {
                    if(outlinescript != null)
                        outlinescript.OutlineColor = Color.white;
                } 
            }
            if (clockscript.isEnded == true)//�ð� ������ ui����
            {
                Myanimator = this.transform.GetChild(0).GetComponent<Animator>();
                exitfalseCamera.gameObject.SetActive(true);
            }
        }




        public void garbbtnclick()//grap��ư �̺�Ʈ
        {
            grabbtnon = true;
            Invoke("grabbtnoff", 0.5f);
        }

        void grabbtnoff()
        {
            grabbtnon = false;
        }

        private void Selectobject(Collider hit)
        {
            if (grabbtnon == true && inventory.Items.Count < inventory.Slots.Length)
            {
                Myanimator.SetTrigger("grap");
                Objectitem objectitem = hit.GetComponent<Objectitem>();
                Item nomalItem = objectitem.iteminObjectitem;
                inventory.AddItem(nomalItem);
                hit.gameObject.SetActive(false);

            }
            else if (inventory.Items.Count >= inventory.Slots.Length)
            {
                outlinescript.OutlineColor = Color.red;
            }
        }


        void OnTriggerStay(Collider other) //������ ������ �� �����ϸ�
        {
            Myanimator = this.transform.GetChild(0).GetComponent<Animator>();
            if (other.gameObject.tag == "Warming")
            {
                if (grabbtnon == true)
                {
                    inventory.ClearSlot();
                    Particle[2].Play();
                }

                clockscript.sucess = clockscript.isEnded;
                if (clockscript.sucess == true)
                {
                    this.transform.position = exitfalsepos.transform.position;
                    this.transform.LookAt(Particle[2].transform.position, Vector3.up);
                    Myanimator.SetTrigger("Exit");
                    transform.DOMove(Particle[2].transform.position, 2).SetEase(Ease.Linear);
                    StartCoroutine(NextScene());
                }
               
            }
        }

        IEnumerator NextScene()
        {
            yield return new WaitForSeconds(3.5f);
            Managers.Scene.GetCurrentScene.GetUIScene().SceneChange();
        }

    }
}
