using System;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using YiSha.Cache;
using YiSha.Entity.SystemManage;

namespace YiSha.EntityListener.SystemManage
{
    public class MenuListener : IEntityChangedListener<MenuEntity>
    {
        public void OnChanged(MenuEntity newEntity, MenuEntity oldEntity, DbContext dbContext, Type dbContextLocator,
            EntityState state)
        {
            App.GetService<MenuCache>().Remove();
        }
    }
}
