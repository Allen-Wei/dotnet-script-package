echo "clean..."
rm ../../releases/gitlab-snippets.linux
rm -rf ../publish
echo "publish..."
dotnet script publish main.csx -o ../../publish -c Release -r linux-x64
echo "packaging..."
../../tools/linux-x64.warp-packer --arch linux-x64 --input_dir ../../publish --exec script --output ../../releases/gitlab-snippets.linux
chmod +x ../../releases/gitlab-snippets.linux