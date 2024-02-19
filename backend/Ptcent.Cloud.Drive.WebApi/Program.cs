using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using Ptcent.Cloud.Drive.WebApi;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Repositories;
using Ptcent.Cloud.Drive.WebApi.Filters;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using Ptcent.Cloud.Drive.WebApi.Infrastructure.Extensions;
using Ptcent.Cloud.Drive.WebApi.Options;
using System.Text;
using System.Diagnostics;
using Ptcent.Cloud.Drive.WebApi.Infrastructure.RouteUtil;
using Minio.DataModel.Tags;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
//{
//    config.Sources.Clear();  oldд��
//    config.AddJsonFile("Configs/appsettings.json", optional: true, reloadOnChange: true);
//});

builder.Configuration.AddJsonFile("Configs/appsettings.json", optional: true, reloadOnChange: true);
// Add services to the container.
//��ֹ��������
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});
//IIS �ϴ����ֵ
builder.Services.Configure<FormOptions>(x =>
{

    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});
#region Minioע��
builder.Services.AddMinio(builder.Configuration["MinIO:Minio_AccessKey"], builder.Configuration["MinIO:Minio_SecretKey"]);
builder.Services.AddMinio(configureClient => configureClient
          .WithEndpoint(builder.Configuration["MinIO:Minio_Server"])
          .WithCredentials(builder.Configuration["MinIO:Minio_AccessKey"], builder.Configuration["MinIO:Minio_SecretKey"]));
#endregion
//ע��ѩ��ID  //ѩ��id ������ַhttps://github.com/yitter/IdGenerator
#region ѩ��ID
SnowIdOptions snowIdOptions = new SnowIdOptions
{
    Method = Convert.ToInt16(builder.Configuration["SnowId:Method"]),
    BaseTime = DateTime.Parse(builder.Configuration["SnowId:BaseTime"]),
    WorkerId = Convert.ToUInt16(builder.Configuration["SnowId:WorkerId"]),
    WorkerIdBitLength = (byte)int.Parse(builder.Configuration["SnowId:WorkerIdBitLength"]),
    SeqBitLength = (byte)int.Parse(builder.Configuration["SnowId:SeqBitLength"]),
    MaxSeqNumber = int.Parse(builder.Configuration["SnowId:MaxSeqNumber"]),
    MinSeqNumber = Convert.ToUInt16(builder.Configuration["SnowId:MinSeqNumber"]),
    TopOverCostCount = Convert.ToInt32(builder.Configuration["SnowId:TopOverCostCount"]),
    DataCenterId = Convert.ToUInt16(builder.Configuration["SnowId:DataCenterId"]),
    DataCenterIdBitLength = (byte)Convert.ToUInt16(builder.Configuration["SnowId:DataCenterIdBitLength"]),
    TimestampType = (byte)Convert.ToUInt16(builder.Configuration["SnowId:TimestampType"])
};
builder.Services.AddIdGenerator(snowIdOptions);
#endregion


builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);
builder.Services.AddControllers(options =>
{
    //ע��ȫ�ֹ�����
    options.Filters.Add(typeof(PtcentYiDocApiOperationFilter));
    options.Filters.Add(typeof(ExceptionFilterAttribute));
}).AddJsonOptions(config =>
{
    config.JsonSerializerOptions.PropertyNamingPolicy = null;
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    });
builder.Services.AddSwaggerGen(options =>
{

    options.CustomOperationIds(apiDesc =>
    {
        var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
        return controllerAction!.ControllerName + "-" + controllerAction.ActionName;
    });
    options.OperationFilter<AddTokenHeaderParameter>();
    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
    DirectoryInfo directory = new(basePath);
    FileInfo[] files = directory.GetFiles("*.xml");
    var xmls = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
    foreach (var item in xmls)
    {
        options.IncludeXmlComments(item, true);
    }
    options.DocumentFilter<SwaggerEnumFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CloudDrive����WebApi", Version = "v1" });
    //options.OperationFilter<BinaryPayloadFilter>();
});

//����
builder.Services.AddCors(option => option.AddPolicy("AllowCors", (_builder) =>
{
    _builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
//MemoryCache
builder.Services.AddMemoryCache(option =>
{
    option.Clock = new LocalClockDto();
});

builder.Services.AddHttpClient();

builder.Services.AddMvc(opt =>
{
    opt.UseCentralRoutePrefix(new RouteAttribute("cloudapi/"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //ȡ��˽Կ
    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //��֤������
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        //��֤������
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Authentication:Audience"],
        //��֤�Ƿ����
        ValidateLifetime = true,
        //��֤˽Կ
        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
    };
});
var connection = ConfigUtil.GetValue("PtcentYiDocUserWebApiConnection");
var serverVersion = ServerVersion.AutoDetect(connection);
builder.Services.AddDbContext<EFDbContext>(options => options.UseMySql(connection, serverVersion));// MySqlServerVersion.LatestSupportedServerVersion));
HttpContextUtil.serviceCollection = builder.Services;
//����HTTp
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacConfig());
});
builder.Logging.AddLog4Net("Configs/log4net.config");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
#region �� ��
app.UseCors("AllowCors");
//app.Urls.Add("http://localhost:9000");

#endregion

if (app.Environment.IsDevelopment() || Debugger.IsAttached)
{
    //�����м����������Swagger��ΪJSON�ս��
    app.UseSwagger();
    //�����м�������swagger-ui��ָ��Swagger JSON�ս��
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudDrive����WebApi");
        c.RoutePrefix = string.Empty;
    });
}

string fileRootPath = builder.Configuration["FileRootPath"];

string tempUploadFilePath = Path.Combine(fileRootPath, "TempFile");

if (!Directory.Exists(tempUploadFilePath))
{
    Directory.CreateDirectory(tempUploadFilePath);
}

string uploadFilePath = Path.Combine(fileRootPath, "Upload");

if (!Directory.Exists(uploadFilePath))
{
    Directory.CreateDirectory(uploadFilePath);//AppDomain.CurrentDomain.BaseDirectory
}
string sourceFilePath = Path.Combine(fileRootPath, "SourceFiles");

if (!Directory.Exists(sourceFilePath))
{
    Directory.CreateDirectory(sourceFilePath);
}

// Configure the HTTP request pipeline.

//��֤
app.UseAuthentication();//��ǰ ��Ȩ
app.UseAuthorization();//�ں�  ��Ȩ
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

//��� �ں��
app.UseRouting();
app.Use(async (k, next) =>
{
    if (k.Request.Method == "OPTIONS")
    {
        k.Response.StatusCode = 200;
        await k.Response.WriteAsync("ok");
    }
    await next();
});
app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});
LogUtil.Info($"item����ʼ����" + DateTime.Now);
app.Run();