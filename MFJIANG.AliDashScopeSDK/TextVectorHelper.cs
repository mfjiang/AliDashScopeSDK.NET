using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MFJIANG.AliDashScopeSDK
{
    /// <summary>
    /// 文本向量计算器
    /// </summary>
    public static class TextVectorHelper
    {
        static TextVectorHelper()
        {

        }

        /// <summary>
        /// 计算文本相似度
        /// </summary>
        /// <param name="textA">文本A</param>
        /// <param name="textB">文本B</param>
        /// <returns></returns>
        public static double CalculateCosineSimilarity(string textA, string textB)
        {
            double similarity = 0;

            // 计算文本向量值
            var vectorA = CalculateVector(textA);
            var vectorB = CalculateVector(textB);

            // 计算相似度值            
            similarity = CalculateCosineSimilarity(vectorA, vectorB);

            return similarity;
        }


        /// <summary>
        /// 计算文本相似度
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        static double CalculateCosineSimilarity(Dictionary<string, double> vectorA, Dictionary<string, double> vectorB)
        {
            var dotProduct = 0.0;
            var normA = 0.0;
            var normB = 0.0;

            foreach (var word in vectorA.Keys)
            {
                if (vectorB.ContainsKey(word))
                {
                    dotProduct += vectorA[word] * vectorB[word];
                }

                normA += Math.Pow(vectorA[word], 2);
            }

            foreach (var word in vectorB.Keys)
            {
                normB += Math.Pow(vectorB[word], 2);
            }

            var similarity = dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));

            return similarity;
        }

        /// <summary>
        /// 计算文本中的单词频率生成一个向量表
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static Dictionary<string, double> CalculateVector(string text)
        {
            var wordCounts = new Dictionary<string, double>();

            // 分割文本为单词列表
            var words = SplitTextIntoWords(text);

            // 统计每个单词的出现次数
            foreach (var word in words)
            {
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word] += 1.0;
                }
                else
                {
                    wordCounts[word] = 1.0;
                }
            }

            // 计算单词频率
            var totalWordsCount = words.Length;

            foreach (var word in wordCounts.Keys.ToList())
            {
                wordCounts[word] /= totalWordsCount;
            }

            return wordCounts;
        }

        /// <summary>
        /// 单词分割支持中英文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static string[] SplitTextIntoWords(string text)
        {
            // 使用正则表达式分割文本为单词
            //var words = Regex.Split(text, @"\W+");
            // 去除空白单词
            //words = words.Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();

            // 使用中文分词库 JiebaNet.Segmenter 进行中文分词
            var segmenter = new JiebaSegmenter();
            var words = segmenter.Cut(text).ToArray();
            return words;
        }
    }
}
