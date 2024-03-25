using Application.UserCQ.Commands;
using Domain.Entity;
using Infra.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Abstract;
using FluentValidation.AspNetCore;
using Application.UserCQ.Validators;
using Infra.Repository;
using Application.Mappings;

namespace API
{
    public static class BuilderExtensions
    {
        public static void AddAppServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
            });
            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });
        }

        public static void AddAppPersistence(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TasksDefaulConnectionString")));
        }

        public static void AddAppIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<TasksDbContext>().AddDefaultTokenProviders(); 
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
