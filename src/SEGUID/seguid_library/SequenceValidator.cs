using SEGUID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SEGUID
{
    /// <summary>
    /// Validation utilities for SEGUID sequence operations.
    /// </summary>
    public static class SequenceValidator
    {
        /// <summary>
        /// Valid characters for sequences and alphabets.
        /// </summary>
        private static readonly HashSet<char> ValidChars = new HashSet<char>(
            Enumerable.Range('A', 26)            // A-Z 
                .Concat(Enumerable.Range('a', 26))  // a-z
                .Concat(Enumerable.Range('0', 10))  // 0-9
                .Select(x => (char)x)
                .Concat(new[] { '-', '\n', ';' })
        );

        /// <summary>
        /// Validates that a sequence only contains characters from the specified alphabet.
        /// </summary>
        /// <param name="seq">The sequence to validate</param>
        /// <param name="alphabet">The alphabet dictionary</param>
        /// <exception cref="ArgumentNullException">Thrown when arguments are null</exception>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        public static void AssertInAlphabet(string seq, Dictionary<char, string> alphabet)
        {
            if (seq == null)
                throw new ArgumentNullException(nameof(seq), "Argument 'seq' must be a string");
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet), "Argument 'alphabet' must be a dictionary");
            if (alphabet.Count == 0)
                throw new ArgumentException("Argument 'alphabet' must not be empty");

            // Validate alphabet characters
            var invalidChars = alphabet.Keys.Where(c => !ValidChars.Contains(c)).ToList();
            if (invalidChars.Any())
            {
                throw new ArgumentException(
                    $"Only A-Z a-z 0-9 -\\n; allowed. Invalid: {string.Join(" ", invalidChars)}"
                );
            }

            // Nothing to do for empty sequence
            if (seq.Length == 0)
                return;

            // Find unknown characters
            var unknownChars = seq.Where(c => !alphabet.ContainsKey(c)).Distinct().ToList();
            if (unknownChars.Any())
            {
                string missing = string.Join(" ", unknownChars.Select(c => Regex.Escape(c.ToString())));
                throw new ArgumentException($"Detected symbols {missing} not in the 'alphabet'");
            }
        }

        /// <summary>
        /// Validates that an alphabet is well-formed (all keys appear in values).
        /// </summary>
        /// <param name="alphabet">The alphabet to validate</param>
        /// <exception cref="ArgumentNullException">Thrown when alphabet is null</exception>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        public static void AssertAlphabet(Dictionary<char, string> alphabet)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet), "Argument 'alphabet' must be a dictionary");

            var keys = new HashSet<char>(alphabet.Keys);
            var values = new HashSet<char>();

            // Handle string values by splitting into characters
            foreach (var value in alphabet.Values)
            {
                foreach (char c in value)
                {
                    values.Add(c);
                }
            }

            // Check for keys not in values
            var unknown = keys.Where(k => !values.Contains(k)).ToList();
            if (unknown.Any())
            {
                string missing = string.Join(" ", unknown.Select(c => Regex.Escape(c.ToString())));
                throw new ArgumentException($"Detected keys ({missing}) in 'alphabet' that are not in the values");
            }
        }

        /// <summary>
        /// Validates that two strands are complementary according to the specified alphabet.
        /// </summary>
        /// <param name="watson">Watson strand</param>
        /// <param name="crick">Crick strand</param>
        /// <param name="alphabetSpec">Alphabet specification string</param>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        public static void AssertComplementary(string watson, string crick, string alphabetSpec)
        {
            // Get complementarity table
            var table = Alphabet.TableFactory(alphabetSpec);
            var keys = new HashSet<char>(table.Keys);

            // Validate alphabet
            var values = table.Values.ToList();
            if (values.Count <= 1)
                throw new ArgumentException("Was a single-stranded alphabet used by mistake? (values)");
            if (values[0].Length != 1)
                throw new ArgumentException("Was a single-stranded alphabet used by mistake? (value length)");

            AssertAlphabet(table);

            // Add gap character if not present
            if (!table.ContainsKey('-'))
            {
                table['-'] = "-";
                keys.Add('-');
            }

            // Validate sequence lengths
            if (watson.Length != crick.Length)
                throw new ArgumentException("Watson and Crick strands must be equal length");

            // Validate sequences against alphabet
            AssertInAlphabet(watson, keys.ToDictionary(k => k, k => ""));
            AssertInAlphabet(crick, keys.ToDictionary(k => k, k => ""));

            // Check complementarity
            string reverseCrick = SequenceManipulation.Reverse(crick);

            for (int i = 0; i < watson.Length; i++)
            {
                char w = watson[i];
                char c = reverseCrick[i];

                // Skip gaps
                if (w == '-' || c == '-')
                    continue;

                // Check complementarity
                string validPairs = table[w];
                if (!validPairs.Contains(c))
                {
                    throw new ArgumentException(
                        $"Non-complementary basepair ({w},{c}) detected at position {i + 1}"
                    );
                }
            }
        }
    }
}