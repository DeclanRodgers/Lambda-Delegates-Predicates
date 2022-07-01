using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApp1
{
    internal class Program
    {


        public static void Main(string[] args)
        {
            //DemoEnumerable();
            //DemoEnumerableOfT();
            //DemoStatics();
            //DemoDeclaredDelegate();
            //DemoAnonymousDelegate();
            //DemoLambda();
            //DemoGenericDelegate();
            DemoLambdaExpression();
        }

        private static void DemoEnumerable() {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(1);
            arrayList.Add("Dec");

            foreach (var item in arrayList)
            {
                Console.WriteLine(item);
            }
        }

        private static void DemoEnumerableOfT() {
            List<string> namelist = new List<string>();
            List<int> numberList = new List<int>();
            List<MyThing> thingList = new List<MyThing>();

            namelist.Add("Dave");
            namelist.Add("Bob");
            namelist.Add("Jay");

            numberList.Add(17);
            numberList.Add(10);
            numberList.Add(1991);

            IEnumerable<string> stringEnumerable = namelist;
            IEnumerable<int> intEnumerable = numberList;
            IEnumerable<MyThing> thingEnumerable = thingList;

            foreach (string item in namelist) {
                Console.WriteLine(item);
            }

            foreach (int item in numberList) {
                Console.WriteLine(item);
            }

            stringEnumerable.GetEnumerator();

        }

        private static void DemoStatics() {
            MyThing thing = new MyThing();
            MyThing otherThing = new MyThing();
            MyThing.Counter++;
            Console.WriteLine(MyThing.Counter);

            OtherThing<string> stringThing = new OtherThing<string>();
            OtherThing<int> intThing = new OtherThing<int>();

            OtherThing<string>.Counter++;
            Console.WriteLine(OtherThing<string>.Counter);

            OtherThing<int>.Counter++;
            Console.WriteLine(OtherThing<int>.Counter);

            stringThing.DoThing();
            Console.WriteLine(OtherThing<string>.Counter);
        }

        private class MyThing
        {
            public static int Counter;
            private string property2;

            public string Property1 { get; set; }
            public string Property2
            {
                get { return property2; }
                set
                {
                    property2 = value;
                }
            }
        }

        private class OtherThing<T> {
            T Thing { get; set; }
            public static int Counter;

            public void DoThing() {
                Counter++;
            }
        }

        //Plug socket enalogy - this is the plug specification
        private delegate string SayHelloDelegate(string name); 
        private static void DemoDeclaredDelegate() {
            SayHelloDelegate sayHello = SayHelloInEnglish;
            Console.WriteLine(sayHello("Dec"));
            sayHello = SayHelloInFrench;
            Console.WriteLine(sayHello("Dec"));
            sayHello = SayHelloInEnglish;
            SayHelloToDeclan(sayHello);
        }

        private static string SayHelloInEnglish(string name) {
            return $"Hello {name}";
        }
        private static string SayHelloInFrench(string name) {
            return $"Bonjour {name}";
        }

        private static void SayHelloToDeclan(SayHelloDelegate sayHello) {
            Console.WriteLine(sayHello("Declan"));
        }

        private static void DemoAnonymousDelegate() {
            SayHelloToDeclan(delegate (string name) { return $"Awrite {name}";} );
            SayHelloToDeclan(delegate (string name) { return $"Howdy {name}"; });
            SayHelloToDeclan(delegate (string name) { return $"Greetings {name}"; });
        }

        //combines delegates with an anonymous function
        private static void DemoLambda() {
            SayHelloToDeclan(x => $"Awrite {x}");
            SayHelloToDeclan(name => $"Howdy {name}");
            SayHelloToDeclan(name => $"Greetings {name}");
        }

        private delegate bool IsValidDelegate<T>(T expression);

        private static string ShoutIfValid<T>(IsValidDelegate<T> isValid, T value) {
            return isValid(value) ? "Yay!" : "Boo!";
        }

        private static void DemoGenericDelegate() {
            Console.WriteLine(ShoutIfValid(x => x == 2, 2));
            Console.WriteLine(ShoutIfValid(x => x == 3, 2));
            Console.WriteLine(ShoutIfValid(x => x == 3, 3));
            Console.WriteLine(ShoutIfValid(x => x == "wibble", "wibble"));
            Console.WriteLine(ShoutIfValid(x => x == "monkey", "wibble"));

            MyThing thing = new MyThing { 
                Property1 = "wibble", 
                Property2 = "monkey"
            };

            Console.WriteLine(ShoutIfValid(t => t.Property1 == t.Property2, thing));
            Console.WriteLine(ShoutIfValid(t => t.Property1.Length == t.Property2.Length, thing));
            Console.WriteLine(ShoutIfValid(t => t.Property1[5] == t.Property2[4], thing));

            Action action1;
            Action<int> actionOfInt;
            Func<int> function;
            Func<int, int> functionOfInt;
            Func<int, int, string> functionOfThings;

            List<int> numberList = new List<int> {1,2,3,4,5};
            var result = numberList.Where(x => x>3).Select(x => $"Item {x}").ToList();
            var result2 = (from x in numberList
                          where x > 3
                          select $"Item {x}").ToList();
            
            Console.WriteLine();
            foreach (var item in result) {
                Console.WriteLine(item);
            }
            
            Console.WriteLine();
            foreach (var item in result2) {
                Console.WriteLine(item);
            }
        }

        private static string ShoutIfValid2<T>(Predicate<T> predicate, T value) {
            return predicate(value) ? "True" : "False";
        }

        private static void DemoLambdaExpression() {
            Console.WriteLine(Evaluate(() => "2"));
            //Console.WriteLine(EvaluateExpression(() => 6+12));

            int x = 6;
            int y = 12;
            Console.WriteLine(EvaluateExpression(() => x+y));
        }

        private static T Evaluate<T>(Func<T> expression) {
            return expression();   
        }

        private static T EvaluateExpression<T>(Expression<Func<T>> expression) {
            var compiled = expression.Compile();
            return compiled();
        }
    }
}