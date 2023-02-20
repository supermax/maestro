using SuperMaxim.IOC.Attributes;

namespace SuperMaxim.Editor.IOC.Tests.Entities
{
    public class Dog : Mammal
    {
        [Inject]
        public IFood Food { get; set; }
    }
}
