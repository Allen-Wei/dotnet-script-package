echo "clean..."
rm ../../release/dwz.mac
rm -rf ../../publish/
echo "publish..."
dotnet script publish main.csx -o ../../publish -c Release -r osx-x64
echo "packaging..."
./macos-x64.warp-packer --arch macos-x64 --input_dir ../../publish --exec script --output ../../release/dwz.mac
chmod +x ../../release/dwz.mac