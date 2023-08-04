using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MFJIANG.AliDashScopeSDK
{
    /// <summary>
    /// 文本聊天返回实体
    /// </summary>
    public class AigcTextResponseData
    {
        [JsonProperty("output")]
        public OutputData Output { get; set; } = new OutputData();
        [JsonProperty("usage")]
        public UsageData Usage { get; set; }
        [JsonProperty("request_id")]
        public string RequestId { get; set; } = "";
    }
    public class OutputData
    {
        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; } = "";
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
    public class UsageData
    {
        [JsonProperty("output_tokens")]
        public int OutputTokens { get; set; } = 0;
        [JsonProperty("input_tokens")]
        public int InputTokens { get; set; } = 0;
    }

    /// <summary>
    /// 聊天对话数据实体
    /// </summary>
    public class DialogueHistoryEntity
    {
        /// <summary>
        /// 用户的发言
        /// </summary>
        public string user { get; set; } = "";

        /// <summary>
        /// 机器人的发言
        /// </summary>
        public string bot { get; set; } = "";
    }
}
