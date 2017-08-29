using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoTextTranslator
{
    public class TranslateManager
    {
        private string sentence;
        private static TranslateManager manager;

        public static TranslateManager createInstance
        {
            get
            {
                if (manager == null)
                {
                    manager = new TranslateManager();
                }
                return manager;
            }
        }

        public void setSentence(string s)
        {
            this.sentence = s;
        }

        public string getSentence()
        {
            return this.sentence;
        }
    }
}
