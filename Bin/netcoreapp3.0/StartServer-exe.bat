@echo off
start "Global"  Server.Global.exe     --appType=Global    --appId=1001 --subId=1
start "Account"  Server.Account.exe   --appType=Account   --appId=1001 --subId=1
start "Manager" Server.Manager.exe    --appType=Manager   --appId=1001 --subId=1
start "Gate"    Server.Gate.exe       --appType=Gate      --appId=1001 --subId=1
start "Map"     Server.Map.exe        --appType=Map       --appId=1001 --subId=1
start "Relation"  Server.Relation.exe --appType=Relation  --appId=1001 --subId=1