rmdir /S /Q ..\..\publish
dotnet script publish main.csx -o ..\..\publish -c Release -r win10-x64
..\..\tools\windows-x64.warp-packer.exe --arch windows-x64 --input_dir ..\..\publish --exec script.exe --output ..\..\releases\dwz.win.exe