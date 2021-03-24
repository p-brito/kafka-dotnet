using KafkaService.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Publisher.BackgroundServices
{
    /// <summary>
    /// Defines the startup service class.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
    internal sealed class StartupService : BackgroundService
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Private Properties

        private ILogger<StartupService> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<StartupService>>();
            }
        }

        private IKafkaService kafkaService
        {
            get
            {
                return this.serviceProvider.GetRequiredService<IKafkaService>();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public StartupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Protect Methods

        /// <summary>
        /// This method is called when the <see cref="IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="StopAsync(CancellationToken)" /> is called.</param>
        /// <returns>
        /// A <see cref="Task" /> that represents the long running operations.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                int count = 1;

                while (!stoppingToken.IsCancellationRequested)
                {
                    await this.kafkaService.ProduceAsync($"Hi! I'm the event number {count}", "myTopic", stoppingToken).ConfigureAwait(false);

                    count++;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Exception: {ex.GetType().FullName} | " +
                             $"Message: {ex.Message}");
            }
        }

        #endregion
    }
}
