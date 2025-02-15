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
```C#

```

## Function Types

| Topology  | Strandedness | Function  |
|-----------|--------------|-----------|
| linear    | single       | lsseguid  |
| circular  | single       | csseguid  |
| linear    | double       | ldseguid  |
| circular  | double       | cdseguid  |

## License

This library is free software; you can redistribute it and/or modify it under the same terms as Perl itself.

## Author

Gyorgy Babnigg <gbabnigg@gmail.com>


