## SmTools
该项目作为我学习 .NET 跨平台框架的实战。它用到了类似模块化架构。

### 运行项目

```bash
dotnet run --project src/SmTools.Api/SmTools.Api.csproj
```

### 数据库迁移
1. 首先，确保已安装必要的 EF Core 工具：

```bash
dotnet tool install --global dotnet-ef
```

2. 然后，在项目目录下（包含 .csproj 文件的目录）执行以下命令来创建新的迁移：

```bash
# 进入 SmTools.Api.Persistence 项目目录
cd src/SmTools.Api.Persistence

# 创建新的迁移，将 InitialCreate 替换为有意义的迁移名称，如 Migration_202502271649
dotnet ef migrations add InitialCreate --startup-project ../SmTools.Api/SmTools.Api.csproj
```

3. 最后，应用迁移到数据库：
- 将 appsettings.Development.json 中的 `Db.Init` 设置为 `true`
- 运行项目
