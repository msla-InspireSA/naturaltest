using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.TextAnalytics;


namespace naturaltest
{
    internal class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("4c4259a1713b48bc87fd83b05b7da59e");
        private static readonly Uri endpoint = new Uri("https://textanalyisnlp.cognitiveservices.azure.com/");

        // Example method for detecting sentiment from text 
        static void SentimentAnalysisExample(TextAnalyticsClient client)
        {
            string inputText = "micheal jackson is a bad man.";
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");

            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tText: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");
            }
        }

        // Example method for detecting opinions text 
        static void SentimentAnalysisWithOpinionMiningExample(TextAnalyticsClient client)
        {
            var documents = new List<string>
            {
                "The food and service were unacceptable, but the concierge were nice."
            };

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });

            foreach (AnalyzeSentimentResult review in reviews)
            {
                Console.WriteLine($"Document sentiment: {review.DocumentSentiment.Sentiment}\n");
                Console.WriteLine($"\tPositive score: {review.DocumentSentiment.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {review.DocumentSentiment.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {review.DocumentSentiment.ConfidenceScores.Neutral:0.00}\n");
                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    Console.WriteLine($"\tText: \"{sentence.Text}\"");
                    Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                    Console.WriteLine($"\tSentence positive score: {sentence.ConfidenceScores.Positive:0.00}");
                    Console.WriteLine($"\tSentence negative score: {sentence.ConfidenceScores.Negative:0.00}");
                    Console.WriteLine($"\tSentence neutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");

                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        Console.WriteLine($"\tTarget: {sentenceOpinion.Target.Text}, Value: {sentenceOpinion.Target.Sentiment}");
                        Console.WriteLine($"\tTarget positive score: {sentenceOpinion.Target.ConfidenceScores.Positive:0.00}");
                        Console.WriteLine($"\tTarget negative score: {sentenceOpinion.Target.ConfidenceScores.Negative:0.00}");
                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {
                            Console.WriteLine($"\t\tRelated Assessment: {assessment.Text}, Value: {assessment.Sentiment}");
                            Console.WriteLine($"\t\tRelated Assessment positive score: {assessment.ConfidenceScores.Positive:0.00}");
                            Console.WriteLine($"\t\tRelated Assessment negative score: {assessment.ConfidenceScores.Negative:0.00}");
                        }
                    }
                }
                Console.WriteLine($"\n");
            }
        }

        // Example method for detecting the language of text
        static void LanguageDetectionExample(TextAnalyticsClient client)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage("hello");
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            SentimentAnalysisExample(client);
            SentimentAnalysisWithOpinionMiningExample(client);
            LanguageDetectionExample(client);

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
