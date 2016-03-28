using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleLibrary;

namespace Deflector.Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetAssembly = typeof(SampleClassWithInstanceMethod).Assembly;

            // Notice how modifying the sample assembly requires only one line of code
            var modifiedAssembly = targetAssembly.AddInterceptionHooks();

            // Replace the DoSomethingElse() method call with a different method call
            Replace.Method<SampleClassThatCallsAnInstanceMethod>(c => c.DoSomethingElse())
                .With(() =>
                {
                    Console.WriteLine("DoSomethingElse() method call intercepted");
                });

            var targetTypeName = nameof(SampleClassThatCallsAnInstanceMethod);

            // Use the createInstance lambda for syntactic sugar
            Func<string, dynamic> createInstance = typeName =>
            {
                var targetType = modifiedAssembly.GetTypes().First(t => t.Name == typeName);
                return Activator.CreateInstance(targetType);
            };

            // Create the modified type instance
            dynamic targetInstance = createInstance(targetTypeName);

            targetInstance.DoSomething();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
