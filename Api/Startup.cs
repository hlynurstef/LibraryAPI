using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace Api {
#pragma warning disable 1591
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ();
            services.AddTransient<IBooksRepository, BooksRepository> ();
            services.AddTransient<IBooksService, BooksService> ();
            services.AddTransient<IUsersRepository, UsersRepository> ();
            services.AddTransient<IUsersService, UsersService> ();
            services.AddTransient<IReviewsRepository, ReviewsRepository> ();
            services.AddTransient<ISeedRepository, SeedRepository> ();
            services.AddTransient<ISeedService, SeedService> ();
            services.AddTransient<IReviewsService, ReviewsService> ();
            services.AddTransient<IRecommendationsRepository, RecommendationsRepository> ();
            services.AddTransient<IRecommendationsService, RecommendationsService> ();
            services.AddDbContext<AppDataContext> (options =>
                options.UseSqlite ("Data Source=../Repositories/LibraryAPI.db", b => b.MigrationsAssembly ("Api")));

            // Adding swagger stuff
            services.AddMvc ();
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1",
                    new Info {
                        Title = "Library API - Efribrú",
                            Version = "v1",
                            Description = "A Library API to simplify Jóhanna's management of her books",
                            Contact = new Contact {
                                Name = "Andri <andrii13@ru.is>, Guðjón <gudjonss12@ru.is>, Gunnar <gunnarg15@ru.is> & Hlynur <hlynurs15@ru.is>",
                                    Email = "library@mikligardur.com",
                                    Url = "http://library.mikligardur.com/"
                            }
                    });
                var filePath = Path.Combine (PlatformServices.Default.Application.ApplicationBasePath, "SWCapi.xml");
                c.IncludeXmlComments (filePath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc ();
        }
    }
#pragma warning restore 1591
}