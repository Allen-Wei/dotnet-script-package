echo "clean..."
rm dwz.mac
rm -rf publish/
echo "publish..."
dotnet script publish main.csx -o publish -c Release -r osx-x64
echo "packaging..."
./macos-x64.warp-packer --arch macos-x64 --input_dir publish --exec script --output dwz.mac
chmod +x dwz.mac
echo "short url: "
./dwz.mac http://tim.qq.com