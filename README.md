# Giant 游戏服务器框架

## Giant 介绍
Giant是一个基于 C# .NetCore 开发的一套，单线程异步游戏服务器框架。使用分层架构。服务器支持分布式部署、支持服务的自动注册与发现。
使用的插件 https://github.com/giantwugui/GiantPlugins

### 1. 配置表
配置表采用通用的 xml 配置

### 2. 数据库
数据库支持 MongoDB、MySQL

### 3. 缓存
缓存支持 Redis

### 4. 通讯协议
支持 TCP Socket通讯，使用 Google Protobuf 通讯协议。支持通知和Rpc通讯方式


## Giant 2.0
Giant2.0 将插件和服务器整合进一个项目。 以组件模式开发，极大的解耦了模块依赖
