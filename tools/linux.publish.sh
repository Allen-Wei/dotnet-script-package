echo "clean..."
rm dwz.linux
rm -rf publish
echo "publish..."
dotnet script publish main.csx -o publish -c Release -r linux-x64
echo "packaging..."
./linux-x64.warp-packer --arch linux-x64 --input_dir publish --exec script --output dwz.linux
chmod +x dwz.linux
echo "short url: "
./dwz.linux http://tim.qq.com