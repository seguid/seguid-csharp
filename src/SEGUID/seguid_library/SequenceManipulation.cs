using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEGUID
{
    /// <summary>
    /// Provides utilities for manipulating sequence strings.
    /// </summary>
    public static class SequenceManipulation
    {
        /// <summary>
        /// Rotates a sequence by a specified amount.
        /// </summary>
        /// <param name="seq">The sequence to rotate</param>
        /// <param name="amount">The amount to rotate by (default 0)</param>
        /// <returns>The rotated sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown when seq is null</exception>
        /// <exception cref="ArgumentException">Thrown when amount is not a valid integer</exception>
        public static string Rotate(string seq, int amount = 0)
        {
            if (seq == null)
                throw new ArgumentNullException(nameof(seq), "Argument 'seq' must be a string");

            // Nothing to rotate?
            if (seq.Length == 0)
                return seq;

            // Normalize amount to sequence length
            amount = ((amount % seq.Length) + seq.Length) % seq.Length;

            // Rotate?
            if (amount > 0)
            {
                return seq.Substring(amount) + seq.Substring(0, amount);
            }
            return seq;
        }

        /// <summary>
        /// Reverses a sequence string.
        /// </summary>
        /// <param name="seq">The sequence to reverse</param>
        /// <returns>The reversed sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown when seq is null</exception>
        public static string Reverse(string seq)
        {
            if (seq == null)
                throw new ArgumentNullException(nameof(seq), "Argument 'seq' must be a string");

            return new string(seq.Reverse().ToArray());
        }



        /// <summary>
        /// Reverse complement of a DNA sequence.
        /// </summary>
        /// <param name="seq">The sequence to generate reverse complement</param>
        /// <returns>The reversed complement sequence (5' -> 3')</returns>
        /// <exception cref="ArgumentNullException">Thrown when seq is null</exception>
        public static string ReverseComplementDNA(string seq)
        {
            if (seq == null)
                throw new ArgumentNullException(nameof(seq), "Argument 'seq' must be a string");

            // Convert to uppercase and remove invalid characters
            seq = seq.ToUpper();
            seq = Regex.Replace(seq, "[^AGCT]", "");

            if (string.IsNullOrEmpty(seq))
                throw new ArgumentException("A protein sequence must not be empty");

            StringBuilder reverseComplement = new StringBuilder(seq.Length);
            for (int i = seq.Length - 1; i >= 0; i--)
            {
                switch (seq[i])
                {
                    case 'A':
                        reverseComplement.Append('T');
                        break;
                    case 'T':
                        reverseComplement.Append('A');
                        break;
                    case 'G':
                        reverseComplement.Append('C');
                        break;
                    case 'C':
                        reverseComplement.Append('G');
                        break;
                }
            }
            return reverseComplement.ToString();
        }



        /// <summary>
        /// Finds the minimum rotation point of a sequence
        /// </summary>
        /// <param name="s">The sequence to analyze</param>
        /// <returns>The rotation amount that gives the minimum sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown when s is null</exception>
        public static int MinRotation(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s), "Argument must be a string");
            int rep = 0;
            string doubledString = s + s;
            char[] ds = doubledString.ToCharArray();
            int lens = s.Length;
            int lends = lens * 2;
            int old = 0;
            int k = 0;
            string w = "";

            while (k < lends)
            {
                int i = k;
                int j = k + 1;

                while (j < lends && ds[i] <= ds[j])
                {
                    i = (ds[i] == ds[j]) ? i + 1 : k;
                    j++;
                }

                while (k < i + 1)
                {
                    k += j - i;
                    string prev = w;
                    w = new string(ds.Skip(old).Take(k - old).ToArray());
                    old = k;

                    if (w == prev)
                    {
                        rep++;
                    }
                    else
                    {
                        prev = w;
                        rep = 1;
                    }

                    if (w.Length * rep == lens)
                    {
                        return old - i;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Rotates a sequence to its minimum representation.
        /// </summary>
        /// <param name="s">The sequence to rotate</param>
        /// <returns>The minimally rotated sequence</returns>
        /// <exception cref="InvalidOperationException">Thrown when ASCII ordering assumption is violated</exception>
        public static string RotateToMin(string s)
        {
            // Ensure uppercase letters are ordered before lowercase letters
            if (MinRotation("Aa") != 0)
                throw new InvalidOperationException("ASCII ordering assumption violated");

            int amount = MinRotation(s);
            return Rotate(s, amount);
        }
    }
}