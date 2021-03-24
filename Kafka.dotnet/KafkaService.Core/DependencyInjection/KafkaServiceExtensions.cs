using KafkaService.Core.Config;
using KafkaService.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaService.Core.DependencyInjection
{
    /// <summary>
    /// Defines the Kafka service extensions class.
    /// </summary>
    public static class KafkaServiceExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds the Prisma service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaService(this IServiceCollection services)
        {
            // Validate

            if (services == null)
            {
                return null;
            }

            // Gets the options

            services
                .AddOptions()
                .AddOptionsSnapshot<KafkaServiceOptions>();

            services.AddLogging();

            services.TryAddTransient<IKafkaService, KafkaService>();

            return services;
        }

        #endregion Public Methods
    }
}
