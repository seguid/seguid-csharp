using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using static NUnit.Framework.Assert;  // Using Assert methods directly

namespace SEGUID.Tests
{
    [TestFixture]
    public class ChecksumTests
    {
        /// <summary>
        /// Helper method to verify checksum format
        /// </summary>
        private bool IsValidChecksum(string checksum, string prefix, bool isLong)
        {
            if (isLong)
            {
                // Long form should be prefix + 27 base64 chars
                return Regex.IsMatch(checksum, $@"^{prefix}[A-Za-z0-9+\/_-]{{27}}$");
            }
            // Short form should be exactly 6 chars
            return Regex.IsMatch(checksum, @"^[A-Za-z0-9+\/_-]{6}$");
        }

        [Test]
        public void TestBasicSequencesWithSeguid()
        {
            // Basic DNA sequence
            string seq = "ACGT";
            string seqRNA = "ACGU";
            string checksum = Checksum.Seguid(seq);
            That(IsValidChecksum(checksum, "seguid=", true), Is.True, "Valid v1 seguid format");

            // Test form parameter
            string shortForm = Checksum.Seguid(seq, "{DNA}", "short");
            That(IsValidChecksum(shortForm, "", false), Is.True, "Valid short seguid format");

            // Test specific checksums
            That(Checksum.Seguid("AT"), Is.EqualTo("seguid=Ax/RG6hzSrMEEWoCO1IWMGska+4"), "Correct seguid");

            // Testing seguidv1
            string seqV1 = "ACDEFGHIKLMNPQRSTVXY";

            DoesNotThrow(() => Checksum.SeguidV1(seqV1, "{proteinV1}"), "Protein alphabet (V1) accepted");
            var ex = Throws<ArgumentException>(() => Checksum.SeguidV1("BOUZ", "{proteinV1}"));
            That(ex.Message, Does.Contain("protein sequence must not be empty"));

            string longV1 = Checksum.SeguidV1(seqV1, "{proteinV1}", "long");

            // Test specific V1 checksums
            That(Checksum.SeguidV1("ACDEFGHIKLMNPQRSTVWY"), Is.EqualTo("seguidv1=tRntFmqHM23Z+bMbNDfKXFC1+Es"), "Correct seguidv1");
            That(Checksum.SeguidV1UrlSafe("ACDEFGHIKLMNPQRSTVWY"), Is.EqualTo("seguidv1urlsafe=tRntFmqHM23Z-bMbNDfKXFC1-Es"), "Correct seguidv1urlsafe");

            // Additional V1 test cases
            That(Checksum.SeguidV1("MGDRSEGPGPTRPGPPGIGP"), Is.EqualTo("seguidv1=N/DxuiQwt3rU+nDzU5/q+CaRuQM"), "Correct seguidv1");
            That(Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGP"), Is.EqualTo("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM"), "Correct seguidv1urlsafe");
            That(Checksum.SeguidV1UrlSafe("mgDRSEGpgpTRPGPPGigp"), Is.EqualTo("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM"), "Correct seguidv1urlsafe with mixed case");
            That(Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGPB"), Is.EqualTo("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM"), "Correct seguidv1urlsafe with extra B");
            That(Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGPO"), Is.EqualTo("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM"), "Correct seguidv1urlsafe with extra O");
            That(Checksum.SeguidV1UrlSafe("12_*!@#$%^&MGDRSEGP GPTRPGPP  -GIGPO"), Is.EqualTo("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM"), "Correct seguidv1urlsafe with special chars");
        }

        [Test]
        public void TestLinearSingleStrandedSequences()
        {
            string seq = "ACGT";
            string seqRNA = "ACGUBVDHKMSRYWN";

            string checksum = Checksum.LsSeguid(seq);
            That(IsValidChecksum(checksum, "lsseguid=", true), Is.True, "Valid lsseguid format");

            // Test different forms
            string shortForm = Checksum.LsSeguid(seq, "{DNA}", "short");
            That(IsValidChecksum(shortForm, "", false), Is.True, "Valid short lsseguid format");

            // Test alphabets
            DoesNotThrow(() => Checksum.LsSeguid(seq, "{DNA-extended}"), "Extended DNA alphabet accepted");
            DoesNotThrow(() => Checksum.LsSeguid(seqRNA, "{RNA-extended}"), "Extended RNA alphabet accepted");
        }

        [Test]
        public void TestCircularSingleStrandedSequences()
        {
            string seq = "ACGT";
            string checksum = Checksum.CsSeguid(seq);
            That(IsValidChecksum(checksum, "csseguid=", true), Is.True, "Valid csseguid format");

            // Test rotations return same checksum
            string rotated = Checksum.CsSeguid("GTAC");
            That(rotated, Is.EqualTo(checksum), "Rotated sequence produces same checksum");

            // Test different forms
            string shortForm = Checksum.CsSeguid(seq, "{DNA}", "short");
            That(IsValidChecksum(shortForm, "", false), Is.True, "Valid short csseguid format");
        }

        [Test]
        public void TestLinearDoubleStrandedSequences()
        {
            string watson = "-TATGCC";
            string crick = "-GCATAC";

            string checksum = Checksum.LdSeguid(watson, crick);
            That(IsValidChecksum(checksum, "ldseguid=", true), Is.True, "Valid ldseguid format");

            // Test different forms
            string shortForm = Checksum.LdSeguid(watson, crick, "{DNA}", "short");
            That(IsValidChecksum(shortForm, "", false), Is.True, "Valid short ldseguid format");

            // Test error conditions
            var ex = Throws<ArgumentException>(() => Checksum.LdSeguid(watson, crick.Substring(0, crick.Length - 1)));
            That(ex.Message, Does.Contain("must be equal length"));
        }

        [Test]
        public void TestCircularDoubleStrandedSequences()
        {
            string watson = "GTATGCC";
            string crick = "GGCATAC";

            string checksum = Checksum.CdSeguid(watson, crick);
            That(IsValidChecksum(checksum, "cdseguid=", true), Is.True, "Valid cdseguid format");

            // Test rotations return same checksum
            string rotated = Checksum.CdSeguid(watson, crick);
            That(rotated, Is.EqualTo(checksum), "Rotated sequence produces same checksum");

            // Test different forms
            string shortForm = Checksum.CdSeguid(watson, crick, "{DNA}", "short");
            That(IsValidChecksum(shortForm, "", false), Is.True, "Valid short cdseguid format");

            // Additional test cases
            watson = "TTGGCATA";
            crick = "TATGCCAA";
            checksum = Checksum.CdSeguid(watson, crick);
            That(IsValidChecksum(checksum, "cdseguid=", true), Is.True, "Valid cdseguid format");

            watson = "AATATGCC";
            crick = "GGCATATT";
            checksum = Checksum.CdSeguid(watson, crick);
            That(IsValidChecksum(checksum, "cdseguid=", true), Is.True, "Valid cdseguid format");

            // Test error conditions
            // addgene-plasmid-64953-sequence-121770
            watson = "tcgcgcgtttcggtgatgacggtgaaaacctctgacacatgcagctcccggagacggtcacagcttgtctgtaagcggatgccgggagcagacaagcccgtcagggcgcgtcagcgggtgttggcgggtgtcggggctggcttaactatgcggcatcagagcagattgtactgagagtgcaccatatgcggtgtgaaataccgcacagatgcgtaaggagaaaataccgcatcaggcgccattcgccattcaggctgcgcaactgttgggaagggcgatcggtgcgggcctcttcgctattacgccagctagaggaccagccgcgtaacctggcaaaatcggttacggttgagtaataaatggatgccctgcgtaagcgggtgtgggcggacaataaagtcttaaactgaacaaaatagatctaaactatgacaataaagtcttaaactagacagaatagttgtaaactgaaatcagtccagttatgctgtgaaaaagcatactggacttttgttatggctaaagcaaactcttcattttctgaagtgcaaattgcccgtcgtattaaagaggggcgtggggtcgacgatatcatgcatgagctcactagtggatcccccgggctgcaggaattcctcgagaagcttgggcccggtacctcgcgaaggccttgcaggccaaccagataagtgaaatctagttccaaactattttgtcatttttaattttcgtattagcttacgacgctacacccagttcccatctattttgtcactcttccctaaataatccttaaaaactccatttccacccctcccagttcccaactattttgtccgcccacagcggggcatttttcttcctgttatgtttgggcgctgcattaatgaatcggccaacgcgcggggagaggcggtttgcgtattgggcgctcttccgcttcctcgctcactgactcgctgcgctcggtcgttcggctgcggcgagcggtatcagctcactcaaaggcggtaatacggttatccacagaatcaggggataacgcaggaaagaacatgtgagcaaaaggccagcaaaaggccaggaaccgtaaaaaggccgcgttgctggcgtttttccataggctccgcccccctgacgagcatcacaaaaatcgacgctcaagtcagaggtggcgaaacccgacaggactataaagataccaggcgtttccccctggaagctccctcgtgcgctctcctgttccgaccctgccgcttaccggatacctgtccgcctttctcccttcgggaagcgtggcgctttctcatagctcacgctgtaggtatctcagttcggtgtaggtcgttcgctccaagctgggctgtgtgcacgaaccccccgttcagcccgaccgctgcgccttatccggtaactatcgtcttgagtccaacccggtaagacacgacttatcgccactggcagcagccactggtaacaggattagcagagcgaggtatgtaggcggtgctacagagttcttgaagtggtggcctaactacggctacactagaaggacagtatttggtatctgcgctctgctgaagccagttaccttcggaaaaagagttggtagctcttgatccggcaaacaaaccaccgctggtagcggtggtttttttgtttgcaagcagcagattacgcgcagaaaaaaaggatctcaagaagatcctttgatcttttctacggggtctgacgctcagtggaacgaaaactcacgttaagggattttggtcatgagattatcaaaaaggatcttcacctagatccttttaaattaaaaatgaagttttaaatcaatctaaagtatatatgagtaaacttggtctgacagttaccaatgcttaatcagtgaggcacctatctcagcgatctgtctatttcgttcatccatagttgcctgactccccgtcgtgtagataactacgatacgggagggcttaccatctggccccagtgctgcaatgataccgcgagacccacgctcaccggctccagatttatcagcaataaaccagccagccggaagggccgagcgcagaagtggtcctgcaactttatccgcctccatccagtctattaattgttgccgggaagctagagtaagtagttcgccagttaatagtttgcgcaacgttgttgccattgctacaggcatcgtggtgtcacgctcgtcgtttggtatggcttcattcagctccggttcccaacgatcaaggcgagttacatgatcccccatgttgtgcaaaaaagcggttagctccttcggtcctccgatcgttgtcagaagtaagttggccgcagtgttatcactcatggttatggcagcactgcataattctcttactgtcatgccatccgtaagatgcttttctgtgactggtgagtactcaaccaagtcattctgagaatagtgtatgcggcgaccgagttgctcttgcccggcgtcaatacgggataataccgcgccacatagcagaactttaaaagtgctcatcattggaaaacgttcttcggggcgaaaactctcaaggatcttaccgctgttgagatccagttcgatgtaacccactcgtgcacccaactgatcttcagcatcttttactttcaccagcgtttctgggtgagcaaaaacaggaaggcaaaatgccgcaaaaaagggaataagggcgacacggaaatgttgaatactcatactcttcctttttcaatattattgaagcatttatcagggttattgtctcatgagcggatacatatttgaatgtatttagaaaaataaacaaataggggttccgcgcacatttccccgaaaagtgccacctgacgtctaagaaaccattattatcatgacattaacctataaaaataggcgtatcacgaggccctttcgtc";
            checksum = Checksum.CcSeguid(watson, "long");
            That(IsValidChecksum(checksum, "ccseguid=", true), Is.True, "Valid ccseguid format");


        }

        [Test]
        public void TestErrorConditions()
        {
            // Empty sequences
            Throws<ArgumentException>(() => Checksum.Seguid(""), "Detects empty sequence");
            Throws<ArgumentException>(() => Checksum.LsSeguid(""), "Detects empty sequence");
            Throws<ArgumentException>(() => Checksum.CsSeguid(""), "Detects empty sequence");
            Throws<ArgumentException>(() => Checksum.LdSeguid("", "ACGT"), "Detects empty Watson strand");
            Throws<ArgumentException>(() => Checksum.CdSeguid("ACGT", ""), "Detects empty Crick strand");

            // Invalid alphabets
            Throws<ArgumentException>(() => Checksum.Seguid("ACGT", "{invalid}"), "Detects invalid alphabet");
            Throws<ArgumentException>(() => Checksum.LsSeguid("12345", "{DNA}"), "Detects invalid characters");

            // Invalid forms
            Throws<ArgumentException>(() => Checksum.Seguid("ACGT", "{DNA}", "invalid"), "Detects invalid form");
        }

        [Test]
        public void TestSpecialCases()
        {
            // Very long sequences
            string longSeq = new string('A', 1000);
            DoesNotThrow(() => Checksum.LsSeguid(longSeq), "Handles long sequences");
        }

        [Test]
        public void TestIdempotency()
        {
            string seq = "ACGT";
            string checksum1 = Checksum.LsSeguid(seq);
            string checksum2 = Checksum.LsSeguid(seq);
            That(checksum1, Is.EqualTo(checksum2), "Checksum function is idempotent");

            string watson = "ACGT";
            string crick = "ACGT";
            checksum1 = Checksum.CdSeguid(watson, crick);
            checksum2 = Checksum.CdSeguid(watson, crick);
            That(checksum1, Is.EqualTo(checksum2), "Double-stranded checksum is idempotent");
        }

        [Test]
        public void TestBase64UrlEncoding()
        {
            // Generate sequence likely to produce '+' or '/' in base64
            string seq = new string('A', 30);

            string v1 = Checksum.Seguid(seq);  // Uses standard base64
            string v2 = Checksum.LsSeguid(seq);  // Uses base64url

            That(v1, Does.Match("[+/]"), "v1 uses standard base64 encoding");
            That(v2, Does.Not.Match("[+/]"), "v2 uses URL-safe base64 encoding");
            That(v2, Does.Match("[-_]"), "v2 contains URL-safe characters");
        }
    }
}