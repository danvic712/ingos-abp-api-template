# packing the nuget package
Write-Output "1. packing the nuget package..."
dotnet pack ../Ingos.Abp.Templates.csproj --output ../
Write-Output "  packing end."

# delete bin and obj folders
Write-Output "2. clean up the folders generated during the packaging process..."
Remove-Item ../bin, ../obj -Recurse
Write-Output "  clean up end."

