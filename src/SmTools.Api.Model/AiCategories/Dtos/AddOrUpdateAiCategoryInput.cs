using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Model.AiCategories.Dtos;

/// <summary>
/// 新增/编辑 AI 网站分类入参
/// </summary>
public class AddOrUpdateAiCategoryInput : IdInput<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

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
    }
}