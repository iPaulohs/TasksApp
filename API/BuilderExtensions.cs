using Application.Mappings;
using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using Domain.Abstract;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistence;
using Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Auth;
using System.Reflection;
using System.Text;

namespace API
{
    public static class BuilderExtensions
    {
        public static void AddSwaggerDocs(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tasks App",
                    Description = "Um aplicativo de tarefas baseado no Trello e escrito em ASP.NET Core V8.",
                    Contact = new OpenApiContact
                    {
                        Name = "Examplo de página de contato",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Examplo de página de licença",
                        Url = new Uri("https://example.com/license")
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
        public static void AddAppServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            });
        }

        public static void AddPersistence(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TasksDefaulConnectionString")));
        }

        public static void AddAppInjections(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            builder.Services.AddScoped<IListRepository, ListRepository>();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
        }
    }
}
