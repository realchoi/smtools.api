using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Model.AiWebsites.Dtos;

/// <summary>
/// 新增/编辑 AI 网站入参
/// </summary>
public class AddOrUpdateAiWebsiteInput : IdInput<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 分类 Id
    /// </summary>
    public string CategoryId { get; set; }

    /// <summary>
    /// 网站地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 网站 Logo
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// 必填项验证
    /// </summary>
    /// <exception cref="InvalidParameterException"></exception>
    public void Validate()
    {
        if (Name.IsNullOrEmpty())
        {
            throw new InvalidParameterException("名称不能为空");
        }

        if (Url.IsNullOrEmpty())
        {
            throw new InvalidParameterException("网站地址不能为空");
        }
    }
}