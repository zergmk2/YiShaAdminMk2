using System;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using YiSha.Entity;

namespace YiSha.EntityListener.SystemManage
{
    public class CodeTempletListener : IEntityChangedListener<CodeTempletEntity>
    {
        public void OnChanged(CodeTempletEntity newEntity, CodeTempletEntity oldEntity, DbContext dbContext, Type dbContextLocator,
            EntityState state)
        {
        }
    }
}
