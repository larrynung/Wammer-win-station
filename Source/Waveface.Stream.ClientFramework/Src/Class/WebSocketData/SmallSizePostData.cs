﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Waveface.Stream.ClientFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class SmallSizePostData
    {
        #region Public Property
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeStamp { get; set; }
        #endregion


        #region Public Method
        /// <summary>
        /// Shoulds the type of the serialize.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeType()
        {
            return Type != null && Type.Length > 0;
        }

        /// <summary>
        /// Shoulds the serialize time stamp.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTimeStamp()
        {
            return TimeStamp != null && TimeStamp.Length > 0;
        }

        /// <summary>
        /// Shoulds the serialize ID.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeID()
        {
            return ID != null && ID.Length > 0;
        }
        #endregion
    }
}