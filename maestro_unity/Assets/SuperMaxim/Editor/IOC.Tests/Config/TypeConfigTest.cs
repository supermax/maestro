using System.Runtime.Serialization.Json;
using NUnit.Framework;
using SuperMaxim.IOC.Config;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Tests.Config
{
    [TestFixture]
    public class TypeConfigTest
    {
        [Test]
        public void Maestro_TypeConfig_Save_Test()
        {
            var config = new TypeConfig
                {
                    SourceType = typeof(Entities.Animal)
                };

            var json = JsonUtility.ToJson(typeof(Entities.Animal), true);
            Debug.LogFormat("{0}", json);

            Assert.NotNull(json);
        }
    }
}
