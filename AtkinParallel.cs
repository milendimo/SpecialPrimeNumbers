
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exclaimer.RPNGen
{
    ///Description: 
    /// This class implements Sieve of Atkin in order to find all prime numbers below 2^31 -1 as per https://en.wikipedia.org/wiki/Sieve_of_Atkin
    /// Additionally it would check any number for zero digits and check contiguous sequence of prime numebrs in every number 
    /// Returns:
    /// A list with integers mathicng the rules above 
    public static class AtkinParallel
    {
            public static List<uint> FindPrimes(uint max)
            {
                //check for maximimum allowed number of items in array.
                //From MSDN: The maximum index in any single dimension byte array is 2,147,483,591
                if (max >= 2147483591)
                return new List<uint>();

                var isPrime = new bool[max + 1];
                var sqrt = (uint) Math.Sqrt(max);
                List<uint> RPNsList = new List<uint>();

                //mark number 2 and numebr 3 as primes.
                isPrime[2] = true;
                isPrime[3] = true;

                //implement parallel computation of the sieve
                //Higher the numbe of CPU cores the better the result.
                Parallel.For(1, sqrt, x =>
                {
                    var xx = x * x;
                    for (uint y = 1; y <= sqrt; y++)
                    {
                        var yy = y * y;
                        var n = 4 * xx + yy;
                        if (n <= max && (n % 12 == 1 || n % 12 == 5))
                            isPrime[n] ^= true;

                        n = 3 * xx + yy;
                        if (n <= max && n % 12 == 7)
                            isPrime[n] ^= true;

                        n = 3 * xx - yy;
                        if (x > y && n <= max && n % 12 == 11)
                            isPrime[n] ^= true;
                    }
                });

                for (uint n = 5; n <= sqrt; n++)
                {
                    if (isPrime[n])
                    {
                        uint nn = n * n;
                        for (uint k = nn; k <= max; k += nn)
                            isPrime[k] = false;

                    }
                }
                
                //Implement additional rules on top of generated prime numbers
                for (uint n = 0; n <= max; n++)
                    if (isPrime[n])
                    {
                    if (IsHavingZeros(n)){  
                        isPrime[n] = false;
                        }
                        else
                        {
                            //get contiguous sequence numbers in prime number and check if those are still prime  
                            uint numberCount = (uint)Math.Floor(Math.Log10(n) + 1);

                            uint extractedNumber = n % ((uint)Math.Pow(10, numberCount - 1));

                            if (extractedNumber>0)
                            {
                            if (!isPrime[extractedNumber])
                                isPrime[n] = false;
                            }
                        }

                        //if is in the bitarray by now, should be mathcing all the rule -> add it to the result list.
                        if (isPrime[n])
                            RPNsList.Add(n);
                    }

                return RPNsList;
            }


            //check if a numebr has a digit in it
            private static bool IsHavingZeros(long n)
            {
                //go through each digit and check if the reminder of a division of [10] is [0]. In case it is, the number contains a zero digit.
                while (n!=0){
                if (n % 10 == 0)
                return true;

                n/=10;
                }    
                return false;
            }
    }
}