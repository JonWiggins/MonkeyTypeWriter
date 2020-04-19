using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyTypeWriter
{
    class Program
    {
        private const string DEFAULT_PHRASE = "tobeornottobe";
        static void Main(string[] args)
        {
            Console.Write("Enter a target phrase:");
            string target = Console.ReadLine();

            if (target.Equals(""))
                target = DEFAULT_PHRASE;

            Console.Write("Limit chars to phrase? (y/n):");
            string reply = Console.ReadLine();
            bool trainingWheels = false;
            if (reply.Contains("y"))
                trainingWheels = true;

            new MonkeyTypeWriter(target, trainingWheels);
        }
    }

    class MonkeyTypeWriter
    {
        char[] Target;
        public MonkeyTypeWriter(string targetPhrase, bool trainingWheels)
        {
            this.Target = targetPhrase.ToCharArray();
            if (trainingWheels)
                TrainingWheelsKeyboard();
            else
                FullKeyboard();
        }

        public void TrainingWheelsKeyboard()
        {
            this.Run(this.Target);
        }

        public void FullKeyboard()
        {
            this.Run(Enumerable.Range((Int32)'A', 26)
                        .SelectMany(c => new[] { (Char)c, (Char)(c + 'a' - 'A' )})
                .ToArray());
        }

        private void Run(char[] keys)
        {
            Random rng = new Random();
            bool hasCompleted = false;

            long attemptCounter = 0;
            StringBuilder typingbuffer = new StringBuilder();

            Console.WriteLine("Key Presses\t\tBuffer");
            while (!hasCompleted)
            {
                char next = Convert.ToChar(keys[rng.Next(0, keys.Length)]);
                typingbuffer.Append(next);
                if (typingbuffer.Length > this.Target.Length)
                    typingbuffer.Remove(0, 1);

                if (IsTarget(typingbuffer))
                    hasCompleted = true;

                attemptCounter++;
                if (attemptCounter % 1_000_000 == 0)
                    Console.WriteLine(String.Format("{0:n0}", attemptCounter) + "\t" + typingbuffer.ToString());
            }

            Console.WriteLine("It worked! After " + attemptCounter + " key presses, the monkey wrote: " + this.Target.ToString());
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private bool IsTarget(StringBuilder Current)
        {
            if (Current.Length != this.Target.Length)
                return false;
            for (int index = 0; index < Current.Length; index++)
            {
                if (Current[index] != this.Target[index])
                    return false;
            }
            return true;
        }
    }
}
