# ZLMediaKitServer 可视化后台管理系统

[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/MingZhuLiu/ZLMediaServerManagent/blob/master/LICENSE)
[![Language](https://img.shields.io/static/v1?label=Language&message=.NetCore&color=red)](https://github.com/dotnet/core)
[![platform](https://img.shields.io/badge/platform-linux%20|%20macos%20|%20windows-green.svg)](https://github.com/MingZhuLiu/ZLMediaServerManagent)
[![Build Status](https://img.shields.io/static/v1?label=Develop&message=building&color=yellow)](https://github.com/MingZhuLiu/ZLMediaServerManagent)

## 项目特点
* 基于.NetCore开发，可跨平台，可以打包编译出不依赖运行时的可执行程序。
* 前端页面使用Bootstrap框架。
* 数据库支持SQLite,Oracle,SQLServer,Mysql,Postgresql.
* 只需要配置好数据库连接字符串，无须建库建表，系统启动判断无库无表会自动创建库表并引导至初始化页面连接ZLMediaKitServer。
* 监控断流自动重新拉流。

***

## 项目依赖
  * 基于[ZLMediaKitServer](https://github.com/xia-chu/ZLMediaKit)作为基础服务，实现流媒体服务可视化管理，开箱即用。
  * 基于[ZLMediaKit.DotNetCore.Sdk](https://github.com/MingZhuLiu/ZLMediaKit.DotNetCore.Sdk)库实现与ZLMediaKitServer通信与调用。
  ***


  ## 快速上手
  1. 根据平台安装[.Net Core SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)(已安装则忽略)
  2. 项目根目录执行命令 dotnet restore
  3. 配置文件参考 appsettings.json 数据库参数根据自身情况注释掉其他四种配置，保留自己期望的数据库。(SQLite数据库无须事先创建文件，只需要正确配置最终SQLite文件位置即可。其他类型数据库无须事先创建数据库，只需要确保连接字符串中的ip，端口，数据库用户密码正确即可)
  4. 执行命令dotnet run 测试运行情况(如果新手遇到问题，请跳过本条命令，直接执行下面的发布命令)。
  5. 发布运行命令范例 dotnet publish -r  --runtime win-x64 --self-contained true
  6. 发布完毕，看一下输出最后一行'publish/'结尾的路径，跳转至改目录执行命令： ./ZLMediaServerManagent
  6. --runtime win-x64  表示运行时是windwos64位 Linux平台则为linux-64 Mac平台则为 osx.10.11-x64
  7. 发布命令参数解释 --self-contained true 表示是否自包含运行环境，如传false则目标计算机需先安装 .Net Core 运行时，优势是打包文件较小。
  8. 您也可以将整个系统打包成单个可执行程序文件 发布命令加上参数 /p:PublishSingleFile=true
***


## 运行效果

  * 首次运行系统自动引导至初始化页面

  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/初始化页面.jpg)


  * 菜单管理页面

  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/菜单模块.jpg)

  * 角色管理页面

  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/角色管理.jpg)

  * 服务器配置
  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/服务器配置修改.jpg)
  
  * 域名和应用(对应拉流中的参数vHost和app)
  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/域名和应用管理.jpg)

  * 拉流代理
  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/拉流代理.jpg)

  * 视频播放
  ![avatar](https://raw.githubusercontent.com/MingZhuLiu/ZLMediaServerManagent/master/wwwroot/imgs/template/视频播放v1.jpg)

  ## 下一步计划
  * 制作首页，展示ZLMediaServer服务器性能指标。
  * 添加对H.265的支持。
  * 添加支持MP4控件播放。


  ## 问题反馈
  * QQ:583530128
  * Mail: starry_link@foxmail.com
***

  ## 致谢
  * 感谢作者[夏楚](https://github.com/xia-chu)开源出项目[ZLMediaKit](https://github.com/xia-chu/ZLMediaKit)




  