using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaService.Core
{
    /// <summary>
    /// Defines the Kafka service interface.
    /// </summary>
    public interface IKafkaService
    {
        /// <summary>
        /// Produces the asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="msg">The MSG.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ProduceAsync<TMessage>(TMessage msg, string topic, CancellationToken cancellationToken = default);

        /// <summary>
        /// Consumes the message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ConsumeAsync<TMessage>(string topic, MessageHandlerDelegate<TMessage> handler, CancellationToken cancellationToken = default);
    }
}
