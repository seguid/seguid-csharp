# SEGUID v2 C# Implementation: Checksums for Linear, Circular, Single- and Double-Stranded Biological Sequences

This C# program, seguid, implements SEGUID v2 together with the original SEGUID algorithm.

## Installation

### Option 1: Using pre-built binaries
1. Download the latest release from the [Releases](https://github.com/seguid/seguid-csharp/releases/tag/v.1.0.0) page
2. Extract the zip file
3. Add the directory containing `seguid.exe` to your system PATH, or run it directly from the extracted location

### Option 2: Building from source
Requirements:
- .NET 8.0 SDK or later

Steps:
1. Clone the repository:
```bash
git clone https://github.com/yourusername/seguid-csharp.git
cd seguid-csharp
```

2. Build the solution:
```bash
dotnet build -c Release
```
The executable will be located at:
```bash
src/SEGUID.Console/bin/Release/net8.0/seguid.exe
```

## Usage

From command line:

```bash
# Calculate SEGUID for a DNA sequence
echo "ACGT" | .\seguid.exe

# Calculate LSSEGUID with RNA alphabet
echo "ACGU" | .\seguid.exe --type=lsseguid --alphabet={RNA}

# Calculate LDSEGUID for double-stranded sequence
echo "ACGT;TGCA" | .\seguid.exe --type=ldseguid
```

For help and available options:
```bash
.\seguid.exe --help
```


In  code:
```
using SEGUID;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("SEGDUID v2 examples for linear sequences (DNA, RNA, and protein) with controlled alphabets\n");

        // Example DNA sequence ({DNA})
        string exampleDNA = "GCATGCAT";

        // Example RNA sequence ({RNA})
        string exampleRNA = "GCAUGCAU";

        // Example extended DNA sequence ({DNA-extended})
        string exampleDNAExtended = "GCATBVDHKMSSRYWWNN";

        // Example extended RNA sequence ({RNA-extended})
        string exampleRNAExtended = "GCAUBVDHKMSSRYWWNN";

        // Example protein sequence ({protein})
        string exampleProtein = "ACDEFGHIKLMNPQRSTVWYOU";

        // Example extended protein sequence ({protein-extended})
        string exampleProteinExtended = "ACDEFGHIKLMNPQRSTVWYOUBJXZ";

        // Example protein sequence for demonstrating the SEGUID V1 method ({proteinV1})
        string exampleProteinV1 = "ACDEFGHIKLMNPQRSTVWYAC";

        // Calculate SEGUID v2 for DNA sequence
        Console.WriteLine("Checksum.Seguid(\"" + exampleDNA + "\", \"{DNA}\"): " + Checksum.Seguid(exampleDNA, "{DNA}"));
        // Calculate SEGUID v2 for DNA sequence (short form)
        Console.WriteLine("Checksum.Seguid(\"" + exampleDNA + "\", \"{DNA}\", \"short\"): " + Checksum.Seguid(exampleDNA, "{DNA}", "short"));
        // Calculate SEGUID v2 for RNA sequence
        Console.WriteLine("Checksum.Seguid(\"" + exampleRNA + "\", \"{RNA}\"): " + Checksum.Seguid(exampleRNA, "{RNA}"));
        // Calculate SEGUID v2 for DNA sequence with extended alphabet
        Console.WriteLine("Checksum.Seguid(\"" + exampleDNAExtended + "\", \"{DNA-extended}\"): " + Checksum.Seguid(exampleDNAExtended, "{DNA-extended}"));
        // Calculate SEGUID v2 for RNA sequence with extended alphabet
        Console.WriteLine("Checksum.Seguid(\"" + exampleRNAExtended + "\", \"{RNA-extended}\"): " + Checksum.Seguid(exampleRNAExtended, "{RNA-extended}"));
        // Calculate SEGUID v2 for protein sequence
        Console.WriteLine("Checksum.Seguid(\"" + exampleProtein + "\", \"{protein}\"): " + Checksum.Seguid(exampleProtein, "{protein}"));
        // Calculate SEGUID v2 for protein sequence with extended alphabet
        Console.WriteLine("Checksum.Seguid(\"" + exampleProteinExtended + "\", \"{protein-extended}\"): " + Checksum.Seguid(exampleProteinExtended, "{protein-extended}"));


        // More examples of SEGUID v2 for DNA sequences
        Console.WriteLine("\nSEGDUID v2 examples for single-stranded DNA\n");

        // Calculate SEGUID v2 for linear single-stranded DNA sequence
        Console.WriteLine("Calculate SEGUID v2 for linear single-stranded DNA sequence (Checksum.LsSeguid)");
        Console.WriteLine("Checksum.LsSeguid(\"" + exampleDNA + "\", \"{DNA}\"): " + Checksum.LsSeguid(exampleDNA, "{DNA}"));

        // Calculate SEGUID v2 for linear circular DNA sequence (note the change in value because of rotation of circular DNA)
        Console.WriteLine("\nCalculate SEGUID v2 for circular single-stranded DNA sequence (Checksum.CsSeguid)");
        Console.WriteLine("Checksum.CsSeguid(\"" + exampleDNA + "\", \"{DNA}\"): " + Checksum.CsSeguid(exampleDNA, "{DNA}"));


        Console.WriteLine("\nSEGDUID v2 examples for linear double-stranded DNA\n");


        //TestLinearDoubleStrandedSequences
        string watson = "-TATGCC";
        string crick = "-GCATAC";


        // Calculate SEGUID v2 for linear double-stranded DNA sequence
        Console.WriteLine("Calculate SEGUID v2 for linear double-stranded DNA sequence (Checksum.LdSeguid(watson, crick))");
        Console.WriteLine("Checksum.LdSeguid(\"" + watson + "\", \"" + crick + "\"): " + Checksum.LdSeguid(watson, crick));


        // Topology Strandedness    Function
        // linear  single lsseguid
        // circular single  csseguid
        // linear  double ldseguid
        // circular    double cdseguid

        Console.WriteLine("Checksum.SeguidV1(\"" + exampleProteinV1 + "\", \"{proteinV1}\"): " + Checksum.SeguidV1(exampleProteinV1, "{proteinV1}"));
        Console.WriteLine("Checksum.SeguidV1UrlSafe(\"" + exampleProteinV1 + "\", \"{proteinV1}\"): " + Checksum.SeguidV1UrlSafe(exampleProteinV1, "{proteinV1}"));



        Console.WriteLine("seguid: " + Checksum.Seguid("A"));
        Console.WriteLine("lsseguid: " + Checksum.Seguid("A"));
        Console.WriteLine("csseguid: " + Checksum.Seguid("A"));

        Console.WriteLine("seguidv1=N/DxuiQwt3rU+nDzU5/q+CaRuQM: " + Checksum.SeguidV1("MGDRSEGPGPTRPGPPGIGP"));
        Console.WriteLine("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM: " + Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGP"));
        Console.WriteLine("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM: " + Checksum.SeguidV1UrlSafe("mgDRSEGpgpTRPGPPGigp"));
        Console.WriteLine("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM: " + Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGPB"));
        Console.WriteLine("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM: " + Checksum.SeguidV1UrlSafe("MGDRSEGPGPTRPGPPGIGPO"));
        Console.WriteLine("seguidv1urlsafe=N_DxuiQwt3rU-nDzU5_q-CaRuQM: " + Checksum.SeguidV1UrlSafe("12_*!@#$%^&MGDRSEGP GPTRPGPP  -GIGPO"));

        string seq = "ACGT";
        string seqRNA = "ACGUBVDHKMSRYWN";

        string checksum = Checksum.LsSeguid(seq);
        Console.WriteLine("lsseguid: " + checksum);

        string shortForm = Checksum.LsSeguid(seq, "{DNA}", "short");
        Console.WriteLine("lsseguid: " + shortForm);

        Console.WriteLine(Checksum.LsSeguid(seq, "{DNA-extended}"));
        Console.WriteLine(Checksum.LsSeguid(seqRNA, "{RNA-extended}"));

        seq = "ACGT";
        checksum = Checksum.CsSeguid(seq);

        Console.WriteLine("csseguid: " + checksum);

        string rotated = Checksum.CsSeguid("GTAC");

        Console.WriteLine("rotated csseguid: " + rotated);


        Console.WriteLine("ldseguid: " + checksum);


        //TestCircularDoubleStrandedSequences

        watson = "GTATGCC";
        crick = "GGCATAC";

        checksum = Checksum.CdSeguid(watson, crick);

        Console.WriteLine("cdseguid: " + checksum);
        // rotated
        watson = "CGTATGC";
        crick = "GCATACG";

        checksum = Checksum.CdSeguid(watson, crick);
        Console.WriteLine("rotated cdseguid: " + checksum);


        watson = "tcgcgcgtttcggtgatgacggtgaaaacctctgacacatgcagctcccggagacggtcacagcttgtctgtaagcggatgccgggagcagacaagcccgtcagggcgcgtcagcgggtgttggcgggtgtcggggctggcttaactatgcggcatcagagcagattgtactgagagtgcaccatatgcggtgtgaaataccgcacagatgcgtaaggagaaaataccgcatcaggcgccattcgccattcaggctgcgcaactgttgggaagggcgatcggtgcgggcctcttcgctattacgccagctagaggaccagccgcgtaacctggcaaaatcggttacggttgagtaataaatggatgccctgcgtaagcgggtgtgggcggacaataaagtcttaaactgaacaaaatagatctaaactatgacaataaagtcttaaactagacagaatagttgtaaactgaaatcagtccagttatgctgtgaaaaagcatactggacttttgttatggctaaagcaaactcttcattttctgaagtgcaaattgcccgtcgtattaaagaggggcgtggggtcgacgatatcatgcatgagctcactagtggatcccccgggctgcaggaattcctcgagaagcttgggcccggtacctcgcgaaggccttgcaggccaaccagataagtgaaatctagttccaaactattttgtcatttttaattttcgtattagcttacgacgctacacccagttcccatctattttgtcactcttccctaaataatccttaaaaactccatttccacccctcccagttcccaactattttgtccgcccacagcggggcatttttcttcctgttatgtttgggcgctgcattaatgaatcggccaacgcgcggggagaggcggtttgcgtattgggcgctcttccgcttcctcgctcactgactcgctgcgctcggtcgttcggctgcggcgagcggtatcagctcactcaaaggcggtaatacggttatccacagaatcaggggataacgcaggaaagaacatgtgagcaaaaggccagcaaaaggccaggaaccgtaaaaaggccgcgttgctggcgtttttccataggctccgcccccctgacgagcatcacaaaaatcgacgctcaagtcagaggtggcgaaacccgacaggactataaagataccaggcgtttccccctggaagctccctcgtgcgctctcctgttccgaccctgccgcttaccggatacctgtccgcctttctcccttcgggaagcgtggcgctttctcatagctcacgctgtaggtatctcagttcggtgtaggtcgttcgctccaagctgggctgtgtgcacgaaccccccgttcagcccgaccgctgcgccttatccggtaactatcgtcttgagtccaacccggtaagacacgacttatcgccactggcagcagccactggtaacaggattagcagagcgaggtatgtaggcggtgctacagagttcttgaagtggtggcctaactacggctacactagaaggacagtatttggtatctgcgctctgctgaagccagttaccttcggaaaaagagttggtagctcttgatccggcaaacaaaccaccgctggtagcggtggtttttttgtttgcaagcagcagattacgcgcagaaaaaaaggatctcaagaagatcctttgatcttttctacggggtctgacgctcagtggaacgaaaactcacgttaagggattttggtcatgagattatcaaaaaggatcttcacctagatccttttaaattaaaaatgaagttttaaatcaatctaaagtatatatgagtaaacttggtctgacagttaccaatgcttaatcagtgaggcacctatctcagcgatctgtctatttcgttcatccatagttgcctgactccccgtcgtgtagataactacgatacgggagggcttaccatctggccccagtgctgcaatgataccgcgagacccacgctcaccggctccagatttatcagcaataaaccagccagccggaagggccgagcgcagaagtggtcctgcaactttatccgcctccatccagtctattaattgttgccgggaagctagagtaagtagttcgccagttaatagtttgcgcaacgttgttgccattgctacaggcatcgtggtgtcacgctcgtcgtttggtatggcttcattcagctccggttcccaacgatcaaggcgagttacatgatcccccatgttgtgcaaaaaagcggttagctccttcggtcctccgatcgttgtcagaagtaagttggccgcagtgttatcactcatggttatggcagcactgcataattctcttactgtcatgccatccgtaagatgcttttctgtgactggtgagtactcaaccaagtcattctgagaatagtgtatgcggcgaccgagttgctcttgcccggcgtcaatacgggataataccgcgccacatagcagaactttaaaagtgctcatcattggaaaacgttcttcggggcgaaaactctcaaggatcttaccgctgttgagatccagttcgatgtaacccactcgtgcacccaactgatcttcagcatcttttactttcaccagcgtttctgggtgagcaaaaacaggaaggcaaaatgccgcaaaaaagggaataagggcgacacggaaatgttgaatactcatactcttcctttttcaatattattgaagcatttatcagggttattgtctcatgagcggatacatatttgaatgtatttagaaaaataaacaaataggggttccgcgcacatttccccgaaaagtgccacctgacgtctaagaaaccattattatcatgacattaacctataaaaataggcgtatcacgaggccctttcgtc";
        crick = "gacgaaagggcctcgtgatacgcctatttttataggttaatgtcatgataataatggtttcttagacgtcaggtggcacttttcggggaaatgtgcgcggaacccctatttgtttatttttctaaatacattcaaatatgtatccgctcatgagacaataaccctgataaatgcttcaataatattgaaaaaggaagagtatgagtattcaacatttccgtgtcgcccttattcccttttttgcggcattttgccttcctgtttttgctcacccagaaacgctggtgaaagtaaaagatgctgaagatcagttgggtgcacgagtgggttacatcgaactggatctcaacagcggtaagatccttgagagttttcgccccgaagaacgttttccaatgatgagcacttttaaagttctgctatgtggcgcggtattatcccgtattgacgccgggcaagagcaactcggtcgccgcatacactattctcagaatgacttggttgagtactcaccagtcacagaaaagcatcttacggatggcatgacagtaagagaattatgcagtgctgccataaccatgagtgataacactgcggccaacttacttctgacaacgatcggaggaccgaaggagctaaccgcttttttgcacaacatgggggatcatgtaactcgccttgatcgttgggaaccggagctgaatgaagccataccaaacgacgagcgtgacaccacgatgcctgtagcaatggcaacaacgttgcgcaaactattaactggcgaactacttactctagcttcccggcaacaattaatagactggatggaggcggataaagttgcaggaccacttctgcgctcggcccttccggctggctggtttattgctgataaatctggagccggtgagcgtgggtctcgcggtatcattgcagcactggggccagatggtaagccctcccgtatcgtagttatctacacgacggggagtcaggcaactatggatgaacgaaatagacagatcgctgagataggtgcctcactgattaagcattggtaactgtcagaccaagtttactcatatatactttagattgatttaaaacttcatttttaatttaaaaggatctaggtgaagatcctttttgataatctcatgaccaaaatcccttaacgtgagttttcgttccactgagcgtcagaccccgtagaaaagatcaaaggatcttcttgagatcctttttttctgcgcgtaatctgctgcttgcaaacaaaaaaaccaccgctaccagcggtggtttgtttgccggatcaagagctaccaactctttttccgaaggtaactggcttcagcagagcgcagataccaaatactgtccttctagtgtagccgtagttaggccaccacttcaagaactctgtagcaccgcctacatacctcgctctgctaatcctgttaccagtggctgctgccagtggcgataagtcgtgtcttaccgggttggactcaagacgatagttaccggataaggcgcagcggtcgggctgaacggggggttcgtgcacacagcccagcttggagcgaacgacctacaccgaactgagatacctacagcgtgagctatgagaaagcgccacgcttcccgaagggagaaaggcggacaggtatccggtaagcggcagggtcggaacaggagagcgcacgagggagcttccagggggaaacgcctggtatctttatagtcctgtcgggtttcgccacctctgacttgagcgtcgatttttgtgatgctcgtcaggggggcggagcctatggaaaaacgccagcaacgcggcctttttacggttcctggccttttgctggccttttgctcacatgttctttcctgcgttatcccctgattctgtggataaccgtattaccgcctttgagtgagctgataccgctcgccgcagccgaacgaccgagcgcagcgagtcagtgagcgaggaagcggaagagcgcccaatacgcaaaccgcctctccccgcgcgttggccgattcattaatgcagcgcccaaacataacaggaagaaaaatgccccgctgtgggcggacaaaatagttgggaactgggaggggtggaaatggagtttttaaggattatttagggaagagtgacaaaatagatgggaactgggtgtagcgtcgtaagctaatacgaaaattaaaaatgacaaaatagtttggaactagatttcacttatctggttggcctgcaaggccttcgcgaggtaccgggcccaagcttctcgaggaattcctgcagcccgggggatccactagtgagctcatgcatgatatcgtcgaccccacgcccctctttaatacgacgggcaatttgcacttcagaaaatgaagagtttgctttagccataacaaaagtccagtatgctttttcacagcataactggactgatttcagtttacaactattctgtctagtttaagactttattgtcatagtttagatctattttgttcagtttaagactttattgtccgcccacacccgcttacgcagggcatccatttattactcaaccgtaaccgattttgccaggttacgcggctggtcctctagctggcgtaatagcgaagaggcccgcaccgatcgcccttcccaacagttgcgcagcctgaatggcgaatggcgcctgatgcggtattttctccttacgcatctgtgcggtatttcacaccgcatatggtgcactctcagtacaatctgctctgatgccgcatagttaagccagccccgacacccgccaacacccgctgacgcgccctgacgggcttgtctgctcccggcatccgcttacagacaagctgtgaccgtctccgggagctgcatgtgtcagaggttttcaccgtcatcaccgaaacgcgcga";
        watson = watson.ToUpper();
        crick = crick.ToUpper();

        long startTime = DateTime.Now.Ticks;
        checksum = Checksum.CdSeguid(watson, crick);
        long endTime = DateTime.Now.Ticks;
        Console.WriteLine("5793 bp plasmid: " + checksum);
        Console.WriteLine("Performed in: " + Convert.ToString((endTime - startTime)*100/1000000) + " ms");



        int minRotation = SequenceManipulation.MinRotation(watson);
        Console.WriteLine("minRotation: " + minRotation);
        Console.WriteLine("minRotation subseq: " + watson.Substring(minRotation,10));


        // testing ccseguid
        watson = "tcgcgcgtttcggtgatgacggtgaaaacctctgacacatgcagctcccggagacggtcacagcttgtctgtaagcggatgccgggagcagacaagcccgtcagggcgcgtcagcgggtgttggcgggtgtcggggctggcttaactatgcggcatcagagcagattgtactgagagtgcaccatatgcggtgtgaaataccgcacagatgcgtaaggagaaaataccgcatcaggcgccattcgccattcaggctgcgcaactgttgggaagggcgatcggtgcgggcctcttcgctattacgccagctagaggaccagccgcgtaacctggcaaaatcggttacggttgagtaataaatggatgccctgcgtaagcgggtgtgggcggacaataaagtcttaaactgaacaaaatagatctaaactatgacaataaagtcttaaactagacagaatagttgtaaactgaaatcagtccagttatgctgtgaaaaagcatactggacttttgttatggctaaagcaaactcttcattttctgaagtgcaaattgcccgtcgtattaaagaggggcgtggggtcgacgatatcatgcatgagctcactagtggatcccccgggctgcaggaattcctcgagaagcttgggcccggtacctcgcgaaggccttgcaggccaaccagataagtgaaatctagttccaaactattttgtcatttttaattttcgtattagcttacgacgctacacccagttcccatctattttgtcactcttccctaaataatccttaaaaactccatttccacccctcccagttcccaactattttgtccgcccacagcggggcatttttcttcctgttatgtttgggcgctgcattaatgaatcggccaacgcgcggggagaggcggtttgcgtattgggcgctcttccgcttcctcgctcactgactcgctgcgctcggtcgttcggctgcggcgagcggtatcagctcactcaaaggcggtaatacggttatccacagaatcaggggataacgcaggaaagaacatgtgagcaaaaggccagcaaaaggccaggaaccgtaaaaaggccgcgttgctggcgtttttccataggctccgcccccctgacgagcatcacaaaaatcgacgctcaagtcagaggtggcgaaacccgacaggactataaagataccaggcgtttccccctggaagctccctcgtgcgctctcctgttccgaccctgccgcttaccggatacctgtccgcctttctcccttcgggaagcgtggcgctttctcatagctcacgctgtaggtatctcagttcggtgtaggtcgttcgctccaagctgggctgtgtgcacgaaccccccgttcagcccgaccgctgcgccttatccggtaactatcgtcttgagtccaacccggtaagacacgacttatcgccactggcagcagccactggtaacaggattagcagagcgaggtatgtaggcggtgctacagagttcttgaagtggtggcctaactacggctacactagaaggacagtatttggtatctgcgctctgctgaagccagttaccttcggaaaaagagttggtagctcttgatccggcaaacaaaccaccgctggtagcggtggtttttttgtttgcaagcagcagattacgcgcagaaaaaaaggatctcaagaagatcctttgatcttttctacggggtctgacgctcagtggaacgaaaactcacgttaagggattttggtcatgagattatcaaaaaggatcttcacctagatccttttaaattaaaaatgaagttttaaatcaatctaaagtatatatgagtaaacttggtctgacagttaccaatgcttaatcagtgaggcacctatctcagcgatctgtctatttcgttcatccatagttgcctgactccccgtcgtgtagataactacgatacgggagggcttaccatctggccccagtgctgcaatgataccgcgagacccacgctcaccggctccagatttatcagcaataaaccagccagccggaagggccgagcgcagaagtggtcctgcaactttatccgcctccatccagtctattaattgttgccgggaagctagagtaagtagttcgccagttaatagtttgcgcaacgttgttgccattgctacaggcatcgtggtgtcacgctcgtcgtttggtatggcttcattcagctccggttcccaacgatcaaggcgagttacatgatcccccatgttgtgcaaaaaagcggttagctccttcggtcctccgatcgttgtcagaagtaagttggccgcagtgttatcactcatggttatggcagcactgcataattctcttactgtcatgccatccgtaagatgcttttctgtgactggtgagtactcaaccaagtcattctgagaatagtgtatgcggcgaccgagttgctcttgcccggcgtcaatacgggataataccgcgccacatagcagaactttaaaagtgctcatcattggaaaacgttcttcggggcgaaaactctcaaggatcttaccgctgttgagatccagttcgatgtaacccactcgtgcacccaactgatcttcagcatcttttactttcaccagcgtttctgggtgagcaaaaacaggaaggcaaaatgccgcaaaaaagggaataagggcgacacggaaatgttgaatactcatactcttcctttttcaatattattgaagcatttatcagggttattgtctcatgagcggatacatatttgaatgtatttagaaaaataaacaaataggggttccgcgcacatttccccgaaaagtgccacctgacgtctaagaaaccattattatcatgacattaacctataaaaataggcgtatcacgaggccctttcgtc";

        startTime = DateTime.Now.Ticks;
        checksum = Checksum.CcSeguid(watson);
        endTime = DateTime.Now.Ticks;
        Console.WriteLine("5793 bp plasmid: " + checksum);
        Console.WriteLine("Performed in: " + Convert.ToString((endTime - startTime) * 100 / 1000000) + " ms");

        crick = "gacgaaagggcctcgtgatacgcctatttttataggttaatgtcatgataataatggtttcttagacgtcaggtggcacttttcggggaaatgtgcgcggaacccctatttgtttatttttctaaatacattcaaatatgtatccgctcatgagacaataaccctgataaatgcttcaataatattgaaaaaggaagagtatgagtattcaacatttccgtgtcgcccttattcccttttttgcggcattttgccttcctgtttttgctcacccagaaacgctggtgaaagtaaaagatgctgaagatcagttgggtgcacgagtgggttacatcgaactggatctcaacagcggtaagatccttgagagttttcgccccgaagaacgttttccaatgatgagcacttttaaagttctgctatgtggcgcggtattatcccgtattgacgccgggcaagagcaactcggtcgccgcatacactattctcagaatgacttggttgagtactcaccagtcacagaaaagcatcttacggatggcatgacagtaagagaattatgcagtgctgccataaccatgagtgataacactgcggccaacttacttctgacaacgatcggaggaccgaaggagctaaccgcttttttgcacaacatgggggatcatgtaactcgccttgatcgttgggaaccggagctgaatgaagccataccaaacgacgagcgtgacaccacgatgcctgtagcaatggcaacaacgttgcgcaaactattaactggcgaactacttactctagcttcccggcaacaattaatagactggatggaggcggataaagttgcaggaccacttctgcgctcggcccttccggctggctggtttattgctgataaatctggagccggtgagcgtgggtctcgcggtatcattgcagcactggggccagatggtaagccctcccgtatcgtagttatctacacgacggggagtcaggcaactatggatgaacgaaatagacagatcgctgagataggtgcctcactgattaagcattggtaactgtcagaccaagtttactcatatatactttagattgatttaaaacttcatttttaatttaaaaggatctaggtgaagatcctttttgataatctcatgaccaaaatcccttaacgtgagttttcgttccactgagcgtcagaccccgtagaaaagatcaaaggatcttcttgagatcctttttttctgcgcgtaatctgctgcttgcaaacaaaaaaaccaccgctaccagcggtggtttgtttgccggatcaagagctaccaactctttttccgaaggtaactggcttcagcagagcgcagataccaaatactgtccttctagtgtagccgtagttaggccaccacttcaagaactctgtagcaccgcctacatacctcgctctgctaatcctgttaccagtggctgctgccagtggcgataagtcgtgtcttaccgggttggactcaagacgatagttaccggataaggcgcagcggtcgggctgaacggggggttcgtgcacacagcccagcttggagcgaacgacctacaccgaactgagatacctacagcgtgagctatgagaaagcgccacgcttcccgaagggagaaaggcggacaggtatccggtaagcggcagggtcggaacaggagagcgcacgagggagcttccagggggaaacgcctggtatctttatagtcctgtcgggtttcgccacctctgacttgagcgtcgatttttgtgatgctcgtcaggggggcggagcctatggaaaaacgccagcaacgcggcctttttacggttcctggccttttgctggccttttgctcacatgttctttcctgcgttatcccctgattctgtggataaccgtattaccgcctttgagtgagctgataccgctcgccgcagccgaacgaccgagcgcagcgagtcagtgagcgaggaagcggaagagcgcccaatacgcaaaccgcctctccccgcgcgttggccgattcattaatgcagcgcccaaacataacaggaagaaaaatgccccgctgtgggcggacaaaatagttgggaactgggaggggtggaaatggagtttttaaggattatttagggaagagtgacaaaatagatgggaactgggtgtagcgtcgtaagctaatacgaaaattaaaaatgacaaaatagtttggaactagatttcacttatctggttggcctgcaaggccttcgcgaggtaccgggcccaagcttctcgaggaattcctgcagcccgggggatccactagtgagctcatgcatgatatcgtcgaccccacgcccctctttaatacgacgggcaatttgcacttcagaaaatgaagagtttgctttagccataacaaaagtccagtatgctttttcacagcataactggactgatttcagtttacaactattctgtctagtttaagactttattgtcatagtttagatctattttgttcagtttaagactttattgtccgcccacacccgcttacgcagggcatccatttattactcaaccgtaaccgattttgccaggttacgcggctggtcctctagctggcgtaatagcgaagaggcccgcaccgatcgcccttcccaacagttgcgcagcctgaatggcgaatggcgcctgatgcggtattttctccttacgcatctgtgcggtatttcacaccgcatatggtgcactctcagtacaatctgctctgatgccgcatagttaagccagccccgacacccgccaacacccgctgacgcgccctgacgggcttgtctgctcccggcatccgcttacagacaagctgtgaccgtctccgggagctgcatgtgtcagaggttttcaccgtcatcaccgaaacgcgcga";

        startTime = DateTime.Now.Ticks;
        checksum = Checksum.CcSeguid(crick);
        endTime = DateTime.Now.Ticks;
        Console.WriteLine("5793 bp plasmid: " + checksum);
        Console.WriteLine("Performed in: " + Convert.ToString((endTime - startTime) * 100 / 1000000) + " ms");




    }



}

```

## Function Types

| Topology  | Strandedness | Function  |
|-----------|--------------|-----------|
| linear    | single       | lsseguid  |
| circular  | single       | csseguid  |
| linear    | double       | ldseguid  |
| circular  | double       | cdseguid  |


ccseguid calculates the checksum for a circular, single-stranded sequence, representing either the 5' to 3' or 3' to 5' direction of a plasmid for example. Any characters other than A, C, G, and T will be ignored.




## License

MIT License

Copyright (c) 2025 Gyorgy Babnigg

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Author

Gyorgy Babnigg <gbabnigg@gmail.com>


