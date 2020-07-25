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
        
    }

    public class Cat : Mammal
    {
        
    }

    public abstract class Fish : Animal
    {
        
    }

    public class Shark : Fish
    {
        
    }

    public class Carp : Fish
    {
        
    }
}
