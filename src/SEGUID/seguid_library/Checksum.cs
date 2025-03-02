using SEGUID;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SEGUID
{
    /// <summary>
    /// Provides methods for generating SEGUID checksums for various DNA sequence types.
    /// </summary>
    public static class Checksum
    {
        private const string SeguidPrefix = "seguid=";
        private const string LsSeguidPrefix = "lsseguid=";
        private const string CsSeguidPrefix = "csseguid=";
        private const string LdSeguidPrefix = "ldseguid=";
        private const string CdSeguidPrefix = "cdseguid=";
        private const string SeguidV1Prefix = "seguidv1=";
        private const string SeguidV1UrlSafePrefix = "seguidv1urlsafe=";
        private const int ShortLength = 6;

        private static readonly HashSet<char> Base64Alphabet = new HashSet<char>(
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/_-"
        );

        /// <summary>
        /// Internal function for SEGUID generation.
        /// </summary>
        private static string GenerateSeguid(string seq, string alphabet, bool useUrlSafe)
        {
            if (string.IsNullOrEmpty(seq))
                throw new ArgumentException("A sequence must not be empty");

            // Get alphabet and validate sequence
            var table = Alphabet.TableFactory(alphabet);
            SequenceValidator.AssertInAlphabet(seq, table.Keys.ToDictionary(k => k, _ => ""));

            // Generate SHA1 hash
            byte[] hash;
            using (var sha1 = SHA1.Create())
            {
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(seq));
            }

            // Encode in base64
            string encoded = useUrlSafe
                ? Convert.ToBase64String(hash).Replace('+', '-').Replace('/', '_').TrimEnd('=')
                : Convert.ToBase64String(hash).TrimEnd('=');

            // Validate
            if (encoded.Length != 27)
                throw new InvalidOperationException("Invalid checksum length");

            if (encoded.Any(c => !Base64Alphabet.Contains(c)))
                throw new InvalidOperationException($"Invalid base64 character");

            return encoded;
        }

        /// <summary>
        /// Internal function for SEGUIDv1 generation.
        /// </summary>
        private static string GenerateSeguidV1(string seq, string alphabet, bool useUrlSafe)
        {
            if (string.IsNullOrEmpty(seq))
                throw new ArgumentException("A sequence must not be empty");

            // Convert to uppercase and remove invalid characters
            seq = seq.ToUpper();
            seq = Regex.Replace(seq, "[^ACDEFGHIKLMNPQRSTVWY]", "");

            if (string.IsNullOrEmpty(seq))
                throw new ArgumentException("A protein sequence must not be empty");

            return GenerateSeguid(seq, alphabet, useUrlSafe);
        }

        /// <summary>
        /// Formats the checksum according to the specified form.
        /// </summary>
        private static string FormatChecksum(string prefix, string checksum, string form)
        {
            switch (form?.ToLower())
            {
                case "both":
                    return $"{checksum.Substring(0, ShortLength)},{prefix}{checksum}";
                case "long":
                    return $"{prefix}{checksum}";
                case "short":
                    return checksum.Substring(0, ShortLength);
                default:
                    throw new ArgumentException($"Invalid form: {form}");
            }
        }

        public static string Seguid(string seq, string alphabet = "{DNA}", string form = "long")
        {
            return FormatChecksum(
                SeguidPrefix,
                GenerateSeguid(seq, alphabet, false),
                form
            );
        }

        public static string SeguidV1(string seq, string alphabet = "{proteinV1}", string form = "long")
        {
            return FormatChecksum(
                SeguidV1Prefix,
                GenerateSeguidV1(seq, alphabet, false),
                form
            );
        }

        public static string SeguidV1UrlSafe(string seq, string alphabet = "{proteinV1}", string form = "long")
        {
            return FormatChecksum(
                SeguidV1UrlSafePrefix,
                GenerateSeguidV1(seq, alphabet, true),
                form
            );
        }

        public static string LsSeguid(string seq, string alphabet = "{DNA}", string form = "long")
        {
            return FormatChecksum(
                LsSeguidPrefix,
                GenerateSeguid(seq, alphabet, true),
                form
            );
        }

        public static string CsSeguid(string seq, string alphabet = "{DNA}", string form = "long")
        {
            return FormatChecksum(
                CsSeguidPrefix,
                GenerateSeguid(SequenceManipulation.RotateToMin(seq), alphabet, true),
                form
            );
        }

        public static string LdSeguid(string watson, string crick, string alphabet = "{DNA}", string form = "long")
        {
            if (string.IsNullOrEmpty(watson))
                throw new ArgumentException("Watson sequence must not be empty");
            if (string.IsNullOrEmpty(crick))
                throw new ArgumentException("Crick sequence must not be empty");
            if (watson.Length != crick.Length)
                throw new ArgumentException("Sequences must be equal length");

            SequenceValidator.AssertComplementary(watson, crick, alphabet);

            var table = Alphabet.TableFactory(alphabet);
            if (table.Count <= 1)
                throw new ArgumentException("Was a single-stranded alphabet used by mistake?");

            string exalphabet = $"{alphabet},--,;;";

            // Create spec string based on lexicographic ordering
            string spec = string.CompareOrdinal(watson, crick) < 0
                ? $"{watson};{crick}"
                : $"{crick};{watson}";

            return FormatChecksum(
                LdSeguidPrefix,
                GenerateSeguid(spec, exalphabet, true),
                form
            );
        }

        public static string CdSeguid(string watson, string crick, string alphabet = "{DNA}", string form = "long")
        {
            if (string.IsNullOrEmpty(watson))
                throw new ArgumentException("Watson sequence must not be empty");
            if (string.IsNullOrEmpty(crick))
                throw new ArgumentException("Crick sequence must not be empty");
            if (watson.Length != crick.Length)
                throw new ArgumentException("Sequences must be equal length");

            SequenceValidator.AssertComplementary(watson, crick, alphabet);

            const string concatConnector = "TTTT";
            string concatenated = watson + concatConnector + crick;
            int minRotationConcat = SequenceManipulation.MinRotation(concatenated);

            string w, c;
            if (minRotationConcat < watson.Length)
            {
                // Watson
                int ind = minRotationConcat;
                w = SequenceManipulation.Rotate(watson, ind);
                c = SequenceManipulation.Rotate(crick, crick.Length - ind);
            }
            else
            {
                // Crick
                int ind = minRotationConcat - watson.Length - concatConnector.Length;
                w = SequenceManipulation.Rotate(crick, ind);
                c = SequenceManipulation.Rotate(watson, watson.Length - ind);
            }

            string result = LdSeguid(w, c, alphabet, "long");
            return FormatChecksum(
                CdSeguidPrefix,
                result.Substring(LdSeguidPrefix.Length),
                form
            );
        }


        public static string CcSeguid(string watson,  string form = "long")
        {
            if (string.IsNullOrEmpty(watson))
                throw new ArgumentException("Watson sequence must not be empty");

            string alphabet = "{DNA}";
            watson = watson.ToUpper();

            string crick = SequenceManipulation.ReverseComplementDNA(watson);

            SequenceValidator.AssertComplementary(watson, crick, alphabet);


            string reversedWatson = SequenceManipulation.Reverse(watson);
            string reversedCrick = SequenceManipulation.Reverse(crick);

            SequenceValidator.AssertComplementary(reversedWatson, reversedCrick, alphabet);


            const string concatConnector = "TTTT";
            string concatenated = watson + concatConnector + crick;
            int minRotationConcat = SequenceManipulation.MinRotation(concatenated);

            string w, c;
            if (minRotationConcat < watson.Length)
            {
                // Watson
                int ind = minRotationConcat;
                w = SequenceManipulation.Rotate(watson, ind);
                c = SequenceManipulation.Rotate(crick, crick.Length - ind);
            }
            else if(minRotationConcat < watson.Length + concatConnector.Length + crick.Length)
            {
                // Crick
                int ind = minRotationConcat - (watson.Length + concatConnector.Length);
                w = SequenceManipulation.Rotate(crick, ind);
                c = SequenceManipulation.Rotate(watson, watson.Length - ind);
            }
            else if (minRotationConcat < watson.Length + concatConnector.Length + crick.Length + concatConnector.Length + reversedWatson.Length)
            {
                // reversedWatson
                int ind = minRotationConcat - (watson.Length + concatConnector.Length + crick.Length + concatConnector.Length);
                w = SequenceManipulation.Rotate(reversedWatson, ind);
                c = SequenceManipulation.Rotate(reversedCrick, reversedCrick.Length - ind);
            }
            else
            {
                // reversedCrick
                int ind = minRotationConcat - (watson.Length + concatConnector.Length + crick.Length + concatConnector.Length + reversedWatson.Length + concatConnector.Length);
                w = SequenceManipulation.Rotate(reversedCrick, ind);
                c = SequenceManipulation.Rotate(reversedWatson, reversedWatson.Length - ind);
            }

            string result = LdSeguid(w, c, alphabet, "long");
            return FormatChecksum(
                CcSeguidPrefix,
                result.Substring(LdSeguidPrefix.Length),
                form
            );
        }



    }
}