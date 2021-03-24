using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaService.Core
{
    /// <summary>
    /// Defines a delegate that represents a handler capable of processing the specified <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The message type to be handled.</typeparam>
    /// <param name="message">The message.</param>
    public delegate Task MessageHandlerDelegate<T>(T message);
}
