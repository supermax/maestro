using SuperMaxim.IOC.Attributes;

namespace SuperMaxim.Editor.IOC.Tests.Entities
{
    public class Shark : Fish
    {
        [Inject]
        public IFood Food { get; set; }
    }
}
