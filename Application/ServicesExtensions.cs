using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application
{
    public static class ServicesExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            //registre automaticamente los mapeos que haga en esta biblioteca de clases
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //las sentencias de validacion van en esta bilbioteca de classes
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //version posterior 12.0 no son compatibles
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        }
    }
}
