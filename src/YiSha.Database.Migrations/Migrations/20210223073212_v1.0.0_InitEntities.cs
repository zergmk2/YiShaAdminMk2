using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YiSha.Database.Migrations.Migrations
{
    public partial class v100_InitEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseAreaEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProvinceId = table.Column<long>(type: "INTEGER", nullable: true),
                    CityId = table.Column<long>(type: "INTEGER", nullable: true),
                    CountyId = table.Column<long>(type: "INTEGER", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseAreaEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysApiAuthorize",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Authorize = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysApiAuthorize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysArea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AreaCode = table.Column<string>(type: "TEXT", nullable: true),
                    ParentAreaCode = table.Column<string>(type: "TEXT", nullable: true),
                    AreaName = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    AreaLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysAutoJob",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobGroupName = table.Column<string>(type: "TEXT", nullable: true),
                    JobName = table.Column<string>(type: "TEXT", nullable: true),
                    JobStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    CronExpression = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NextStartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysAutoJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysAutoJobLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobGroupName = table.Column<string>(type: "TEXT", nullable: true),
                    JobName = table.Column<string>(type: "TEXT", nullable: true),
                    LogStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysAutoJobLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCodeTemplet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Flag = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCodeTemplet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDataDict",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    DictType = table.Column<string>(type: "TEXT", nullable: true),
                    DictSort = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDataDict", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDataDictDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DictType = table.Column<string>(type: "TEXT", nullable: true),
                    DictSort = table.Column<int>(type: "INTEGER", nullable: true),
                    DictKey = table.Column<int>(type: "INTEGER", nullable: true),
                    DictValue = table.Column<string>(type: "TEXT", nullable: true),
                    ListClass = table.Column<string>(type: "TEXT", nullable: true),
                    DictStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDefault = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDataDictDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDepartment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: true),
                    DepartmentName = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Fax = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PrincipalId = table.Column<long>(type: "INTEGER", nullable: true),
                    DepartmentSort = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: true),
                    PrincipalPhone = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLogApi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    LogStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteParam = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteResult = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteTime = table.Column<int>(type: "INTEGER", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLogApi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLogLogin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    LogStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IpLocation = table.Column<string>(type: "TEXT", nullable: true),
                    Browser = table.Column<string>(type: "TEXT", nullable: true),
                    OS = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    ExtraRemark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLogLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLogOperate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IpLocation = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    LogType = table.Column<string>(type: "TEXT", nullable: true),
                    BusinessType = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteParam = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteResult = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteTime = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLogOperate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: true),
                    MenuName = table.Column<string>(type: "TEXT", nullable: true),
                    MenuIcon = table.Column<string>(type: "TEXT", nullable: true),
                    MenuUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MenuTarget = table.Column<string>(type: "TEXT", nullable: true),
                    MenuSort = table.Column<int>(type: "INTEGER", nullable: false),
                    MenuType = table.Column<int>(type: "INTEGER", nullable: false),
                    MenuStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Authorize = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuAuthorize",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    MenuId = table.Column<long>(type: "INTEGER", nullable: true),
                    AuthorizeId = table.Column<long>(type: "INTEGER", nullable: true),
                    AuthorizeType = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuAuthorize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMqttMsg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ThemeName = table.Column<string>(type: "TEXT", nullable: true),
                    Msg = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMqttMsg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMqttTheme",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ThemeName = table.Column<string>(type: "TEXT", nullable: true),
                    IsSubscribe = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMqttTheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysPosition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    PositionName = table.Column<string>(type: "TEXT", nullable: true),
                    PositionSort = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    RoleName = table.Column<string>(type: "TEXT", nullable: true),
                    RoleSort = table.Column<int>(type: "INTEGER", nullable: true),
                    RoleStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsDelete = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Salt = table.Column<string>(type: "TEXT", nullable: true),
                    RealName = table.Column<string>(type: "TEXT", nullable: true),
                    DepartmentId = table.Column<long>(type: "INTEGER", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    Birthday = table.Column<string>(type: "TEXT", nullable: true),
                    Portrait = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", nullable: true),
                    QQ = table.Column<string>(type: "TEXT", nullable: true),
                    WeChat = table.Column<string>(type: "TEXT", nullable: true),
                    LoginCount = table.Column<int>(type: "INTEGER", nullable: true),
                    UserStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSystem = table.Column<int>(type: "INTEGER", nullable: true),
                    IsOnline = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstVisit = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PreviousVisit = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastVisit = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    ApiToken = table.Column<string>(type: "TEXT", nullable: true),
                    Picture = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUserBelong",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: true),
                    BelongId = table.Column<long>(type: "INTEGER", nullable: true),
                    BelongType = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserBelong", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseAreaEntity");

            migrationBuilder.DropTable(
                name: "SysApiAuthorize");

            migrationBuilder.DropTable(
                name: "SysArea");

            migrationBuilder.DropTable(
                name: "SysAutoJob");

            migrationBuilder.DropTable(
                name: "SysAutoJobLog");

            migrationBuilder.DropTable(
                name: "SysCodeTemplet");

            migrationBuilder.DropTable(
                name: "SysDataDict");

            migrationBuilder.DropTable(
                name: "SysDataDictDetail");

            migrationBuilder.DropTable(
                name: "SysDepartment");

            migrationBuilder.DropTable(
                name: "SysLogApi");

            migrationBuilder.DropTable(
                name: "SysLogLogin");

            migrationBuilder.DropTable(
                name: "SysLogOperate");

            migrationBuilder.DropTable(
                name: "SysMenu");

            migrationBuilder.DropTable(
                name: "SysMenuAuthorize");

            migrationBuilder.DropTable(
                name: "SysMqttMsg");

            migrationBuilder.DropTable(
                name: "SysMqttTheme");

            migrationBuilder.DropTable(
                name: "SysPosition");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysUser");

            migrationBuilder.DropTable(
                name: "SysUserBelong");
        }
    }
}
