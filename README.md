# dotnetCampus.MSBuildUtils

当你需要写一个运行于 MSBuild 中的可执行程序，那么会有一堆杂七杂八的脚手架代码需要编写，还需要每次指定如何报告编译错误等。现在有了这个，你就不在需要编写杂七杂八的代码了，专注于你的编译代码吧 

| Build | NuGet |
|--|--|
|![](https://github.com/dotnet-campus/dotnetCampus.MSBuildUtils/workflows/.NET%20Core/badge.svg)|[![](https://img.shields.io/nuget/v/dotnetCampus.MSBuildUtils.Source.svg)](https://www.nuget.org/packages/dotnetCampus.MSBuildUtils.Source)|

支持功能：

- 在 MSBuild 中输出消息
- 在 VisualStudio 输出警告或错误，并支持传入错误的文件及行数让 VisualStudio 进行自动跳转
