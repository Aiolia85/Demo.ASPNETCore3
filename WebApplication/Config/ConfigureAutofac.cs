﻿using Autofac;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Reoository.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication
{
    public class ConfigureAutofac : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            var appSetting = configuration.Get<AppSetting>();

            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<SQLiteDbContext>().UseSqlite(appSetting.ConnectionStrings.DefaultSQLite);

                containerBuilder.RegisterType<SQLiteDbContext>()
                .WithParameter("options", dbContextOptionsBuilder.Options)
                .InstancePerLifetimeScope();
            }

            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<LocalDBContext>(new DbContextOptions<LocalDBContext>()).UseSqlServer(appSetting.ConnectionStrings.LocalDB);

                containerBuilder.RegisterType<LocalDBContext>()
                .WithParameter("options", dbContextOptionsBuilder.Options)
                .InstancePerLifetimeScope();
            }
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<LocalDBContext1>(new DbContextOptions<LocalDBContext1>()).UseSqlServer(appSetting.ConnectionStrings.LocalDB1);

                containerBuilder.RegisterType<LocalDBContext1>()
                .WithParameter("options", dbContextOptionsBuilder.Options)
                .InstancePerLifetimeScope();
            }
            Assembly Repository = Assembly.Load("Reoository.EF");
            Assembly IRepository = Assembly.Load("Repository");
            containerBuilder.RegisterAssemblyTypes(Repository, IRepository)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().PropertiesAutowired();

            //var test=containerBuilder.Build().Resolve<SQLiteDbContext>();
            //UserInfo ui = new UserInfo();
            //ui.CreateDate = DateTime.Now;
            //ui.Username = "testName";
            //ui.Password = "password";
            //test.Users.Add(ui);
            //test.SaveChanges();
            //var ddd=test.SaveChangesAsync().Result;


            //直接注册某一个类和接口
            //左边的是实现类，右边的As是接口
            //containerBuilder.RegisterType<TestServiceE>().As<ITestServiceE>().SingleInstance();


            #region 方法1   Load 适用于无接口注入
            //var assemblysServices = Assembly.Load("Exercise.Services");

            //containerBuilder.RegisterAssemblyTypes(assemblysServices)
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope();

            //var assemblysRepository = Assembly.Load("Exercise.Repository");

            //containerBuilder.RegisterAssemblyTypes(assemblysRepository)
            //          .AsImplementedInterfaces()
            //          .InstancePerLifetimeScope();

            #endregion

            #region 方法2  选择性注入 与方法1 一样
            //            Assembly Repository = Assembly.Load("Exercise.Repository");
            //            Assembly IRepository = Assembly.Load("Exercise.IRepository");
            //            containerBuilder.RegisterAssemblyTypes(Repository, IRepository)
            //.Where(t => t.Name.EndsWith("Repository"))
            //.AsImplementedInterfaces().PropertiesAutowired();

            //            Assembly service = Assembly.Load("Exercise.Services");
            //            Assembly Iservice = Assembly.Load("Exercise.IServices");
            //            containerBuilder.RegisterAssemblyTypes(service, Iservice)
            //.Where(t => t.Name.EndsWith("Service"))
            //.AsImplementedInterfaces().PropertiesAutowired();
            #endregion

            #region 方法3  使用 LoadFile 加载服务层的程序集  将程序集生成到bin目录 实现解耦 不需要引用
            //获取项目路径
            //var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            //var ServicesDllFile = Path.Combine(basePath, "Reoository.EF.dll");//获取注入项目绝对路径
            //var assemblysServices = Assembly.LoadFile(ServicesDllFile);//直接采用加载文件的方法
            //containerBuilder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            //var RepositoryDllFile = Path.Combine(basePath, "Exercise.Repository.dll");
            //var RepositoryServices = Assembly.LoadFile(RepositoryDllFile);//直接采用加载文件的方法
            //containerBuilder.RegisterAssemblyTypes(RepositoryServices).AsImplementedInterfaces();
            #endregion


            #region 在控制器中使用属性依赖注入，其中注入属性必须标注为public
            //在控制器中使用属性依赖注入，其中注入属性必须标注为public
            //            var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
            //.Where(type => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type)).ToArray();
            //            containerBuilder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
            #endregion


        }
    }
}
