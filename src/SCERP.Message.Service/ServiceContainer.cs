using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Message.Service
{
    public static class ServiceContainer
    {
        static List<ServiceHost> _services;

        static ServiceContainer()
        {
            _services = new List<ServiceHost>();
        }

        /// <summary>
        /// Create WCF service and change CommunicationState to 'Open'
        /// </summary>
        /// <returns></returns>
        public static List<string> Open()
        {
            // read service names from configuration file
            ServicesSection servicesSection = (ServicesSection)ConfigurationManager.GetSection("system.serviceModel/services");
            var serviceNames = (from ServiceElement service in servicesSection.Services
                                select service.Name).ToList();

            var endPoints = new List<string>();

            _services = typeof(ServiceContainer).Assembly.GetTypes()
                .Where(x => serviceNames.Contains(x.FullName))
                .Select(x => new ServiceHost(x))
                .ToList();

            foreach (var service in _services)
            {
                service.Open();
                endPoints.AddRange(service.BaseAddresses.Select(x => x.ToString()));
            }

            return endPoints;
        }

        /// <summary>
        /// Change CommunicationState to 'Closed'
        /// </summary>
        public static void Close()
        {
            foreach (var service in _services)
            {
                if (service.State != CommunicationState.Closed)
                {
                    service.Close();
                }
            }
        }
    }
}
