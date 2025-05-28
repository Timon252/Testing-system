using Autofac;
using Caliburn.Micro.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestingSystem.ViewModels;

namespace TestingSystem
{
    class Bootstrapper : AutofacBootstrapper<ShellViewModel>
    {

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<ShellViewModel>().SingleInstance();
            builder.RegisterType<AdminTreeViewModel>().SingleInstance();
            builder.RegisterType<UserTreeViewModel>().InstancePerDependency();


        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
