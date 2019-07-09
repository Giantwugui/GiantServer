@echo off
start dotnet Server.App.dll --appType=Gate --appId=1001
start dotnet Server.App.dll --appType=Manager --appId=1002
start dotnet Server.App.dll --appType=Map --appId=1003
start dotnet Server.App.dll --appType=Social --appId=1004