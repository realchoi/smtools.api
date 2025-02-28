using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Model.AiFavorites.Dtos;

/// <summary>
/// 新增/编辑 AI 网站收藏入参
/// </summary>
public class AddOrUpdateAiFavoriteInput : IdInput<string>
{
    /// <summary>
    /// 用户 Id，关联 user_info 表的主键
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 网站 Id，关联 ai_website 表的主键
    /// </summary>
    public string WebsiteId { get; set; }

    /// <summary>
    /// 必填项验证
    /// </summary>
    /// <exception cref="InvalidParameterException"></exception>
    public void Validate()
    {
        if (UserId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 Id 不能为空");
        }

        if (WebsiteId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("网站 Id 不能为空");
        }
    }
}