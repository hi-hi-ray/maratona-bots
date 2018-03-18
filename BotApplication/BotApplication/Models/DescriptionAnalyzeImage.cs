using System;
using System.Collections.Generic;

namespace BotTranslatorUnicorn.Models
{
    [Serializable]
    public class DescriptionAnalyzeImage
    {
        public Adult adult { get; set; }
        public Description description { get; set; }
        public List<object> faces { get; set; }
        public string requestId { get; set; }
        public List<Tag> tags { get; set; }
    }

    [Serializable]
    public class Adult
    {
        public double adultScore { get; set; }
        public bool isAdultContent { get; set; }
        public bool isRacyContent { get; set; }
        public double racyScore { get; set; }
    }

    [Serializable]
    public class Caption
    {
        public double confidence { get; set; }
        public string text { get; set; }
    }

    [Serializable]
    public class Description
    {
        public List<Caption> captions { get; set; }
        public List<string> tags { get; set; }
    }

    [Serializable]
    public class Tag
    {
        public double confidence { get; set; }
        public string name { get; set; }
    }
}