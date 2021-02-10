@echo off
start "Global"  Server.App.exe       --appType=Global    --appId=1001 --subId=1
start "Account"  Server.App.exe    --appType=Account   --appId=1001 --subId=1
start "Manager" Server.App.exe    --appType=Manager   --appId=1001 --subId=1
start "Gate"    Server.App.exe       --appType=Gate      --appId=1001 --subId=1
start "Map"     Server.App.exe      --appType=Zone       --appId=1001 --subId=1
start "Relation"  Server.App.exe   --appType=Relation  --appId=1001 --subId=1