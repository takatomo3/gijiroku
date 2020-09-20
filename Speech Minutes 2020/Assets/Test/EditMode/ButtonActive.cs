using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class ButtonActive : MonoBehaviour
    {
        public GameObject Wadai;


        public Toggle toggle;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTolggle0()
        {
            if (toggle.isOn) Wadai.SetActive(true);
            else Wadai.SetActive(false);
        }
    }

}
