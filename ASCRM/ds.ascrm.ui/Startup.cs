using Dapper;
using ds.core.common;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.document;
using ds.core.emailtemplates;
using ds.core.task;
using ds.core.uow;
using ds.portal.applications;
using ds.portal.applications.invites;
using ds.portal.calendar;
using ds.portal.tasks;
using ds.portal.dashboard;
using ds.portal.diary;
using ds.portal.documents;
using ds.portal.leads;
using ds.portal.report;
using ds.portal.sqlupgrade;
using ds.portal.ui.Services;
using ds.portal.users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using portal.data.repository;
using portal.data.repository.Interfaces;
using portal.data.repository.Repositories;
using portal.ui.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ds.portal.companies;
using crm.portal.Interfaces.CourseContent;
using crm.portal.Repositories.CourseContent;
using crm.portal.Interfaces.CourseSummary;
using crm.portal.Repositories.CourseSummary;
using crm.portal.Interfaces.ResourceLibrary;
using crm.portal.Repositories.ResourceLibrary;
using crm.portal.Repositories.Matrix;
using crm.portal.Interfaces.Matrix;
using crm.portal.Interfaces.MyDocument;
using crm.portal.Repositories.MyDocument;
using crm.portal.Interfaces.MyCPD;
using crm.portal.Repositories.MyCPD;
using crm.portal.Interfaces.User;
using crm.portal.Repositories.User;
using crm.portal.Interfaces.MyTab2;
using crm.portal.Repositories.MyTab2;
using crm.portal.Interfaces.Survey;
using crm.portal.Repositories.Survey;
using crm.portal.Repositories.Message;
using crm.portal.Interfaces.Message;
using crm.portal.Interfaces.WorkUpload;
using crm.portal.Repositories.WorkUpload;
using DinkToPdf;
using crm.osca.Interfaces.Learners;
using crm.osca.Repositories.Lerarners;

namespace portal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                             .AddJsonOptions(options =>
                                options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                            .AddSessionStateTempDataProvider();
            services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(
                                    Configuration.GetConnectionString("DefaultConnection")
            ));

            //TODO: MR ConfigureServices
            //Noticed auth wasn't working as intended so added cookie auth
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Account/Login/";
                    options.Cookie.Name = "PortalOneAuth";
                    options.Cookie.Expiration = TimeSpan.FromHours(3);
                }); 

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });


            services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMemoryCache(); // caching some items

            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IData_UOW, Data_UOW>(provider => new Data_UOW(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUOW, UOW>(provider => new UOW(Configuration.GetConnectionString("DefaultConnection")));
           services.AddScoped<IUOW_Osca, UOW_Osca>(provider => new UOW_Osca(Configuration.GetConnectionString("DefaultConnectionOsca")));
            services.AddScoped<IUOW_Portal, UOW_Portal>(provider => new UOW_Portal(Configuration.GetConnectionString("PortalConnection")));
           // services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddScoped<ILoginRepository, LoginRepository>();
            //services.AddScoped<IACTRepository, ACTRepository>();
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<ILead_ListRepository, Lead_ListRepository>();
            services.AddScoped<ILead_UserRepository, Lead_UserRepository>();
            services.AddScoped<IRoleAdminRepository, RoleAdminRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ISqlUpgradeRepository, SqlUpgradeRepository>();
            services.AddScoped<IDiaryRepository, DiaryRepository>();
            services.AddScoped<IDocument, Document>();
            services.AddScoped<ds.core.task.ITaskRepository, ds.core.task.TaskRepository>();
            services.AddScoped<IAppInvitesRepository, AppInvitesRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAssetsService, AssetsService>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            services.AddScoped<IQuestionValidationRepository, QuestionValidationRepository>();
            services.AddScoped<ICourseContentRepository, CourseContentRepository>();
            services.AddScoped<ICourseSummaryRepository, CourseSummaryRepository>();
            services.AddScoped<IResourceLibraryRepository, ResourceLibraryRepository>();
            services.AddScoped<IMatrixRepository, MatrixRepository>();
            services.AddScoped<IMyDocumentRepository, MyDocumentRepository>();
            services.AddScoped<IMyCPDRepository, MyCPDRepository>();
            services.AddScoped<IPortalUserRepository, PortalUserRepository>();
            services.AddScoped<IMyTab2Repository, MyTab2Repository>();
            services.AddScoped<ISurveyRepository, SurveyRepository>();
            services.AddScoped<IMassageRepository, MessageRepository>();
            services.AddScoped<IWorkUploadRepository, WorkUploadRepository>();
            services.AddScoped<ILearnersRepository, LearnersRepository>();
            services.AddCalendar();
            services.AddMyTask();

            //TODO services.AddSession();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.IdleTimeout = TimeSpan.FromHours(3);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddKendo();

            //services.AddControllers(options =>options.Filters.Add(new HttpResponseExceptionFilter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            string baseDir = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(baseDir, "App_Data"));

            var _api_source_url = GetSingleConfigValueByConfigKey("api_source_url");
            if (string.IsNullOrEmpty(_api_source_url))
            {
                _api_source_url = "https://www.accessskills.co.uk"; // "http://as.3db.uk";
            }

            
            var _isLog = GetSingleConfigValueByConfigKey("IsLogger");
            if (_isLog == "1")
            {
                loggerFactory.AddFile("Logs/myapp-{Date}.txt");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseCors(policy => policy
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithOrigins(_api_source_url)
                   .AllowCredentials());
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           //not needed  app.UseCookiePolicy();
            // app.UseCors(options => options.AllowAnyOrigin());
            // app.UseCors("MyPolicy");
            app.UseCors(
                options => options.WithOrigins(_api_source_url).AllowAnyMethod()
            );

            app.UseAuthentication();

            //enable session before MVC
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                   template: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");

                routes.MapRoute(
                    name: "UserProfile",
                    template: "{controller=UserProfile}/{action=Index}");

                routes.MapRoute(
                   name: "MyDiary",
                   template: "{controller=MyDiary}/{action=Index}");

                routes.MapRoute(
                   name: "Task",
                   template: "{controller=Task}/{action=Index}");
            });


            var cultureInfo = new CultureInfo("en-GB");
            cultureInfo.NumberFormat.CurrencySymbol = "£";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.Run(context =>
            {
                context.Response.StatusCode = 404;
                return Task.FromResult(0);
            });

        }

        private string GetSingleConfigValueByConfigKey(string config_key)
        {
            string value = "";
            var _conn = Configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_GetSingleValueByKey]";
                    object dynamicParams = new { config_key = config_key };
                    value = conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return value;
        }
    }
}
