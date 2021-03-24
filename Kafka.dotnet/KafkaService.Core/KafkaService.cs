using Confluent.Kafka;
using KafkaService.Core.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaService.Core
{
    /// <summary>
    /// Defines the implementation of kafka service interface.
    /// </summary>
    /// <seealso cref="KafkaService.Core.IKafkaService" />
    internal sealed class KafkaService : IKafkaService
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Private Properties

        private KafkaServiceOptions Options
        {
            get
            {
                return this.serviceProvider.GetRequiredService<KafkaServiceOptions>();
            }
        }

        private ILogger<IKafkaService> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<IKafkaService>>();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public KafkaService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task ProduceAsync<TMessage>(TMessage msg, string topic, CancellationToken cancellationToken = default)
        {
            try
            {
                ProducerConfig config = new()
                {
                    BootstrapServers = this.Options.Server
                };

                if (!this.Options.Topics.Any(_ => _ == topic))
                {
                    throw new Exception("The topic is not valid");
                }

                using IProducer<string, TMessage> producer = new ProducerBuilder<string, TMessage>(config).Build();

                string eventId = Guid.NewGuid().ToString();

                this.Logger.LogInformation($"Kafka services is publishing an event with the id {eventId}");

                Message<string, TMessage> @event = new()
                {
                    Key = eventId,
                    Value = msg,
                };

                await producer.ProduceAsync(topic, @event, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ConsumeAsync<TMessage>(string topic, MessageHandlerDelegate<TMessage> handler, CancellationToken cancellationToken = default)
        {
            try
            {
                if(!this.Options.Topics.Any(_=>_ == topic))
                {
                    throw new Exception("The topic is not valid");
                }

                ConsumerConfig config = new()
                {
                    BootstrapServers = this.Options.Server,
                    GroupId = $"{topic}-{Guid.NewGuid()}",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };


                using IConsumer<string, TMessage> consumer = new ConsumerBuilder<string, TMessage>(config).Build();

                consumer.Subscribe(topic);

                while(!cancellationToken.IsCancellationRequested)
                {
                    var @event = consumer.Consume(cancellationToken);

                    await handler(@event.Message.Value).ConfigureAwait(false);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
