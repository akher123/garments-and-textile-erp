using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SCERP.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SCERP.Web.App_Start.NinjectWebCommon), "Stop")]

namespace SCERP.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using DAL;
    using Ninject.Extensions.Conventions;
    using BLL.IManager.IPlanningManager;
    using BLL.Manager.PlanningManager;
    using DAL.IRepository.IPlanningRepository;
    using DAL.Repository.Planning;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<SCERPDBContext>().To<SCERPDBContext>().InRequestScope();
            kernel.Bind(x => x.FromAssemblyContaining<IProcessManager>().SelectAllClasses().BindDefaultInterfaces());
            kernel.Bind(x => x.FromAssemblyContaining<IProcessRepository>().SelectAllClasses().BindDefaultInterfaces());

        }        
    }
}
