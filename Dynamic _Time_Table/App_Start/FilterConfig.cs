﻿using System.Web;
using System.Web.Mvc;

namespace Dynamic__Time_Table
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}