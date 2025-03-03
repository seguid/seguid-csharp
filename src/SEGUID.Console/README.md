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

# Make sure the executable is in your PATH



# Calculate SEGUID for a DNA sequence
echo "ACGT" | seguid

# Calculate LSSEGUID with RNA alphabet
echo "ACGU" | seguid --type=lsseguid --alphabet={RNA}

# Calculate LDSEGUID for double-stranded sequence
echo "ACGT;TGCA" | seguid --type=ldseguid
```

For help and available options:
```bash
seguid --help
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


