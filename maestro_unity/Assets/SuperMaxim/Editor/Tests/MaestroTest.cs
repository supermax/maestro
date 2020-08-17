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
        [Test]
        [Order(0)]
        public void Maestro_Map_Test()
        {
            var food = Maestro.Default
                                            .Map<IFood>()
                                            .To<Food>();
            Debug.Log($"test {nameof(Maestro_Map_Test)}<{nameof(IFood)}> result {food}");
            Assert.NotNull(food, $"failed to Map<{nameof(IFood)}>");
            
            var animal = Maestro.Default
                                            .Map<IAnimal>()
                                            .To<Dog>("dog")
                                            .To<Carp>("fish")
                                            .Singleton(new Cat());
            Debug.Log($"test {nameof(Maestro_Map_Test)}<{nameof(IAnimal)}> result {animal}");
            Assert.NotNull(animal, $"failed to Map<{nameof(IAnimal)}>");
            
            var mammal = Maestro.Default
                                                .Map<Mammal>()
                                                .To<Dog>()
                                                .To<Cat>();
            Debug.Log($"test {nameof(Maestro_Map_Test)}<{nameof(Mammal)}> result {mammal}");
            Assert.NotNull(animal, $"failed to Map<{nameof(Mammal)}>");

            var fish = Maestro.Default
                                        .Map<Fish>()
                                        .Singleton<Shark>("jaws")
                                        .To<Carp>("fishy");
            Debug.Log($"test {nameof(Maestro_Map_Test)}<{nameof(Fish)}> result {fish}");
            Assert.NotNull(fish, $"failed to Map<{nameof(Fish)}>");
        }

        [Test]
        [Order(1)]
        public void Maestro_Get_Test()
        {
            Maestro_Get_Test<IAnimal>();

            Maestro_Get_Test<Mammal>();
            
            Maestro_Get_Test<Fish>();
        }

        [Test]
        [Order(2)]
        public void Maestro_Get_Instance_Test()
        {
            Maestro_Get_Instance_Test<IAnimal>();
            
            Maestro_Get_Instance_Test<Mammal>();
            
            Maestro_Get_Instance_Test<Fish>();
        }
        
        [Test]
        [Order(3)]
        public void Maestro_Get_Specific_Instance_Test()
        {
            Maestro_Get_Instance_Test<IAnimal>("dog");
            Maestro_Get_Instance_Test<IAnimal>("fish");
            
            Maestro_Get_Instance_Test<Mammal>("Dog");
            Maestro_Get_Instance_Test<Mammal>("Cat");
            
            Maestro_Get_Instance_Test<Fish>("jaws");
            Maestro_Get_Instance_Test<Fish>("fishy");
        }

        [Test]
        [Order(4)]
        public void Maestro_Injectable_Properties_Test()
        {
            var dog = Maestro.Default.Get<IAnimal>().Instance("dog") as Dog;
            Debug.Log($"test {nameof(Maestro_Injectable_Properties_Test)}<{nameof(IAnimal)}>(\"dog\").Food = {dog?.Food}");
            Assert.NotNull(dog?.Food, $"failed to Get<{nameof(IAnimal)}>.Instance(\"dog\")");
        }

        private static void Maestro_Get_Instance_Test<T>(string key = null) where T : class
        {
            var instance = Maestro.Default.Get<T>().Instance(key);
            Debug.Log($"test {nameof(Maestro_Get_Instance_Test)}<{typeof(T).Name}>({key}) result {instance}");
            Assert.NotNull(instance, $"failed to Get<{nameof(T)}>.Instance({key})");
        }
        
        private static void Maestro_Get_Test<T>() where T : class
        {
            var result = Maestro.Default.Get<T>();
            Debug.Log($"test {nameof(Maestro_Get_Test)}<{typeof(T).Name}> result {result}");
            Assert.NotNull(result, $"failed to Get<{typeof(T).Name}>");
        }
    }
}
