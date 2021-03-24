using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KafkaService.Core.Config
{    
    /// <summary>
    /// Defines the kafka configuration class.
    /// </summary>
    public sealed class KafkaServiceOptions
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summar>
        public string[] Topics { get; set; }
    }
}
