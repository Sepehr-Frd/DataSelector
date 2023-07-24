using DataSelector.Model.Models;
using Nest;

namespace DataSelector.ExternalService.ElasticSearch;

public class ElasticSearchService
{
    private readonly IElasticClient _elasticClient;
    
    private readonly string _indexName;

    public ElasticSearchService(IElasticClient client)
    {
        _elasticClient = client;
        _indexName = client.ConnectionSettings.DefaultIndex;
    }

    public BulkAllObservable<QuestionDocument> AddRange(List<QuestionDocument> data) =>
           _elasticClient.BulkAll(data, selector => selector
               .Index(_indexName)
               .BackOffRetries(3)
               .BackOffTime("30s")
               .RefreshOnCompleted()
               .MaxDegreeOfParallelism(50)
               .Size(10)
           );

    public static TypeMappingDescriptor<QuestionDocument> MapQuestionDocumentIndices(TypeMappingDescriptor<QuestionDocument> map) => map
        .AutoMap();

    public static AnalysisDescriptor Analysis(AnalysisDescriptor analysis) => analysis

           .Tokenizers(tokenizers => tokenizers
           .NGram("IrcNgram-tokenizer", t => t
           .MinGram(2)
           .MaxGram(3)
           .TokenChars(
               TokenChar.Letter,
               TokenChar.Digit,
               TokenChar.Symbol,
               TokenChar.Punctuation))
           .Pattern("IrcIndices-pattern", p => p.Pattern(@"\W+"))

           )

           .TokenFilters(tokenFilters => tokenFilters
           .UserDefined("IrcNgram-Tokenfilter", new NGramTokenFilter
           {
               MinGram = 2,
               MaxGram = 3

           })


           .WordDelimiter("IrcIndices-words", w => w
               .SplitOnCaseChange()
               .PreserveOriginal()
               .SplitOnNumerics()
               .GenerateNumberParts(false)
               .GenerateWordParts()

           )
           )



           .Analyzers(analyzers => analyzers
           .Custom("IrcPersian-analyzer", c => c
               .Tokenizer("IrcNgram-tokenizer")

               .Filters("IrcNgram-Tokenfilter", "lowercase", "decimal_digit", "arabic_normalization", "persian_normalization", "persian_stop")

           )
           .Custom("IrcEnglish-analyzer", c => c
           .Tokenizer("IrcNgram-tokenizer")
           .Filters("IrcNgram-Tokenfilter", "lowercase", "decimal_digit"))

           .Custom("IrcIndeces-keyword", c => c
               .Tokenizer("keyword")
           .Filters("lowercase")


           )

        .Custom("EquipmentPersian-keyword", c => c
               .Tokenizer("keyword")
           .Filters("lowercase")


           )
           );

}


