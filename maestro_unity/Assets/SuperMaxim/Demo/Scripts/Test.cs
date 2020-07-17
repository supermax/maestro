using System;
using System.Collections.Generic;
using SuperMaxim.IOC;
using SuperMaxim.IOC.Config;
using UnityEngine;

namespace SuperMaxim.Demo
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            var instance = Maestro.Default.Resolve<IEnemy>();
            Debug.Log(instance);
        }
    }
}