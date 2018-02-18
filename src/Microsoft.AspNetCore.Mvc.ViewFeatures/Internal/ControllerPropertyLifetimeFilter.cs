// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures.Internal
{
    internal class ControllerPropertyLifetimeFilter : IActionFilter, IResultFilter
    {
        private readonly IPropertyLifetimeThingie _lifetimeThingie;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly ControllerViewDataDictionaryFactory _viewDataFactory;

        public ControllerPropertyLifetimeFilter(
            IPropertyLifetimeThingie lifetimeThingie, 
            ITempDataDictionaryFactory tempDataFactory, 
            ControllerViewDataDictionaryFactory viewDataFactory)
        {
            _lifetimeThingie = lifetimeThingie;
            _tempDataFactory = tempDataFactory;
            _viewDataFactory = viewDataFactory;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            var viewData = _viewDataFactory.GetViewDataDictionary(context);

            var lifeTimeContext = new PropertyLifetimeContext(tempData, viewData);
            _lifetimeThingie.Populate(context.Controller, lifeTimeContext);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            var viewData = _viewDataFactory.GetViewDataDictionary(context);

            var lifeTimeContext = new PropertyLifetimeContext(tempData, viewData);
            _lifetimeThingie.Save(context.Controller, lifeTimeContext);
        }
    }
}

