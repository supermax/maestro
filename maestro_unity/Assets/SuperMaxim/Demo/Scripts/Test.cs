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
            Maestro.Default.Map<IEnemy>().To<Witch>();

            Maestro.Default.UnMap<IEnemy>().From<Witch>();
        }
    }
}