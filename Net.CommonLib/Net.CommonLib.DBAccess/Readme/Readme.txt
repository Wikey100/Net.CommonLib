[程序功能简介：]
	数据库访问公共组件

[编译环境说明：]（硬件配置、操作系统名称、版本、数据库SERVER/CLIENT要求、版本、编译器名称、版本）

  1.硬件配置、操作系统名称、版本：
	操作系统： Win2000 /WinXP 
	
  2.编译器名称、版本：
	编译工具：VS2008以上版本

[编译变量：]（make文件名称/编译开关说明）

1..net frameword2.0
     
2.需要在工程中引用Suntek.Common.DBAccess，并将以下链接库拷贝到执行目标文件夹：
    sybdrvado11.dll
	sybdrvssl.dll
	Sybase.Data.AseClient.dll
	System.Data.SQLite.dll
	log4net.dll
3.VS2008编译
4.使用时请调用DBAccessor类