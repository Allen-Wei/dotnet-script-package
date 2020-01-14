
rmdir /S /Q .\publish
rmdir /S /Q .\releases
mkdir .\publish
mkdir .\releases

dotnet script publish %1 -o .\publish -c Release -r win10-x64

.\tools\windows-x64.warp-packer.exe --arch windows-x64 --input_dir .\publish --exec script.exe --output ..\releases\%2