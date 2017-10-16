using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class RandomWordApiProvider : ApiProvider, IRandomWordProvider
    {
        private readonly Random _random;
        private List<string> _words;
        private bool _isLoadingWords = false;
        private bool _hasLoadedWords = false;

        public RandomWordApiProvider(IApi api) : base(api)
        {
            _random = new Random(DateTime.Now.Millisecond);
            _words = new List<string>();
        }

        public async Task<string> GetRandomWord()
        {
            if (!_hasLoadedWords) await LoadWords();

            var index = _random.Next(0, _words.Count - 1);
            return _words[index];
        }

        private async Task LoadWords()
        {
            if (_hasLoadedWords) return;
            if (_isLoadingWords)
            {
                await Task.Delay(20);
                await LoadWords();
                return;
            }

            _isLoadingWords = true;

            _words = new List<string>();
            var assembly = typeof(App).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("AminoTools.EmbeddedResources.querywords.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        _words.Add(line);
                    }
                }
            }

            _hasLoadedWords = true;
        }
    }
}
