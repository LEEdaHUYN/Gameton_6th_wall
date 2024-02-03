using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace dahyeon
{


    public class animation : MonoBehaviour
    {
        [SerializeField]
        private Image fadeout;

        private void Awake()
        {
            //fadeout.gameObject.SetActive(false);
            fadeout.DOFade(0f, 3f);
            StartCoroutine(fade());
        }

        IEnumerator fade()
        {
            yield return new WaitForSeconds(3f);
            fadeout.gameObject.SetActive(true);
            fadeout.DOFade(1f, 3f);
        }
    }
}
