using System;
using System.Text.RegularExpressions;

namespace SEGUID
{
    /// <summary>
    /// Utilities for handling DNA/RNA sequence representations, particularly for double-stranded sequences.
    /// </summary>
    public class SequenceUtils
    {
        /// <summary>
        /// Represents the result of parsing a sequence string.
        /// </summary>
        public class SequenceParseResult
        {
            public string Type { get; set; }        // "ss" or "ds"
            public string Specification { get; set; }
            public string Watson { get; set; }
            public string Crick { get; set; }

            public SequenceParseResult(string type, string spec, string? watson = null, string? crick = null)
            {
                if (string.IsNullOrEmpty(type))
                {
                    throw new ArgumentException($"'{nameof(type)}' cannot be null or empty.", nameof(type));
                }
                if (string.IsNullOrEmpty(spec))
                {
                    throw new ArgumentException($"'{nameof(spec)}' cannot be null or empty.", nameof(spec));
                }
                if (string.IsNullOrEmpty(watson))
                {
                    throw new ArgumentException($"'{nameof(watson)}' cannot be null or empty.", nameof(watson));
                }
                if (string.IsNullOrEmpty(crick))
                {
                    throw new ArgumentException($"'{nameof(crick)}' cannot be null or empty.", nameof(crick));
                }

                Type = type;
                Specification = spec;
                Watson = watson;
                Crick = crick;
            }
        }

        /// <summary>
        /// Checks if either strand contains gaps (indicated by '-').
        /// </summary>
        /// <param name="watson">Watson strand</param>
        /// <param name="crick">Crick strand</param>
        /// <returns>True if either strand contains gaps</returns>
        public static bool IsStaggered(string watson, string crick)
        {
            return watson.Contains("-") || crick.Contains("-");
        }

        /// <summary>
        /// Escapes newlines in sequence specifications for error messages.
        /// </summary>
        /// <param name="spec">Sequence specification</param>
        /// <returns>Escaped sequence specification</returns>
        public static string EscapeSequenceSpec(string spec)
        {
            return spec.Replace("\n", "\\n");
        }

        /// <summary>
        /// Reverses a DNA/RNA sequence string.
        /// </summary>
        /// <param name="sequence">The sequence to reverse</param>
        /// <returns>Reversed sequence</returns>
        private static string ReverseSequence(string sequence)
        {
            char[] charArray = sequence.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Parses various sequence string formats.
        /// </summary>
        /// <param name="spec">The sequence specification to parse</param>
        /// <returns>A SequenceParseResult containing sequence type and strands</returns>
        /// <exception cref="ArgumentNullException">Thrown when spec is null</exception>
        /// <exception cref="ArgumentException">Thrown when spec format is invalid</exception>
        public static SequenceParseResult ParseSequenceString(string spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec), "Argument must be a string");

            // Single-stranded sequence
            if (Regex.IsMatch(spec, @"^[0-9A-Za-z-]+$"))
            {
                return new SequenceParseResult("ss", spec);
            }

            // Watson-Crick pair separated by semicolon
            Match semicolonMatch = Regex.Match(spec, @"^([0-9A-Za-z-]+);([0-9A-Za-z-]+)$");
            if (semicolonMatch.Success)
            {
                string watson = semicolonMatch.Groups[1].Value;
                string crick = semicolonMatch.Groups[2].Value;
                string rcrick = ReverseSequence(crick);

                ValidateStrands(watson, crick, spec);

                return new SequenceParseResult("ds", spec, watson, crick);
            }

            // Watson-Crick pair separated by newline
            Match newlineMatch = Regex.Match(spec, @"^([0-9A-Za-z-]+)\n([0-9A-Za-z-]+)$");
            if (newlineMatch.Success)
            {
                string watson = newlineMatch.Groups[1].Value;
                string rcrick = newlineMatch.Groups[2].Value;
                string crick = ReverseSequence(rcrick);

                ValidateStrands(watson, crick, spec);

                return new SequenceParseResult("ds", spec, watson, crick);
            }

            // If we get here, the format is invalid
            throw new ArgumentException($"Syntax error in sequence string: '{EscapeSequenceSpec(spec)}'");
        }

        /// <summary>
        /// Validates the Watson and Crick strands for length and staggering.
        /// </summary>
        private static void ValidateStrands(string watson, string crick, string spec)
        {
            // Length validation
            if (watson.Length != crick.Length)
            {
                throw new ArgumentException(
                    $"Double-strand sequence string specifies two strands of different lengths ({watson.Length} != {crick.Length}): '{EscapeSequenceSpec(spec)}'"
                );
            }

            // Stagger validation
            if (IsStaggered(watson, crick))
            {
                string rcrick = ReverseSequence(crick);

                if (watson.StartsWith("-") && rcrick.StartsWith("-"))
                {
                    throw new ArgumentException(
                        $"Please trim the staggering. Watson and Crick are both staggered at the beginning of the double-stranded sequence: '{EscapeSequenceSpec(spec)}'"
                    );
                }
                if (watson.EndsWith("-") && rcrick.EndsWith("-"))
                {
                    throw new ArgumentException(
                        $"Please trim the staggering. Watson and Crick are both staggered at the end of the double-stranded sequence: '{EscapeSequenceSpec(spec)}'"
                    );
                }
            }
        }
    }
}