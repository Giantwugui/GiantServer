@echo off
protoc.exe -I "F:\GiantFrame\Frame\Giant.Grpc\Proto" --csharp_out "F:\GiantFrame\Frame\Giant.Grpc\Grpc" --grpc_out "F:\GiantFrame\Frame\Giant.Grpc\Grpc" --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe  math.proto

@pause