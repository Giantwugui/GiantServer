@echo off
start "Manager" dotnet Server.App.dll --appType=Manager --appId=1002
start "Gate"    dotnet Server.App.dll --appType=Gate    --appId=1001
start "Map"     dotnet Server.App.dll --appType=Map     --appId=1003
start "Relation"  dotnet Server.App.dll --appType=Relation  --appId=1004