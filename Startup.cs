using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShortcutsBotHost.MongoModels;
using ShortcutsBotHost.ShortcutBot;
using TgBotFramework;

namespace ShortcutsBotHost
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
            services.AddLogging();
            services.AddControllersWithViews();
            services.Configure<MongoSettings>(Configuration.GetSection("ShortcutBot"));
            services.AddScoped<MongoCrud<ShortcutUser>>();

            services.AddScoped<MongoCrud<ShortcutUser>>();
            services.AddScoped<InlineQueryHandler>();
            services.AddSingleton<TextEcho>();
            services.AddScoped<GetOrCreateUser>();
            services.AddSingleton<ExceptionCatcher<ShortcutBotContext>>();
            services.AddScoped<ChosenInlineResultHandler>();
            services.AddScoped<EditShortcutsCommand>();
            
            services.AddBotService<ShortcutBotContext>(Options.Create<BotSettings>(Configuration.GetSection("ShortcutBot").Get<BotSettings>()),
                builder => builder.UseLongPolling()
                    .SetPipeline(pipe => pipe
                        .Use<ExceptionCatcher<ShortcutBotContext>>()
                        .Use<GetOrCreateUser>()
                        .MapWhen(On.Message, message => message
                            .UseCommand<EditShortcutsCommand>("edit"))
                        .MapWhen<InlineQueryHandler>(On.InlineQuery)
                        .MapWhen<ChosenInlineResultHandler>(On.ChosenInlineResult)
                    
                    )
            );
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}