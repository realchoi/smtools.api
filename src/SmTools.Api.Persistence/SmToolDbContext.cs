using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.NameTranslation;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Persistence.Tools;
using SpringMountain.Framework.Domain.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using SmTools.Api.Core.BookmarkCategories;
using SmTools.Api.Core.BookmarkItems;
using SmTools.Api.Core.Systems;
using SmTools.Api.Core.AiWebsites;

namespace SmTools.Api.Persistence;

/// <summary>
/// 数据库上下文
/// </summary>
public class SmToolDbContext : CoreDbContext
{
    /// <summary>
    /// 用户认证信息
    /// </summary>
    public DbSet<UserAuth> UserAuths { get; set; }

    /// <summary>
    /// 用户资料信息
    /// </summary>
    public DbSet<UserInfo> UserInfos { get; set; }

    /// <summary>
    /// 书签分类目录
    /// </summary>
    public DbSet<BookmarkCategory> BookmarkCategories { get; set; }

    /// <summary>
    /// 书签条目
    /// </summary>
    public DbSet<BookmarkItem> BookmarkItems { get; set; }

    /// <summary>
    /// 权限表
    /// </summary>
    public DbSet<Permission> Permissions { get; set; }

    /// <summary>
    /// 权限组表
    /// </summary>
    public DbSet<PermissionGroup> PermissionGroups { get; set; }

    /// <summary>
    /// 权限组-权限关系表
    /// </summary>
    public DbSet<PermissionGroupPermission> PermissionGroupPermissions { get; set; }
    
    /// <summary>
    /// 角色表
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// 角色-权限关系表
    /// </summary>
    public DbSet<RolePermission> RolePermissions { get; set; }  

    /// <summary>
    /// 角色-用户关系表
    /// </summary>
    public DbSet<RoleUser> RoleUsers { get; set; }

    /// <summary>
    /// AI 网站表
    /// </summary>
    public DbSet<AiWebsite> AiWebsites { get; set; }

    /// <summary>
    /// 收藏网站表
    /// </summary>
    public DbSet<FavoriteSite> FavoriteSites { get; set; }

    public SmToolDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var timeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToLocalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Local).ToUniversalTime());

        modelBuilder.HasDefaultSchema("public");
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var type in entityTypes)
        {
            var entity = modelBuilder.Entity(type.ClrType);
            
            // 添加表注释（新API方式）
            var tableComment = type.ClrType.GetCustomAttribute<CommentAttribute>()?.Comment;
            if (!string.IsNullOrWhiteSpace(tableComment))
            {
                entity.ToTable(t => t.HasComment(tableComment));
            }

            // 查看是否自定义了映射表
            var tableAttr = type.ClrType.GetCustomAttribute<TableAttribute>();
            if (string.IsNullOrWhiteSpace(tableAttr?.Name))
            {
                // 如果没有自定义映射表，则默认使用类名（转换为 snake_case）映射
                entity.ToTable(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(type.ClrType.Name));
            }

            var properties = type.ClrType.GetProperties()
                .Where(c => c.GetCustomAttribute<NotMappedAttribute>() == null);
            foreach (var property in properties)
            {
                var prop = entity.Property(property.Name);
                
                // 添加字段注释（新API方式）
                var propertyComment = property.GetCustomAttribute<CommentAttribute>()?.Comment;
                if (!string.IsNullOrWhiteSpace(propertyComment))
                {
                    prop.HasComment(propertyComment);
                }

                // 查看是否自定义了映射字段
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                if (string.IsNullOrWhiteSpace(columnAttribute?.Name))
                {
                    // 如果没有自定义映射字段，则默认使用属性名（转换为 snake_case）映射
                    prop.HasColumnName(NpgsqlSnakeCaseNameTranslator.ConvertToSnakeCase(property.Name));
                }

                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    prop.HasColumnType("timestamp").HasConversion(timeConverter);
                }

                if (columnAttribute?.TypeName == "jsonb")
                {
                    prop.HasConversion(EfValueTool.GetJsonValueConverterByType(property.PropertyType));
                    prop.Metadata.SetValueComparer(EfValueTool.GetListToJsonValueComparerByType(property.PropertyType));
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}