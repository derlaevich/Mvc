// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;

namespace MvcSandbox.Controllers
{
    public class HomeController : Controller
    {
        [ModelBinder]
        public string Id { get; set; }

        [ViewData]
        public string ViewDataMessage { get; set; }

        [TempData]
        public string Stuff { get; set; }

        public IActionResult Index()
        {
            ViewDataMessage = "Stuff says " + Stuff;
            Stuff = "Time is now " + DateTime.Now;
            return View();
        }
    }
}
