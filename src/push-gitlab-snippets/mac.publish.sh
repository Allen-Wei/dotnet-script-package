echo "clean..."
rm ../../releases/dwz.mac
rm -rf ../../publish/
echo "publish..."
dotnet script publish main.csx -o ../../publish -c Release -r osx-x64
echo "packaging..."
../../tools/macos-x64.warp-packer --arch macos-x64 --input_dir ../../publish --exec script --output ../../releases/dwz.mac
chmod +x ../../releases/dwz.mac