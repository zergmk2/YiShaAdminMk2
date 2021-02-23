using System;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using YiSha.Cache;
using YiSha.Entity;

namespace YiSha.EntityListener.SystemManage
{
    public class MenuAuthorizeListener : IEntityChangedListener<MenuAuthorizeEntity>
    {
        public void OnChanged(MenuAuthorizeEntity newEntity, MenuAuthorizeEntity oldEntity, DbContext dbContext, Type dbContextLocator,
            EntityState state)
        {
            App.GetService<MenuAuthorizeCache>().Remove();
        }
    }
}
