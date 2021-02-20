@echo off
taskkill /f /im Server.Global.exe
taskkill /f /im Server.Account.exe
taskkill /f /im Server.Manager.exe
taskkill /f /im Server.Gate.exe
taskkill /f /im Server.Zone.exe
taskkill /f /im Server.Relation.exe