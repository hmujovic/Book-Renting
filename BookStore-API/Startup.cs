using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Hubs;
using BookStore_API.Mappings;
using BookStore_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BookStore_API
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
			services.AddSignalR();
			services.AddResponseCompression(opts =>
			{
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});
			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<DataContext>();

			services.AddCors(o =>
			{
				o.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});

			services.AddAutoMapper(typeof(Maps));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(o =>
				{
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["Jwt:Issuer"],
						ValidAudience = Configuration["Jwt:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
					};
				});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Book Store API",
					Version = "v1",
					Description = "This is an educational API for a Book Store"
				});

				var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xpath = Path.Combine(AppContext.BaseDirectory, xfile);
				c.IncludeXmlComments(xpath);
			});

			services.AddScoped<IMemberRepository, MemberRepository>();
			services.AddScoped<IBookRepository, BookRepository>();
			services.AddScoped<IRentalRepository, RentalRepository>();

			services.AddControllers().AddNewtonsoftJson(op =>
				op.SerializerSettings.ReferenceLoopHandling =
					Newtonsoft.Json.ReferenceLoopHandling.Ignore);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,
			IWebHostEnvironment env,
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager)
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

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Store API");
				c.RoutePrefix = "";
			});

			app.UseHttpsRedirection();

			app.UseCors("CorsPolicy");

			SeedData.Seed(userManager, roleManager).Wait();
			

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<BroadcastHub>("/broadcastHub");
			});
		}
	}
}
