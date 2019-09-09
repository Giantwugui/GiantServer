@echo off
start "Global"  dotnet Server.Global.dll        --appType=Global    --appId=1001 --subId=1
start "Account"  dotnet Server.Account.dll   --appType=Account   --appId=1001 --subId=1
start "Manager" dotnet Server.Manager.dll    --appType=Manager   --appId=1001 --subId=1
start "Gate"    dotnet Server.Gate.dll       --appType=Gate      --appId=1001 --subId=1
start "Map"     dotnet Server.Map.dll        --appType=Map       --appId=1001 --subId=1
start "Relation"  dotnet Server.Relation.dll --appType=Relation  --appId=1001 --subId=1