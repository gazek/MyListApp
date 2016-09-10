using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System;
using Microsoft.Owin.Security.OAuth;
using MyListApp.Api.App_Start;
using MyListApp.Api.Providers;

[assembly: OwinStartup(typeof(MyListApp.Api.Startup))]
namespace MyListApp.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                //AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AuthorizationServerProvider()
            };

            // token generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}