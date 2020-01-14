# $1: csx file path
# $2: targe file path

echo "clean..."
rm -rf ./publish
rm -rf ./releases
mkdir ./publish
mkdir ./releases

echo "publish..."
dotnet script publish $1 -o ./publish -c Release -r osx-x64

echo "packaging..."
./macos-x64.warp-packer --arch macos-x64 --input_dir ./publish --exec script --output $2
chmod +x ./releases/$2
