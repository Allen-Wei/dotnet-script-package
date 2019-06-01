echo "clean..."
rm ../../releases/gitlab-snippets.mac
rm -rf ../../publish/
echo "publish..."
dotnet script publish main.csx -o ../../publish -c Release -r osx-x64
echo "packaging..."
../../tools/macos-x64.warp-packer --arch macos-x64 --input_dir ../../publish --exec script --output ../../releases/gitlab-snippets.mac
chmod +x ../../releases/gitlab-snippets.mac