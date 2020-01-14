# $1: csx file path
# $2: targe file path

echo "clean..."
rm -rf ./publish
rm -rf ./releases
mkdir ./publish
mkdir ./releases

echo "publish..."
dotnet script publish $1 -o ./publish -c Release -r linux-x64

echo "packaging..."
./tools/linux-x64.warp-packer --arch linux-x64 --input_dir ./publish --exec script --output ./releases/$2
chmod +x ./releases/$2