@echo off
start "Manager 1002" dotnet Server.App.dll --appType=Manager --appId=1002
start "Gate 1001"    dotnet Server.App.dll --appType=Gate    --appId=1001
start "Map 1003"     dotnet Server.App.dll --appType=Map     --appId=1003
start "Social 1004"  dotnet Server.App.dll --appType=Social  --appId=1004