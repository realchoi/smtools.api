using Microsoft.AspNetCore.Mvc.Filters;
using SpringMountain.Framework.Uow;

namespace SmTools.Api.Filters;

/// <summary>
/// 工作单元过滤器，为了统一数据库事务
/// </summary>
public class UowFilter : IAsyncActionFilter
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UowFilter(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    /// <summary>
    /// 在 Action 执行之前、模型绑定之后执行该方法。
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        using var scope = _unitOfWorkManager.Begin(new UnitOfWorkOptions(true));
        var res = await next();
        // 如果 Action 发生异常，回滚数据库操作
        if (res.Exception != null)
        {
            await scope.RollbackAsync();
        }
        // 如果 Action 执行正常，则提交数据库
        else
        {
            await scope.CompleteAsync();
        }
    }
}
