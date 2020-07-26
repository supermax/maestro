using System.Collections;
using NUnit.Framework;
using SuperMaxim.Editor.Tests.Entities;
using SuperMaxim.IOC;
using UnityEngine;
using UnityEngine.TestTools;

namespace SuperMaxim.Editor.Tests
{
    public class MaestroTest
    {
        [SetUp]
        public void Map()
        {
            Maestro.Default.Map<IAnimal>().To<Dog>("dog").To<Carp>("fish").Singleton(new Cat());
            
            Maestro.Default.Map<Mammal>().To<Dog>().To<Cat>();

            Maestro.Default.Map<Fish>().Singleton<Shark>("jaws").To<Carp>("fishy");
        }

        [Test]
        public void Maestro_Get_Test()
        {
            Maestro_Get_Test<IAnimal>();

            Maestro_Get_Test<Mammal>();
            
            Maestro_Get_Test<Fish>();
        }

        private static void Maestro_Get_Test<T>()
        {
            var result = Maestro.Default.Get<T>();
            Debug.Log($"test {nameof(Maestro_Get_Test)}<{typeof(T).Name}> result {result}");
            Assert.NotNull(result, $"failed to Get<{typeof(T).Name}>");
        }

        [Test]
        public void Maestro_Get_DefaultInstance_Test()
        {
            var defaultAnimalInstance = Maestro.Default.Get<IAnimal>().Instance();
            Debug.Log($"test {nameof(Maestro_Get_DefaultInstance_Test)}<{nameof(IAnimal)}> result {defaultAnimalInstance}");
            Assert.NotNull(defaultAnimalInstance, $"failed to Get<{nameof(IAnimal)}>.Instance()");
            Assert.IsTrue(defaultAnimalInstance.GetType() == typeof(Dog), 
                $"failed to Get<{nameof(IAnimal)}>.Instance() as default instance of {nameof(Dog)}");
        }
    }
}
