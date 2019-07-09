@echo off
start "Gate" dotnet Server.App.dll --appType=Gate --appId=1001
start "Manager" dotnet Server.App.dll --appType=Manager --appId=1002
start "Map" dotnet Server.App.dll --appType=Map --appId=1003
start "Social" dotnet Server.App.dll --appType=Social --appId=1004