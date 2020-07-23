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
            var map = Maestro.Default.Map<IEnemy>().To<Witch>();
            Debug.Log(map);

            var instance = Maestro.Default.Get<IEnemy>().Instance();
            Debug.Log(instance);

            var unmap = Maestro.Default.UnMap<IEnemy>().From<Witch>();
            Debug.Log(unmap);
        }
    }
}