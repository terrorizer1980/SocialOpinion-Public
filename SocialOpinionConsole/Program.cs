﻿using SocialOpinionAPI.Labs;
using SocialOpinionAPI.Models.TweetMetrics;
using SocialOpinionAPI.Services.TweetMetrics;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SocialOpinionConsole
{
    class Program
    {
        private static void FilteredStreamClient_FilteredStreamDataReceivedEvent(object sender, EventArgs e)
        {
            FilteredStreamClient.TweetReceivedEventArgs eventArgs = e as FilteredStreamClient.TweetReceivedEventArgs;

            string dataResponse = eventArgs.filteredStreamDataResponse;
            Console.WriteLine(dataResponse);
        }

        static void Main(string[] args)
        {
            string _ConsumerKey = ConfigurationManager.AppSettings.Get("ConsumerKey");
            string _ConsumerSecret = ConfigurationManager.AppSettings.Get("ConsumerSecret");
            string _AccessToken = ConfigurationManager.AppSettings.Get("AccessToken");
            string _AccessTokenSecret = ConfigurationManager.AppSettings.Get("AccessTokenSecret");


            // testing Metrics API with strongly typed objects and new Service class
            // that encapsulates low-level mapping from the Labs API
            List<string> ids = new List<string>();
            ids.Add("1258626243987230722");
            TweetMetricsService service = new TweetMetricsService(new SocialOpinionAPI.Core.OAuthInfo
                {
                    AccessSecret = _AccessTokenSecret,
                    AccessToken = _AccessToken,
                    ConsumerSecret = _ConsumerSecret,
                    ConsumerKey = _ConsumerKey
                });
            List<TweetMetricModel> metricModels = service.GetTweetMetrics(ids);

            // testing Filtered Stream Client
            FilteredStreamClient filteredStreamClient = new FilteredStreamClient("", "");
            filteredStreamClient.FilteredStreamDataReceivedEvent += FilteredStreamClient_FilteredStreamDataReceivedEvent;
            filteredStreamClient.StartStream("https://api.twitter.com/labs/1/tweets/stream/filter?tweet.format=detailed", 10, 5);
        }
    }
}
