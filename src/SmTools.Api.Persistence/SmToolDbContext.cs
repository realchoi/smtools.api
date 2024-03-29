﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.NameTranslation;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Persistence.Tools;
using SpringMountain.Framework.Domain.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using SmTools.Api.Core.BookmarkCategories;
using SmTools.Api.Core.BookmarkItems;

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