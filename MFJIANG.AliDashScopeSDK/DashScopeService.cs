using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MFJIANG.AliDashScopeSDK
{
    /// <summary>
    /// 阿里通义千问API服务
    /// </summary>
    public class DashScopeService
    {
        /// <summary>
        /// 返回DashScopeService实例
        /// </summary>
        /// <param name="apiKey"></param>
        public DashScopeService(string apiKey)
        {
            if (!String.IsNullOrEmpty(apiKey))
            {
                DashScopeConfig.SetMyKey(apiKey);
            }
        }

        /// <summary>
        /// 调用通义千问7B模型的文本对话接口
        /// </summary>
        /// <param name="histoy">对话历史</param>
        /// <param name="inputPrompt">提示词</param>
        public async Task<string> CallAigcText(List<DialogueHistoryEntity> histoy, string inputPrompt)
        {
            string rsp = "";
            var httpClient = new HttpClient();

            //using (httpClient)
            {
                //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {DashScopeConfig.Key}");
                //Misused header name, 'Content-Type'.Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {DashScopeConfig.Key}");                

                var requestData = new
                {
                    model = "qwen-v1",
                    input = new
                    {
                        prompt = inputPrompt,
                        history = histoy
                    },
                    parameters = new { }
                };

                var requestDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(DashScopeConfig.AigcTextApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    rsp = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    rsp = $"请求失败：{response.StatusCode}";
                }
            }

            return rsp;
        }

        /// <summary>
        /// 调用文本向量API
        /// </summary>
        /// <param name="inputText">文本数组</param>
        /// <returns></returns>
        public async Task<string> CallTextEmbedding(string[] inputText)
        {
            string result = "";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {DashScopeConfig.Key}");
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                var requestData = new
                {
                    model = "text-embedding-v1",
                    input = new
                    {
                        texts = inputText
                    },
                    parameters = new
                    {
                        text_type = "query"
                    }
                };

                var requestDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestDataJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(DashScopeConfig.EmbeddingApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception(($"请求失败：{response.StatusCode}"));
                }
            }

            return result;
        }

        /// <summary>
        /// 计算两个向量的相似度
        /// </summary>
        /// <param name="vectorA">向量A</param>
        /// <param name="vectorB">向量B</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public float CalculateCosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("向量长度不一致");

            var dotProduct = 0.0;
            var normA = 0.0;
            var normB = 0.0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += Math.Pow(vectorA[i], 2);
                normB += Math.Pow(vectorB[i], 2);
            }

            //余弦相似度公式计算两个向量之间的相似度值
            var similarity = (float)(dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB)));

            return similarity;
        }
    }
}
