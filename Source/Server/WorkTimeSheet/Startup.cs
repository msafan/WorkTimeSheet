using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using WorkTimeSheet.Authentication;
using WorkTimeSheet.DbModels;

namespace WorkTimeSheet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var jwtSecutiyScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecutiyScheme.Reference.Id, jwtSecutiyScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecutiyScheme, new string[] { }}
                });

                var apiKeyScheme = new OpenApiSecurityScheme
                {
                    Name = "ApiKey",
                    Description = "Enter api key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey",
                    Reference = new OpenApiReference
                    {
                        Id = "ApiKey",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(apiKeyScheme.Reference.Id, apiKeyScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {apiKeyScheme, new string[] { }}
                });
            });

            services.AddDbContext<IDbContext, WorkTimeSheetContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnectionString")));

            services.AddTransient<IJwtAuthenticationManager, JwtAuthenticationManager>(serviceProvider =>
            {
                return new JwtAuthenticationManager(serviceProvider.GetService<IDbContext>());
            });
            services.AddTransient<ITokenRefresher, TokenRefresher>(serviceProvider =>
            {
                return new TokenRefresher(serviceProvider.GetService<IDbContext>(),
                    serviceProvider.GetService<IJwtAuthenticationManager>(),
                    serviceProvider.GetService<IMapper>());
            });

            services.AddAuthentication()
                .AddApiKeySupport(options => { })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Constants.JwtTokenKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(ApiKeySchemeOptions.DefaultScheme,
                    JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy(Constants.UserRoleMember, policyBuilder => { policyBuilder.RequireRole(Constants.UserRoleMember); });
                option.AddPolicy(Constants.UserRoleOwner, policyBuilder => { policyBuilder.RequireRole(Constants.UserRoleOwner); });
                option.AddPolicy(Constants.UserRoleProjectManager, policyBuilder => { policyBuilder.RequireRole(Constants.UserRoleProjectManager); });
                option.AddPolicy("OwnerOrProjectManager", policyBuilder => { policyBuilder.RequireRole(Constants.UserRoleProjectManager, Constants.UserRoleOwner); });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{id}",
                    defaults: new { id = System.Web.Http.RouteParameter.Optional });
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
