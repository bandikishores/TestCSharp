//Abstract animal interface

using Autofac;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

interface IAnimal { void Eat(); }

public enum SourceEnum
{
    PERSONAL_DATA_EXTRACTION,
    SWSS,
    OTHER
}

//Concrete animal 1
[Export(typeof(IAnimal))]
class Lion : IAnimal
{
    public Lion() { Console.WriteLine("Grr.. Lion got created"); }
    public void Eat() { Console.WriteLine("Grr.. Lion eating meat"); }
}

//Concrete animal 2
[Export(typeof(IAnimal))]
class Rabbit : IAnimal
{
    public Rabbit() { Console.WriteLine("Crrr.. Rabbit got created"); }
    public void Eat() { Console.WriteLine("Crrr.. Rabbit eating carrot"); }
}

//Our Zoo. MEF will inject animals to this zoo later, at the time of composition
class Zoo
{
    [ImportMany(typeof(IAnimal))]
    public IEnumerable<IAnimal> Animals { get; set; }
}

interface IDummy { void Print(); }

[Export(typeof(IDummy))]
class HiDummy : IDummy
{
    private string MyString { get;  }

    private HiDummy myDummy = new HiDummy("MyDummy");

    [Import]
    private IDummy dummy;

    public HiDummy([Import(AllowDefault = true)] string v = "AutoDummy")
    {
        MyString = v;
    }
    
    private IDummy getDummy()
    {
        return dummy ?? myDummy;
    }

    public void Print()
    {
        Console.WriteLine(((HiDummy)dummy).MyString);
    }
}

interface MyInterface
{

}

class GenericClass<T> : MyInterface
{
    public void print(T value)
    {
        Console.WriteLine("This is of type " + value.GetType());
    }
}

class StringGeneric : GenericClass<string>
{

}

class IntegerGeneric : GenericClass<int>
{

}

//Let us construct our zoo and animals
class TestMEF
{
    static void Main(string[] args)
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterType<StringGeneric>().AsImplementedInterfaces().SingleInstance().PreserveExistingDefaults();
        var builtContainer = containerBuilder.Build();

        GenericClass<string> genericClass = builtContainer.Resolve<MyInterface>() as GenericClass<string>;
        genericClass.print("asdf");
        GenericClass<int> intClass = (GenericClass<int>)builtContainer.Resolve<MyInterface>();
        intClass.print(123);

    }


    static void Main2(string[] args)
    {
        //Let us create a catalog and a container
        var catalog = new AssemblyCatalog(typeof(TestMEF).Assembly);
        var container = new CompositionContainer(catalog);

        container.GetExportedValue<IDummy>().Print();

        /*
        //Compose the zoo.
        var zoo = new Zoo();
        container.ComposeParts(zoo);

        //Let's feed our animals
        foreach (var animal in zoo.Animals)
            animal.Eat();*/

        Console.ReadKey();

    }
}