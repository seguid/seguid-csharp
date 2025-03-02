
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace SEGUID.CLI
{
    public class Program
    {
        private const string Version = "1.01";

        private static readonly string Usage = $@"Usage: echo <sequence> | .\seguid-csharp [options]

Options:
    --help          Display this help message
    --version       Display version information
    --type=TYPE     Type of checksum to calculate:
                    seguid (default), lsseguid, csseguid, ldseguid, cdseguid
    --alphabet=STR  Set of symbols for input sequence:
                    {{DNA}} (default), {{RNA}}, {{protein}}, etc.
    --form=FORM     Form of checksum to display:
                    long (default), short, both

Predefined alphabets:
 '{{DNA}}'              Complementary DNA symbols (= 'AT,CG')
 '{{DNA-extended}}'     Extended DNA (= '{{DNA}},BV,DH,KM,SS,RY,WW,NN')
 '{{RNA}}'              Complementary RNA symbols (= 'AU,CG')
 '{{RNA-extended}}'     Extended RNA (= '{{RNA}},BV,DH,KM,SS,RY,WW,NN')
 '{{protein}}'          Amino-acid symbols (= 'A,C,D,E,F,G,H,I,K,L,M,N,P,Q,R,S,T,V,W,Y')
 '{{protein-extended}}' Amino-acid symbols (= '{{protein}},O,U,B,J,Z,X')

Examples:
 echo 'ACGT' | seguid --type=lsseguid
 echo 'ACGT;TGCA' | seguid --type=ldseguid
 echo '-CGT;ACGT' | seguid --type=ldseguid
 echo 'ACGU' | seguid --type=lsseguid --alphabet='{{RNA}}'
 echo 'tcgcgcgtttcggtgatgacggtgaaaacctctgacacatgcagctcccggagacggtcacagcttgtctgtaagcggatgccgggagcagacaagcccgtcagggcgcgtcagcgggtgttggcgggtgtcggggctggcttaactatg'
     | seguid --type=ccseguid 



Version: {Version}
Copyright: Gyorgy Babnigg (2025)
License: MIT License";

        public class Options
        {
            public bool Help { get; set; }
            public bool Version { get; set; }
            public string Type { get; set; } = "seguid";
            public string Alphabet { get; set; } = "{DNA}";
            public string Form { get; set; } = "long";
        }

        public static int Main(string[] args)
        {
            try
            {
                return Run(args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }


        }

        public static int Run(string[] args)
        {
            // Parse command line options
            var options = ParseCommandLine(args);

            // Handle help and version
            if (options.Help)
            {
                Console.WriteLine(Usage);
                return 0;
            }

            if (options.Version)
            {
                Console.WriteLine(Version);
                return 0;
            }

            // Read sequence from STDIN
            string sequence = ReadStandardInput();

            // Process sequence based on type
            string result = ProcessSequence(sequence, options);

            // Output result
            Console.WriteLine(result);
            return 0;
        }

        private static Options ParseCommandLine(string[] args)
        {
            var options = new Options();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("--"))
                {
                    string option = arg.Substring(2);
                    string value = "";

                    // Handle option=value format
                    int equalIndex = option.IndexOf('=');
                    if (equalIndex != -1)
                    {
                        value = option.Substring(equalIndex + 1);
                        option = option.Substring(0, equalIndex);
                    }
                    // Handle separate value
                    else if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        value = args[++i];
                    }

                    switch (option.ToLower())
                    {
                        case "help":
                            options.Help = true;
                            break;
                        case "version":
                            options.Version = true;
                            break;
                        case "type":
                            options.Type = value;
                            break;
                        case "alphabet":
                            options.Alphabet = value;
                            break;
                        case "form":
                            options.Form = value;
                            break;
                        default:
                            throw new ArgumentException($"Unknown option: --{option}");
                    }
                }
            }

            return options;
        }

        private static string ReadStandardInput()
        {
            var lines = new List<string>();
            string? line;
            while ((line = Console.ReadLine())
                   != null)
            {
                lines.Add(line);
            }
            return string.Join("\n", lines);
        }

        private static string ProcessSequence(string sequence, Options options)
        {
            switch (options.Type.ToLower())
            {
                case "seguid":
                    return Checksum.Seguid(sequence, options.Alphabet, options.Form);

                case "lsseguid":
                    return Checksum.LsSeguid(sequence, options.Alphabet, options.Form);

                case "csseguid":
                    return Checksum.CsSeguid(sequence, options.Alphabet, options.Form);

                case "ccseguid":
                    return Checksum.CcSeguid(sequence, options.Form);

                case "ldseguid":
                case "cdseguid":
                    var parseResult = SequenceRepresentation.ParseSequenceString(sequence);
                    if (parseResult.Type != "ds")
                    {
                        throw new ArgumentException("Double-stranded sequence expected");
                    }
                    return options.Type.ToLower() == "ldseguid"
                        ? Checksum.LdSeguid(parseResult.Watson, parseResult.Crick, options.Alphabet, options.Form)
                        : Checksum.CdSeguid(parseResult.Watson, parseResult.Crick, options.Alphabet, options.Form);

                default:
                    throw new ArgumentException($"Unknown --type='{options.Type}'");
            }
        }
    }
}