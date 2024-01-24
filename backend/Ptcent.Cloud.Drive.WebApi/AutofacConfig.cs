using Autofac;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Ptcent.Cloud.Drive.WebApi
{
    public class AutofacConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.Register(x => x.Resolve<DbContext>()).As<IDbContext>().InstancePerLifetimeScope();
            //builder.Register(x => x.Resolve<DbContext>()).As<IUnitOfWork>().InstancePerLifetimeScope();
            //builder.RegisterType<DbContext>().InstancePerLifetimeScope();
            //builder.RegisterType<PtcentYiDocApiOperationFilter>().InstancePerLifetimeScope();
            //builder.RegisterType<WxLogin>().InstancePerLifetimeScope();  

            var iServices = Assembly.Load("Ptcent.Cloud.Drive.WebApi.Application");
            var iRepositories = Assembly.Load("Ptcent.Cloud.Drive.WebApi.Domain.IRepositories");
            var repositories = Assembly.Load("Ptcent.Cloud.Drive.WebApi.Domain.Repositories");
            var serviceImpls = Assembly.Load("Ptcent.Cloud.Drive.WebApi.Application");

            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(iServices, serviceImpls)
              .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("ServiceImpl"))
              .AsImplementedInterfaces();

            //根据名称约定（数据访问层的接口和实现均以Repos结尾），实现数据访问接口和数据访问实现的依赖
            builder.RegisterAssemblyTypes(iRepositories, repositories)
              .Where(t => t.Name.EndsWith("Repos") || t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();
            builder.RegisterType<JwtSecurityTokenHandler>().InstancePerLifetimeScope();


        }
    }
}
