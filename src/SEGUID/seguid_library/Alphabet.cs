using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEGUID
{
    /// <summary>
    /// Defines the alphabets used for sequence processing.
    /// </summary>
    public static class Alphabet
    {
        /// <summary>
        /// Predefined alphabets and their specifications.
        /// </summary>
        private static readonly Dictionary<string, string> Alphabets = new Dictionary<string, string>
        {
            {"{DNA}", "GC,AT"},
            {"{RNA}", "GC,AU"},
            {"{DNA-extended}", "GC,AT,BV,DH,KM,SS,RY,WW,NN"},
            {"{RNA-extended}", "GC,AU,BV,DH,KM,SS,RY,WW,NN"},
            {"{protein}", "A,C,D,E,F,G,H,I,K,L,M,N,P,Q,R,S,T,V,W,Y,O,U"},
            {"{protein-extended}", "A,C,D,E,F,G,H,I,K,L,M,N,P,Q,R,S,T,V,W,Y,O,U,B,J,X,Z"},
            {"{proteinV1}", "A,C,D,E,F,G,H,I,K,L,M,N,P,Q,R,S,T,V,W,Y"}
        };

        /// <summary>
        /// Creates a lookup table based on the provided alphabet specification.
        /// </summary>
        /// <param name="argument">The alphabet specification string</param>
        /// <returns>A dictionary mapping characters to their corresponding characters</returns>
        /// <exception cref="ArgumentException">Thrown when the specification is invalid</exception>
        public static Dictionary<char, string> TableFactory(string argument)
        {
            // Replace predefined alphabet names with their values
            foreach (var alphabet in Alphabets)
            {
                argument = argument.Replace(alphabet.Key, alphabet.Value);
            }

            var result = new Dictionary<char, string>();
            int expectedLength = -1;

            // Process each specification in the argument
            foreach (var spec in argument.Split(','))
            {
                int length = spec.Length;

                // Validate specification length
                if (expectedLength < 0)
                {
                    expectedLength = length;
                }
                else if (length != expectedLength)
                {
                    throw new ArgumentException($"Inconsistent specification length for '{spec}'");
                }

                // Process the specification based on its length
                switch (length)
                {
                    case 1:
                        result[spec[0]] = "";
                        break;

                    case 2:
                        char first = spec[0];
                        char second = spec[1];

                        // Handle first character
                        if (result.ContainsKey(first))
                        {
                            result[first] += second;
                        }
                        else
                        {
                            result[first] = second.ToString();
                        }

                        // Handle second character
                        if (result.ContainsKey(second))
                        {
                            result[second] += first;
                        }
                        else
                        {
                            result[second] = first.ToString();
                        }
                        break;

                    default:
                        throw new ArgumentException($"Unknown alphabet specification: {spec}");
                }
            }

            return result;
        }
    }
}