using SuperMaxim.IOC.Attributes;

namespace SuperMaxim.Editor.Tests.Entities
{
    public interface IAnimal
    {
                
    }

    public abstract class Animal : IAnimal
    {
        
    }
    
    public abstract class Mammal : Animal
    {
        
    }

    public class Dog : Mammal
    {
        [Inject]
        public IFood Food { get; set; }
    }

    public class Cat : Mammal
    {
        
    }

    public abstract class Fish : Animal
    {
        
    }

    public class Shark : Fish
    {
        [Inject]
        public IFood Food { get; set; }
    }

    public class Carp : Fish
    {
        
    }

    public interface IFood
    {
        
    }

    public class Food : IFood
    {
        
    }
}
