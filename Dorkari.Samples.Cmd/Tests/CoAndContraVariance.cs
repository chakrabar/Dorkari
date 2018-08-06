using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Tests
{
    //T is COvariant
    public interface IGetValue<out TCo>
    {
        TCo GetValue();
    }

    //T is CONTRAvariant
    public interface ISetValue<in TContra>
    {
        void SetValue(TContra data);
    }

    public class Animal
    { }

    public class Mammal : Animal
    { }

    public class Human : Mammal
    { }

    public class CoAndContraVariance : IGetValue<Mammal>, ISetValue<Mammal>
    {
        public Mammal GetValue()
        {
            return default(Mammal);
        }

        public void SetValue(Mammal data)
        {
            Mammal temp = data;
        }
    }

    public class TestCoAndContraVariance
    {
        CoAndContraVariance _mammals = new CoAndContraVariance();

        // >>> FRAMEWORK exmaples <<<
        public void TestInFramework()
        {
            //COvariance - out T in IEnumerable
            var humans = new List<Human>();
            IEnumerable<Mammal> mammals = humans;
            //IEnumerable<Mammal> mammals2 = new List<Animal>(); //doesn't compile

            //CONTRAvariance - in T in Action
            Action<Animal> animalFunc = (ani) => { };
            Action<Human> humanFunc = (hum) => { };

            Action<Mammal> mammalFunc = animalFunc;
            //Action<Mammal> mammalFunc2 = humanFunc; //doesn't compile
        }

        public void TestOurs()
        {
            //our contrived examples
            Animal animal = _mammals.GetValue(); //covariance, kind of
            Action<Human> confusion = _mammals.SetValue; //contravariance!!
        }
    }
}
