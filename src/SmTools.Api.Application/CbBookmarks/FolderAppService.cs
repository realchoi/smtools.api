using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.CbBookmarks;
using SmTools.Api.Model.CbBookmarks.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;

namespace SmTools.Api.Application.CbBookmarks;

/// <summary>
/// 书签文件夹服务实现类
/// </summary>
public class FolderAppService : IFolderAppService
{
    private readonly IRepository<Folder, long> _folderRepository;

    public FolderAppService(IRepository<Folder, long> folderRepository)
    {
        _folderRepository = folderRepository;
    }

    /// <summary>
    /// 获取用户的书签文件夹树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<List<FolderTreeDto>> GetFolderTree(string userId)
    {
        if (userId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(userId, out var userIdValue))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        var folders = await _folderRepository.GetQueryable()
            .Where(p => p.UserId == userIdValue)
            .AsNoTracking().ToListAsync();
        var tree = RenderFolderTree(null);
        return tree;

        List<FolderTreeDto> RenderFolderTree(long? parentId = null)
        {
            return folders.Where(p => p.ParentId == parentId)
                .Select(c =>
                {
                    var children = RenderFolderTree(c.Id);
                    var item = new FolderTreeDto
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Children = children
                    };
                    return item;
                }).ToList();
        }
    }
}