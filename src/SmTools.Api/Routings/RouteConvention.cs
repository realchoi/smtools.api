using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SmTools.Api.Routings;

public class RouteConvention : IApplicationModelConvention
{
    /// <summary>
    /// 定义一个路由前缀变量
    /// </summary>
    private readonly AttributeRouteModel _centralPrefix;

    /// <summary>
    /// 调用时传入指定的路由前缀
    /// </summary>
    /// <param name="routeTemplateProvider"></param>
	public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
    {
        _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
    }

    /// <summary>
    /// Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.
    /// </summary>
    /// <param name="application">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.</param>
    public void Apply(ApplicationModel application)
    {
        // 遍历所有的 Controller
        foreach (var controller in application.Controllers)
        {
            // 1. 已经标记了 RouteAttribute 的控制器
            // 如果在控制器中已经标注有路由了，则会在路由的前面再添加指定的路由内容
            var matchedSelectors = controller.Selectors.Where(p => p.AttributeRouteModel != null).ToList();
            if (matchedSelectors.Any())
            {
                foreach (var selectorModel in matchedSelectors)
                {
                    // 在当前路由上，再添加一个路由前缀
                    selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selectorModel.AttributeRouteModel);
                }
            }

            // 2. 没有标记 RouteAttribute 的控制器
            var unMatchedSelectors = controller.Selectors.Where(p => p.AttributeRouteModel == null).ToList();
            if (unMatchedSelectors.Any())
            {
                foreach (var selectorModel in unMatchedSelectors)
                {
                    // 直接添加路由前缀
                    selectorModel.AttributeRouteModel = _centralPrefix;
                }
            }
        }
    }
}
