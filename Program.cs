using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Exclaimer.RPNGen
{
//# This module generates a sequence of Robustly Prime Numbers as defined in "Exclaimer C# Programming Test - Robustly Prime Numbers v1.1.pdf"
//#Date: 16/06/2017
//# Author: Milen Dimov
//# Comments: The application calculates all RPN numbers in advance and then asks the user to pick a numebr from that list.

    class Program
    {
        //Limit the number of RPNs generated as per requirement document
        private static int maxRPNs = 2209;
        private static List<uint> RPNsList = new List<uint>();
    
        //Main starting point for the app
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("### Welcome to Exclaimer Robustly Prime Number generator (RPNGen) ###");
            Console.WriteLine("Please wait until all numbers are loaded. Averge load time is 64s ...");

            //Get a timer to measure time elapsed
//            var watch = System.Diagnostics.Stopwatch.StartNew();

            //Use the number before the biggest 32 bit prime number. 
            RPNsList = AtkinParallel.FindPrimes(2147483563);

//            watch.Stop();
//            var elapsedMs = watch.ElapsedMilliseconds;
            GetUserInput();
        }

        //Prompt user for input
        private static void GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(String.Format("Please pick a sequence number between 1 and {0}: ", maxRPNs));          
            string userInput = Console.ReadLine();

            int n;
            if (int.TryParse(userInput, out n))
            {
                if (n > 0 && n <= maxRPNs)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(String.Format("The {0} th RPN number is: {1}", n, RPNsList[n-1]));
                    GetUserInput();
                }
                else
                    GetUserInput();
            }
        }
    }
}
