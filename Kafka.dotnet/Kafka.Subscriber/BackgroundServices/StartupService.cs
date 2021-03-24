using KafkaService.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Subscriber.BackgroundServices
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

        private IKafkaService kafkaService
        {
            get
            {
                return this.serviceProvider.GetRequiredService<IKafkaService>();
            }
        }

        private ILogger<StartupService> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<StartupService>>();
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
            this.Logger.LogInformation($"Executing subscriber.");

            try
            {

                MessageHandlerDelegate<string> handler = this.HandleAsync;

                await this.kafkaService.ConsumeAsync<string>("myTopic", handler, stoppingToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Exception: {ex.GetType().FullName} | " + $"Message: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        private Task HandleAsync(string @event)
        {
            this.Logger.LogInformation($"Event received: {@event}");

            return Task.CompletedTask;
        }

        #endregion
    }
}
