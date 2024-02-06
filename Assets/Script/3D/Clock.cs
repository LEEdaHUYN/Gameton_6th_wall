using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace dahyeon
{



    public class Clock : MonoBehaviour
    {
        [SerializeField]
        private Image fadeout;
        [SerializeField]
        private Charcontroll charcontrollscript;
        [SerializeField]
        private GameObject wall;
        //public GameObject warming;

        public TextMeshProUGUI text_CoolTime;
        public Image red;
        public LoopType loopType;
        public Image image_fill;
        public GameObject Warming;

        private float time_cooltime = 60f;
        private float time_current;
        private float time_start;
        public bool isEnded = false;
        public bool sucess = false;

        void Start()
        {

            Init_UI();
            Trigger_Skill();
            red.gameObject.SetActive(false);
            red.DOFade(0.0f, 1).SetLoops(-1, loopType);
        }

        void Update()
        {
            if (isEnded)
                return;
            Check_CoolTime();
        }
        private void Init_UI()
        {
            image_fill.type = Image.Type.Filled;
            image_fill.fillMethod = Image.FillMethod.Radial360;
            image_fill.fillOrigin = (int)Image.Origin360.Top;
            image_fill.fillClockwise = false;
        }
        private void Check_CoolTime()
        {
            time_current = Time.time - time_start;
            if (time_current < time_cooltime)
            {
                Set_FillAmount(time_cooltime - time_current);
                if (time_current >= 45f)
                {
                    red.gameObject.SetActive(true);
                }

            }
            else if (!isEnded)
            {
                StartCoroutine(End_CoolTime());
                isEnded = true;
            }
        }

        IEnumerator End_CoolTime()
        {
            yield return new WaitForSeconds(0.5f);
            Set_FillAmount(0);
            text_CoolTime.gameObject.SetActive(false);
            red.gameObject.SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
            if (sucess == false)
            {
                charcontrollscript.Myanimator.SetTrigger("exitfalse");
                fadeout.DOFade(1f, 2.2f);
                StartCoroutine(GoToTitleScene());
            }
            else if (sucess == true)
            {
                wall.gameObject.SetActive(false);
                Warming.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                fadeout.DOFade(1f, 2.2f);
            }

        }

        IEnumerator GoToTitleScene()
        {
            yield return new WaitForSeconds(3.5f);
            Managers.Scene.GetCurrentScene.GetUIScene().SceneChange();
        }


        private void Trigger_Skill()
        {
            if (!isEnded)
            {
                Debug.LogError("Hold On");
                return;
            }

            StartCoroutine("Reset_CoolTime");
        }
        IEnumerator Reset_CoolTime()
        {
            yield return new WaitForSeconds(3f);
            text_CoolTime.gameObject.SetActive(true);
            time_current = time_cooltime;
            time_start = Time.time;
            Set_FillAmount(time_cooltime);
            isEnded = false;
        }
        private void Set_FillAmount(float _value)
        {
            image_fill.fillAmount = _value / time_cooltime;
            string txt = _value.ToString("0");
            text_CoolTime.text = txt;
        }
    }

}
