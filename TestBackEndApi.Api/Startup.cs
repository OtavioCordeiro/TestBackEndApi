using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System;
using TestBackEndApi.Domain.Profiles;
using TestBackEndApi.Infrastructure.Services.Interfaces;
using TestBackEndApi.Infrastructure.Services.ServiceHandlers;

namespace TestBackEndApi.Api
{
    public class Startup
    {

        private Container container = new Container();
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;

        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("TestBackEndApi.Domain");

            services.AddMediatR(assembly);
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).AddXmlSerializerFormatters();
            services.AddCors();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #region IoC


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GetCepQueryResponseProfile());
                mc.AddProfile(new GetCepQueryRequestProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IViaCepServiceClient, ViaCepServiceClient>();

            #endregion IoC

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Test Back-End API",
                    Version = "v1",
                    Description = "Test Back-End API - OnlineApp.",
                    TermsOfService = new Uri("http://www.onlineapp.com.br")
                });
            });

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Test Back-End API");

            });

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
