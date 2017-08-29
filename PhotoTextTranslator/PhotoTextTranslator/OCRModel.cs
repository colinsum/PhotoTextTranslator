using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoTextTranslator
{
    public class OCRModel
    {
        public string language { get; set; }
        public List<regions> regions { get; set; }
    }

    public class regions
    {
        public string boundingbox { get; set; }
        public List<lines> lines { get; set; }
    }

    public class lines
    {
        public string boundingbox { get; set; }
        public List<words> words { get; set; }
    }

    public class words
    {
        public string boundingbox { get; set; }
        public string text { get; set; }
    }
}
