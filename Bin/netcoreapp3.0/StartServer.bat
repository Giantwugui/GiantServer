@echo off
start "Global"    dotnet Server.App.dll       --appType=Global       --appId=1001 --subId=1
start "Account"   dotnet Server.App.dll     --appType=Account    --appId=1001 --subId=1
start "Manager"   dotnet Server.App.dll    --appType=Manager   --appId=1001 --subId=1
start "Gate"      dotnet Server.App.dll        --appType=Gate         --appId=1001 --subId=1
start "Map"       dotnet Server.App.dll       --appType=Zone         --appId=1001 --subId=1
start "Relation"  dotnet Server.App.dll      --appType=Relation     --appId=1001 --subId=1